using Codice.CM.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//[Serializable]
//public class G2_HintWordEditor
//{
//    public string HintWord;
//    public string Describes;
//}


[CustomEditor(typeof(G2_SO_DataInfoLevel))]
public class G2_LevelDataEditor : Editor
{
    G2_SO_DataInfoLevel levelData;
    int number_hint_word;
    private string add_so_describe;
    private string add_so_letter;

    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();

        levelData = (G2_SO_DataInfoLevel)target;
        levelData.so_index = EditorGUILayout.IntField("Level ID: ", levelData.so_index);
        levelData.so_sentence = EditorGUILayout.TextField("Sentence: ", levelData.so_sentence);
        var labelStyle = new GUIStyle(GUI.skin.label);
        labelStyle.stretchWidth = false;
        labelStyle.fixedWidth = 100;


        if (string.IsNullOrEmpty(levelData.so_sentence) || levelData.dataHintWord == null || levelData.dataHintWord.Count == 0)
        {
            number_hint_word = EditorGUILayout.IntField("Number hint word: ", number_hint_word);

            if (GUILayout.Button("Create hint word"))
            {
                levelData.dataHintWord = new List<SO_DataHintWord>();

                for (int i = 0; i < number_hint_word; i++)
                {
                    levelData.dataHintWord.Add(new SO_DataHintWord());
                }
                Save();
            }
        }
        else
        {
            GUILayout.Label("Hint Words", labelStyle);
            for (int i = 0; i < levelData.dataHintWord.Count;)
            {
                using (new EditorGUILayout.VerticalScope())
                {
                    var hintword = levelData.dataHintWord[i];
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        GUILayout.Label((i + 1).ToString(), labelStyle);
                        hintword.so_describe = EditorGUILayout.TextField(hintword.so_describe);
                        hintword.so_letter = EditorGUILayout.TextField(hintword.so_letter);

                        if (GUILayout.Button("X"))
                        {
                            levelData.dataHintWord.Remove(hintword);
                        }
                    }
                    i++;
                }
            }


            GUILayout.Label("Add Hint Word", labelStyle);
            add_so_describe = EditorGUILayout.TextField(add_so_describe);
            add_so_letter = EditorGUILayout.TextField(add_so_letter);
            if (GUILayout.Button("Add hint word (+)"))
            {
                var addHintWord = new SO_DataHintWord(add_so_describe, add_so_letter);
                levelData.dataHintWord.Add(addHintWord);
                Save();
            }
        }


        if (GUILayout.Button("----Save----"))
        {
            Save();
        }

    }

    void Save()
    {
        EditorUtility.SetDirty(levelData);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
