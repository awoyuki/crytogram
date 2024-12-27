using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

public class G6_WordSnake
{
    [TableColumnWidth(60)]
    public string word;

    [TableColumnWidth(50)]
    public Color color;

    [HideInTables]
    public (int, int)[] indexes;

    [TableColumnWidth(60)]
    [ShowInInspector]
    [VerticalGroup("Actions")]
    public void SelectWord()
    {
        G6_SO_DataInfoLevel.current_word = this;
    }
}


[CreateAssetMenu(fileName = "G6_SO_DataInfoLevel_", menuName = "ScriptableObject/DataInfoLevel/G6_SO_DataInfoLevel")]
public class G6_SO_DataInfoLevel : SerializedScriptableObject
{
    public int so_index;

    [HideIf("Editing")]
    public int so_column;
    private int cached_column;

    [HideIf("Editing")]
    public int so_row;
    private int cached_row;

    [PropertySpace(SpaceBefore = 20, SpaceAfter = 20)]
    [ShowIf("Editing")]
    [OnValueChanged("RewriteData")]
    [TableMatrix(HorizontalTitle = "Word Snake Data", DrawElementMethod = "DrawElement", RowHeight = 20, Transpose = true)]
    public string[,] so_board_data;
    private string[,] cached_word_data;

    private bool Editing = false;

    [ReadOnly]
    [ShowInInspector]
    internal static G6_WordSnake current_word;

    [PropertySpace(SpaceBefore = 20, SpaceAfter = 20)]
    [ShowIf("Editing")]
    [TableList(ShowIndexLabels = true)]
    public List<G6_WordSnake> so_word_data;


    private static string DrawElement(Rect rect, string letter)
    {

        var style = new GUIStyle(GUI.skin.textField);
        style.alignment = TextAnchor.MiddleCenter;        

        if (current_word is not null && Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
        {
            GUI.changed = true;
            Event.current.Use();
        }

        letter = EditorGUI.TextField(rect, letter, style);
        return letter;
    }


    [HideIf("Editing")]
    [ShowInInspector]
    private void CreateNewWordDataTable()
    {
        Editing = true;
        so_board_data = new string[so_row, so_column];
        for (int i = 0; i < so_row; i++)
        {
            for (int j = 0; j < so_column; j++)
            {
                so_board_data[i, j] = "";
            }
        }
        if (cached_word_data is not null)
        {
            for (int i = 0; i < so_row; i++)
            {
                for (int j = 0; j < so_column; j++)
                {
                    if (i >= cached_row || j >= cached_column)
                        continue;
                    so_board_data[i, j] = cached_word_data[i, j];
                }
            }
        }
        SaveData();
    }


    [ShowIf("Editing")]
    [ShowInInspector]
    private void EditWordDataTable()
    {
        Editing = false;
        cached_column = so_column;
        cached_row = so_row;
    }

    [ShowInInspector]
    private void SaveData()
    {
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void RewriteData()
    {
        for (int i = 0; i < so_row; i++)
        {
            for (int j = 0; j < so_column; j++)
            {
                var item = so_board_data[i, j];
                if (!string.IsNullOrEmpty(item))
                    so_board_data[i, j] = item.ToUpper()[0].ToString();
            }
        }
    }


}
