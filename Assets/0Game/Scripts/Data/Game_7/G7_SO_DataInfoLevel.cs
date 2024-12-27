using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "G7_SO_DataInfoLevel", menuName = "ScriptableObject/DataInfoLevel/G7_SO_DataInfoLevel")]
public class G7_SO_DataInfoLevel : SerializedScriptableObject
{
    public int level_index;
    public int row_amount;
    public int col_amount;

    public bool is_base_old_broad = true;
    
    [TableMatrix(HorizontalTitle = "Broad", DrawElementMethod = "DrawColoredEnumElement", RowHeight = 3, SquareCells = true, ResizableColumns = false, Transpose = true)]
    public G7_SquareState[,] broad_state;
    
    #if UNITY_EDITOR
    
    private static G7_SquareState DrawColoredEnumElement(Rect rect, G7_SquareState value)
    {
        if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
        {
            value = UpdateStateValue(value);
            GUI.changed = true;
            Event.current.Use();
        }

        EditorGUI.DrawRect(rect.Padding(1), GetColorByStatus(value));
        return value;
    }

    [Sirenix.OdinInspector.Button("Create New Table")]
    public void CreateTable()
    {
        if (is_base_old_broad && broad_state != null)
        {
            G7_SquareState[,] copy_broad = broad_state;
            broad_state = new G7_SquareState[row_amount, col_amount];
            int minRows = Math.Min(row_amount, copy_broad.GetLength(0));
            int minCols = Math.Min(col_amount, copy_broad.GetLength(1));
            for (int i = 0; i < minRows; i++)
            {
                for(int j = 0; j < minCols; j++)
                    broad_state[i, j] = copy_broad[i, j];
            }
        }
        else
        {
            broad_state = new G7_SquareState[row_amount, col_amount];
        }
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }
    
    
    [Sirenix.OdinInspector.Button("Save")]
    public void Save()
    {
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }

    private static Color GetColorByStatus(G7_SquareState state)
    {
        switch (state)
        {
            case G7_SquareState.None:
                return Color.black;
            case G7_SquareState.Fill:
                return Color.green;
            case G7_SquareState.Cross:
                return Color.red;
        }
        return Color.black;
    }

    private static G7_SquareState UpdateStateValue(G7_SquareState value)
    {
        int cast_value = (int)value;
        cast_value++;
        if (cast_value > 2) cast_value = 0;

        return (G7_SquareState)cast_value;
    }
    #endif 

    /// <summary>
    /// Get Clue of Row Then Column
    /// </summary>
    public List<string> GetListClue()
    {
        List<string> result = new List<string>();
        for (int i = 0; i < row_amount; i++)
        {
            int count = 0;
            string clue = "";
            for (int j = 0; j < col_amount; j++)
            {
                if (broad_state[i, j] == G7_SquareState.Fill)
                {
                    count++;
                }
                else
                {
                    if (count != 0)
                    {
                        clue += count + ",";
                    }
                    count = 0;
                }
            }
            
            if(count > 0) clue += count + ",";
            if(clue.Length > 1) clue = clue.Substring(0, clue.Length - 1);
            result.Add(clue);
        }
        for (int i = 0; i < col_amount; i++)
        {
            int count = 0;
            string clue = "";
            for (int j = 0; j < row_amount; j++)
            {
                if (broad_state[j, i] == G7_SquareState.Fill)
                {
                    count++;
                }
                else
                {
                    if (count != 0)
                    {
                        clue += count + ",";
                    }
                    count = 0;
                }
            }
            if(count > 0) clue += count + ",";
            if(clue.Length > 1) clue = clue.Substring(0, clue.Length - 1);
            result.Add(clue);
        }

        return result;
    }
    
}



// [Sirenix.OdinInspector.Button("Export to Txt")]
// public void Export()
// {
//     string docPath =
//         Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
//     // Write the string array to a new file named "WriteLines.txt".
//     using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "G7_Level_"+level_index+".txt")))
//     {
//         outputFile.WriteLine(row_amount);
//         outputFile.WriteLine(col_amount);
//         var list = GetListClue();
//         foreach (string line in list)
//             outputFile.WriteLine(line);
//     }
// }
