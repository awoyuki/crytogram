using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class G7_Preview : MonoBehaviour
{
    [Header("DEV_INIT")] 
    [SerializeField] private List<G7_PreviewRow> list_row;
    [SerializeField] private Color clear_color = Color.white;

    public void FillPixel(int row, int col)
    {
        list_row[row].list_pixel[col].color = G7_Broad.Instance.color_cell_fill;
    }

    public void ClearPixel(int row, int col)
    {
        list_row[row].list_pixel[col].color = clear_color;
    }

    public void Restart()
    {
        foreach (var row in list_row)
        {
            row.Restart();
        }
    }
}
