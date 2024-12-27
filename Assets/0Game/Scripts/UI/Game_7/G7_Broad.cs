using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class G7_Broad : MonoBehaviour
{
    public static G7_Broad Instance;

    [Header("DEV_INIT")] 
    [SerializeField] private ButtonEffectLogic btn_fill;
    [SerializeField] private ButtonEffectLogic btn_cross;
    [SerializeField] private ButtonEffectLogic btn_restart;
    [SerializeField] private Image img_btn_fill;
    [SerializeField] private Image img_btn_cross;
    [SerializeField] private Text txt_check_wrong;
    [SerializeField] private Image img_picture;
    [Space] 
    [SerializeField] private List<G7_Clue> list_clue_of_row;
    [SerializeField] private List<G7_Clue> list_clue_of_column;
    [SerializeField] private List<G7_BroadRow> list_row;
    public G7_Preview preview;
    [Space]  
    [SerializeField] private Color color_txt_red;
    [SerializeField] private Color color_txt_green;
    public Color color_cell_fill;
    public Color color_clue_active;
    public Color color_clue_done;
    public Color color_brush_active;
    public Color color_brush_normal;
    public Color color_clue_hint;

    [Header("ATTRIBUTE")] 
    [SerializeField] private int row_amount;
    [SerializeField] private int column_amount;
    [SerializeField] private G7_SO_DataInfoLevel current_data;
    [SerializeField] private bool is_playing;
    public G7_SquareState current_draw_mode;

    private readonly Vector2 brush_normal_size = new Vector2(150, 150);
    private readonly Vector2 brush_active_size = new Vector2(170, 170);
    private const float check_result_show_duration = 3f;

    private Tween _tween;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }

        Instance = this;
    }

    private void Start()
    {
        btn_fill.onClick.AddListener(Button_Fill_Click);
        btn_cross.onClick.AddListener(Button_Cross_Click);
        btn_restart.onClick.AddListener(Button_Restart_Check);
    }

    /// <summary>
    /// Load 1 time when Create new level
    /// </summary>
    /// <param name="levelData"></param>
    public void LoadBroadData(G7_SO_DataInfoLevel levelData)
    {
        current_draw_mode = G7_SquareState.Fill;
        
        current_data = levelData;
        row_amount = levelData.row_amount;
        column_amount = levelData.col_amount;
        
        //---Load Clue---//
        //Clue of Row -> Column
        List<string> listClue = levelData.GetListClue();
        for (int i = 0; i < row_amount; i++)
        {
            list_clue_of_row[i].LoadClue(i, true, listClue[i]);
        }
        for (int i = row_amount; i < row_amount + column_amount; i++)
        {
            list_clue_of_column[i - row_amount].LoadClue(i - row_amount, false, listClue[i]);
        }
        //---___---//

        for (int i = 0; i < list_row.Count; i++)
        {
            list_row[i].Init(i);
        }
    }

    public void OnBroadButtonClick(G7_ButtonBroad buttonClick)
    {
        is_playing = true;
        //CheckAndUpdateRow
        CheckAndUpdateRow(buttonClick.row_index);
        CheckAndUpdateColumn(buttonClick.column_index);
    }

    public void CheckAndUpdateRow(int index)
    {
        var row_state = list_row[index].GetRowFillState();
        list_clue_of_row[index].UpdateState(row_state.Item1, row_state.Item2);
    }
    
    public void CheckAndUpdateColumn(int index)
    {
        string col_new_state = "";
        int count = 0;
        int count_empty = 0;
        foreach (var row in list_row)
        {
            if (row.list_button[index].current_state == G7_SquareState.Fill)
            {
                count++;
            }
            else
            {
                if (row.list_button[index].current_state == G7_SquareState.None) count_empty++;
                if (count > 0)
                {
                    col_new_state += count + ",";
                }

                count = 0;
            }
        }
        if (count > 0)
        {
            col_new_state += count + ",";
        }
        if(col_new_state.Length > 1) col_new_state = col_new_state.Substring(0, col_new_state.Length - 1);
        list_clue_of_column[index].UpdateState(col_new_state, count_empty);
    }

    public void DrawCrossAuto(bool is_row, int index)
    {
        if (is_row)
        {
            list_row[index].FillCross();
            return;
        }
        
        foreach (var row in list_row)
        {
            if (row.list_button[index].current_state == G7_SquareState.None)
            {
                row.list_button[index].DrawCross();
            }
        }
        
    }

    public void CheckWin()
    {
        foreach (var clue_col in list_clue_of_column)
        {
            if (!clue_col.is_complete)
            {
                return;
            }
        }

        foreach (var clue_row in list_clue_of_row)
        {
            if (!clue_row.is_complete)
            {
                return;
            }
        }

        //Win here
        G7_UIGamePlay.Instance.SetBanTouch(true);
        img_picture.color = new Color(1, 1, 1, 0);
        img_picture.gameObject.SetActive(true);
        img_picture.DOFade(1, 0.5f).SetDelay(1).OnComplete(() =>
        {
            DOVirtual.DelayedCall(1, () =>
            {
                G7_UIGamePlay.Instance.ShowEndPanel();
            });
        });
    }
    
    //Tool Support
    #region ToolSupport

    public void FindClueHint()
    {
        foreach (var clue_col in list_clue_of_column)
        {
            if (!clue_col.is_complete)
            {
                clue_col.ActiveHint();
                return;
            }
        }

        foreach (var clue_row in list_clue_of_row)
        {
            if (!clue_row.is_complete)
            {
                clue_row.ActiveHint();
                return;
            }
        }
    }

    public void DoHint(bool is_row, int index)
    {
        if (is_row){
            list_row[index].ShowAnswer();
            for (int i = 0; i < column_amount; i++)
            {
                CheckAndUpdateColumn(i);
            }
        }
        else
        {
            foreach (var row in list_row)
            {
                row.list_button[index].ShowAnswer();
            }
            CheckAndUpdateColumn(index);
            for (int i = 0; i < row_amount; i++)
            {
                CheckAndUpdateRow(i);
            }
        }

        
    }

    public G7_SquareState GetAnswerForCell(int rowIndex, int colIndex)
    {
        return current_data.broad_state[rowIndex, colIndex];
    }

    public void DoCheck()
    {
        int wrong_count = 0;
        for (int i = 0; i < row_amount; i++)
        {
            for (int j = 0; j < column_amount; j++)
            {
                if (list_row[i].list_button[j].current_state == G7_SquareState.Fill)
                {
                    if (current_data.broad_state[i, j] != G7_SquareState.Fill)
                    {
                        list_row[i].list_button[j].SetWrongAnswer();
                        wrong_count++;
                    }
                }
            }
        }

        txt_check_wrong.DOKill();
        _tween?.Kill();

        if (wrong_count > 0)
        {
            txt_check_wrong.color = color_txt_red;
            txt_check_wrong.text = wrong_count + " wrong uiAnswer found!!";
            txt_check_wrong.gameObject.SetActive(true);
        }
        else
        {
            txt_check_wrong.color = color_txt_green;
            txt_check_wrong.text = "Great, no wrong answers were found!";
            txt_check_wrong.gameObject.SetActive(true);
        }

        txt_check_wrong.DOFade(1, 0.25f).From(0);
        _tween = DOVirtual.DelayedCall(check_result_show_duration, () =>
        {
            txt_check_wrong.DOFade(0, 0.5f).OnComplete(() =>
            {
                txt_check_wrong.gameObject.SetActive(false);
            });
        });
    }
    #endregion
    
    //Button Click
    #region ButtonClick
    private void Button_Fill_Click()
    {
        if(current_draw_mode == G7_SquareState.Fill) return;
        
        current_draw_mode = G7_SquareState.Fill;
        
        img_btn_cross.color = color_brush_normal;
        img_btn_cross.rectTransform.sizeDelta = brush_normal_size;
        
        img_btn_fill.color = color_brush_active;
        img_btn_fill.rectTransform.sizeDelta = brush_active_size;
    }

    private void Button_Cross_Click()
    {
        if(current_draw_mode == G7_SquareState.Cross) return;
        
        current_draw_mode = G7_SquareState.Cross;
        
        img_btn_fill.color = color_brush_normal;
        img_btn_fill.rectTransform.sizeDelta = brush_normal_size;
        
        img_btn_cross.color = color_brush_active;
        img_btn_cross.rectTransform.sizeDelta = brush_active_size;
    }
    #endregion

    private void Button_Restart_Check()
    {
        if(!is_playing) return;
        
        preview.Restart();
        foreach (var clue_row in list_clue_of_row)
        {
            clue_row.Restart();
        }
        foreach (var clue_col in list_clue_of_column)
        {
            clue_col.Restart();
        }

        foreach (var row in list_row)
        {
            row.Restart();
        }
    }
}

