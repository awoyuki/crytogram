using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G7_Dev : MonoBehaviour
{
    public int length;
    public Color color;
    public RectTransform clue_col;
    public RectTransform clue_row;
    public RectTransform preview;
    public RectTransform play_zone;
    public RectTransform columns;
    public RectTransform rows;
    public List<G7_BroadRow> list_broad_row;

    [Space] 
    public Vector2 play_zone_size;


    private Vector2 broad_size = new Vector2(1000, 1000);
    [Sirenix.OdinInspector.Button]
    public void SetSize()
    {
        play_zone.sizeDelta = play_zone_size;
        clue_col.sizeDelta = new Vector2(play_zone_size.x, broad_size.y - play_zone_size.y);
        clue_row.sizeDelta = new Vector2(broad_size.x - play_zone_size.x, play_zone_size.y);
        preview.sizeDelta = broad_size - play_zone_size;
        columns.sizeDelta = new Vector2(play_zone_size.x, broad_size.y);
        rows.sizeDelta = new Vector2(broad_size.x, play_zone_size.y);
    }

    [Sirenix.OdinInspector.Button]
    public void SetBroadBGColor()
    {
        for (int i = 0; i < list_broad_row.Count; i++)
        {
            bool is_row = (i / (length )) % 2 == 0;
            for (int j = 0; j < list_broad_row[i].list_button.Count; j++)
            {
                bool is_col = (j / (length )) % 2 == 0;
                bool is_color = (is_row == is_col);
                if (is_color) list_broad_row[i].list_button[j].SetStartColor(color);
                else list_broad_row[i].list_button[j].SetStartColor(Color.white);
            }
        }
    }
}
