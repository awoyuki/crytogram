using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class G7_Clue : MonoBehaviour
{
    [Header("DEV_INIT")] 
    [SerializeField] private List<Text> list_text_clue;
    [SerializeField] private List<GameObject> list_slash;
    [SerializeField] private ButtonEffectLogic button;
    [SerializeField] private Image img_background;
    [SerializeField] private Color color_text_done;
    [SerializeField] private GameObject hint_mark;

    [Header("ATTRIBUTE")] 
    public int index;
    public bool is_complete;
    [SerializeField] private string clue_string;
    [SerializeField] private bool is_row;
    [SerializeField] private string[] clue_patterns;
    private Color bg_init_color;
    private Color color_text_init;
    private int start_index;

    private void Start()
    {
        button.onClick.AddListener(ClueClick);
        bg_init_color = img_background.color;
        color_text_init = list_text_clue[0].color;
    }

    public void LoadClue(int _index, bool isRow, string clueRaw)
    {
        button.enabled = false;
        this.index = _index;
        is_row = isRow;
        clue_string = clueRaw;
        clue_patterns = clueRaw.Split(",");
        int index = list_text_clue.Count - 1;
        for (int i = clue_patterns.Length - 1; i >= 0; i--)
        {
            list_text_clue[index].text = clue_patterns[i];
            index--;
        }

        start_index = index + 1;
    }

    public void UpdateState(string newState, int emptyAmount)
    {
        if (newState == clue_string) // Satisfy Clue
        {
            is_complete = true;
            G7_Broad.Instance.CheckWin();
            if (emptyAmount == 0) //And Complete line
            {
                button.enabled = false;
                img_background.color = G7_Broad.Instance.color_clue_done;
            }
            else//But not Complete line
            {
                button.enabled = true;
                img_background.color = G7_Broad.Instance.color_clue_active;
            }
        }
        else// Not satisfy Clue
        {
            is_complete = false;
            button.enabled = false;
            img_background.color = bg_init_color;
        }
        
        //Check clue is done
        int clue_done_from_start = 0;
        var line_state = newState.Split(",");
        //Check From Start
        for (int i = 0; i < clue_patterns.Length; i++)
        {
            if (i < line_state.Length && line_state[i] == clue_patterns[i])
            {
                clue_done_from_start++;
                list_text_clue[i + start_index].color = color_text_done;
                list_slash[i + start_index].SetActive(true);
            }
            else
            {
                list_text_clue[i + start_index].color = color_text_init;
                list_slash[i + start_index].SetActive(false);
            }
        }
        //CheckFrom End
        // if (clue_done_from_start < clue_patterns.Length - 1)
        // {
        int index = 0;
            for (int i = clue_patterns.Length - 1; i >= clue_done_from_start +1; i--)
            {
                if (line_state.Length - 1 - index >= 0 && line_state[line_state.Length - 1 - index] == clue_patterns[i])
                {
                    list_text_clue[i + start_index].color = color_text_done;
                    list_slash[i + start_index].SetActive(true);
                }
                else
                {
                    list_text_clue[i + start_index].color = color_text_init;
                    list_slash[i + start_index].SetActive(false);
                    break;
                }

                index++;
                // }
            }
    }

    public void Restart()
    {
        img_background.color = bg_init_color;
        button.enabled = false;
        foreach (var slash in list_slash)
        {
            slash.SetActive(false);
        }
    }
    public void ActiveHint()
    {
        hint_mark.SetActive(true);
        G7_Broad.Instance.DoHint(is_row, index);
    }
    private void ClueClick()
    {
        G7_Broad.Instance.DrawCrossAuto(is_row, index);
        button.enabled = false;
        img_background.color = G7_Broad.Instance.color_clue_done;
    }
}
