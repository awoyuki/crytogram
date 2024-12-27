using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//[CustomEditor(typeof(G7_SO_DataInfoLevel))]
public class G7_LevelDataEditor : Editor
{
    G7_SO_DataInfoLevel levelData;
    
    private bool is_generated_broad = false;
    
    //Call in update
    public override void OnInspectorGUI()
    {
        // levelData = (G7_SO_DataInfoLevel)target;
        
        EditorGUILayout.LabelField("Number Rows: ");
        levelData.row_amount = EditorGUILayout.IntField(levelData.row_amount);
        EditorGUILayout.LabelField("Number Rows: ");
        levelData.col_amount = EditorGUILayout.IntField(levelData.col_amount);
        
        if (GUILayout.Button("Generate"))
        {
            // levelData.broad_state = new SquareState[levelData.row_amount, levelData.col_amount];
        }

        //Draw Broad
        for (int i = 0; i < levelData.row_amount; i++)
        {
            using (var Horizontal = new EditorGUILayout.HorizontalScope(HorizontalLayoutStyle))
            {
                GUILayout.FlexibleSpace();
                for (int j = 0; j < levelData.col_amount; j++)
                {
                    using (var Vertical = new EditorGUILayout.VerticalScope())
                    {
                        // GUI.backgroundColor = GetColorByStatus(levelData.broad_state[i,j]);
                        if (GUILayout.Button("", CharacterButtonStyle))
                        {
                            Save();
                        }
                    }
                }
                GUILayout.FlexibleSpace();
            }
        }
    }


    private void ChangePixelClick(int row, int col)
    {
        
    }

    private Color GetColorByStatus(G7_SquareState state)
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


    void Save()
    {
        // EditorUtility.SetDirty(levelData);
        // AssetDatabase.SaveAssets();
        // AssetDatabase.Refresh();
    }
    
    GUIStyle CharacterButtonStyle
    {
        get
        {
            var style = new GUIStyle(GUI.skin.button);
            style.fixedWidth = 20;
            style.fixedHeight = 20;
            style.stretchWidth = false;

            return style;
        }
    }
    GUIStyle HorizontalLayoutStyle
    {
        get
        {
            var style = new GUIStyle();
            style.stretchWidth = false;
            style.alignment = TextAnchor.MiddleCenter;
            return style;
        }
    }
}
