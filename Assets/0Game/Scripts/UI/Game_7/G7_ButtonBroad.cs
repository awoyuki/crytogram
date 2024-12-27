using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class G7_ButtonBroad : MonoBehaviour
{
    [Header("DEV_INIT")] 
    [SerializeField] private ButtonEffectLogic button;
    [SerializeField] private Image img_bg;
    [SerializeField] private Image img_cross;
    [SerializeField] private GameObject img_wrong;

    [Header("ATTRIBUTE")]
    [SerializeField] private Color bg_init_color;
    public bool is_show_wrong;
    public G7_SquareState current_state;
    public int row_index;
    public int column_index;

    private float effect_duration = 0.25f;

    private float click_time_gap = 0.25f;
    private float last_click;

    private void Start()
    {
        button.onDown.AddListener(Button_Click);
        button.onEnter.AddListener(Button_Enter);
        //Fake
        LoadSelfData();
    }

    public void LoadSelfData()
    {
        bg_init_color = img_bg.color;
    }

    public void Init(int rowIndex, int colIndex)
    {
        row_index = rowIndex;
        column_index = colIndex;
    }
    
    public void Restart()
    {
        button.enabled = true;
        if (current_state == G7_SquareState.Fill)
        {
            img_bg.color = bg_init_color;
        }
        else if(current_state == G7_SquareState.Cross)
        {
            img_cross.gameObject.SetActive(false);
        }
    }
    
    private void Button_Click()
    {
        if(Input.touchCount > 1) return;
        
        if(Time.time - last_click < click_time_gap) return;

        last_click = Time.time;
        switch (G7_Broad.Instance.current_draw_mode)
        {
            case G7_SquareState.Fill:
                if (current_state == G7_SquareState.Fill)
                {
                    ClearFill();

                    if (is_show_wrong)
                    {
                        is_show_wrong = false;
                        img_wrong.SetActive(false);
                    }
                }
                else if(current_state == G7_SquareState.None)
                {
                    DrawFill();
                }
                break;
            case G7_SquareState.Cross:
                if (current_state == G7_SquareState.Cross)
                {
                    ClearCross();
                }
                else if(current_state == G7_SquareState.None)
                {
                    DrawCross();
                }
                break;
        }
        
        G7_Broad.Instance.OnBroadButtonClick(this);
    }

    private void Button_Enter()
    {
        if(Input.touchCount != 1) return;
        if(Input.GetTouch(0).phase is TouchPhase.Began or TouchPhase.Ended) return;
        Button_Click();
    }

    public void SetWrongAnswer()
    {
        is_show_wrong = true;
        img_wrong.SetActive(true);
    }
    public void DrawCross()
    {
        current_state = G7_SquareState.Cross;
        img_cross.gameObject.SetActive(true);
        img_cross.DOKill();
        img_cross.DOFade(1, effect_duration).From(0);
    }

    private void ClearCross()
    {
        current_state = G7_SquareState.None;
        img_cross.DOKill();
        img_cross.DOFade(0, effect_duration).OnComplete(() =>
        {
            img_cross.gameObject.SetActive(false);
        });
    }

    private void DrawFill()
    {
        current_state = G7_SquareState.Fill;
        img_bg.DOKill();
        img_bg.DOColor(G7_Broad.Instance.color_cell_fill, effect_duration);
        G7_Broad.Instance.preview.FillPixel(row_index, column_index);
    }

    private void ClearFill()
    {
        current_state = G7_SquareState.None;
        img_bg.DOKill();
        img_bg.DOColor(bg_init_color, effect_duration);
        G7_Broad.Instance.preview.ClearPixel(row_index, column_index);
    }

    public void ShowAnswer()
    {
        G7_SquareState answer = G7_Broad.Instance.GetAnswerForCell(row_index, column_index);

        switch (answer)
        {
            case G7_SquareState.Fill:
                if(current_state == G7_SquareState.Cross)
                    ClearCross();
                
                DrawFill();
                break;
            default:
                if(current_state == G7_SquareState.Fill)
                    ClearFill();
                
                DrawCross();
                break;
        }
        button.enabled = false;
    }
    public void SetStartColor(Color c)
    {
        img_bg.color = c;
    }
}
