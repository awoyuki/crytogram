using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class G7_BroadRow : MonoBehaviour
{
    [Header("DEV_INIT")] 
    public List<G7_ButtonBroad> list_button;

    [Header("ATTRIBUTE")] 
    public int index;

    public void Init(int _index)
    {
        index = _index;
        for (int i = 0; i < list_button.Count; i++)
        {
            list_button[i].Init(index, i);
        }
    }

    public (string, int) GetRowFillState()
    {
        string row_new_state = "";
        int count = 0;
        int count_empty = 0;
        foreach (var button in list_button)
        {
            if (button.current_state == G7_SquareState.Fill)
            {
                count++;
            }
            else
            {
                if (button.current_state == G7_SquareState.None) count_empty++;
                if (count > 0)
                {
                    row_new_state += count + ",";
                }

                count = 0;
            }
        }
        if (count > 0)
        {
            row_new_state += count + ",";
        }
        if(row_new_state.Length > 1) row_new_state = row_new_state.Substring(0, row_new_state.Length - 1);

        return (row_new_state, count_empty);
    }

    public void FillCross()
    {
        foreach (var button in list_button)
        {
            if (button.current_state == G7_SquareState.None)
            {
                button.DrawCross();
            }
        }
    }

    public void ShowAnswer()
    {
        foreach (var button in list_button)
        {
            button.ShowAnswer();
        }
        
        G7_Broad.Instance.CheckAndUpdateRow(index);
        
    }

    public void Restart()
    {
        foreach (var button in list_button)
        {
            button.Restart();
        }
    }
}
