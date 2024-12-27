using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEditorInternal.VersionControl;
using UnityEngine;

[SerializeField]
public class G4_CrytoLetter
{
    public string letter;
    public int row_id, col_id;
    public bool status;
    public List<int> word_id;
}



[CreateAssetMenu(fileName = "G4_SO_DataInfoLevel_", menuName = "ScriptableObject/DataInfoLevel/G4_SO_DataInfoLevel")]
public class G4_SO_DataInfoLevel : SerializedScriptableObject
{
    public int so_index;

    [HideIf("Editing")]
    public int so_column;
    private int cached_column;

    [HideIf("Editing")]
    public int so_row;
    private int cached_row;

    [PropertySpace(SpaceBefore = 20)]
    [ShowIf("Editing")]
    [TableMatrix(HorizontalTitle = "Word Data", DrawElementMethod = "DrawElement", RowHeight = 20, Transpose = true)]
    public G4_CrytoLetter[,] so_word_data;

    private G4_CrytoLetter[,] cached_word_data;

    [Space(20)]
    [ShowIf("Editing")]
    [ReadOnly]
    public string so_given_letters;

    [ShowIf("Editing")]
    [ReadOnly]
    public List<string> so_answer;

    [ValidateInput("CheckBonusWords", "The bonus words must be less than or equal in number to the maximum value between column and row.")]
    [OnValueChanged("UpperCaseBonus")]
    [ShowIf("Editing")]
    [PropertySpace(SpaceAfter = 20)]
    public List<string> so_bonus_word;

    private bool Editing = false;

    private static G4_CrytoLetter DrawElement(Rect rect, G4_CrytoLetter letter)
    {
        var style = new GUIStyle(GUI.skin.textField);
        style.alignment = TextAnchor.MiddleCenter;
        if (letter is not null)
        {
            letter.letter = EditorGUI.TextField(rect, letter.letter, style);
            if (!string.IsNullOrEmpty(letter.letter))
            {
                letter.letter = letter.letter.ToUpper()[0].ToString();
            }
            return letter;
        }
        letter = new G4_CrytoLetter();
        letter.status = false;
        letter.word_id = new List<int>();
        letter.letter = EditorGUI.TextField(rect, "", style);
        return letter;
    }

    [ShowIf("Editing")]
    [ShowInInspector]
    private void GeneratedAnswerAndSave()
    {
        so_answer.Clear();
        ClearWordIDOfLetter();
        int valid_word_count = 0;
        // Check Horizontal
        for (int i = 0; i < so_row; i++)
        {
            var letter_pool = new List<G4_CrytoLetter>();
            var cur_word = new StringBuilder();
            for (int j = 0; j < so_column; j++)
            {
                var target = so_word_data[i, j];
                CheckCreateValidWord(target, ref cur_word, letter_pool, ref valid_word_count);
            }
            CreateValidWord(ref cur_word, letter_pool, ref valid_word_count);
        }
        // Check Vertical
        for (int i = 0; i < so_column; i++)
        {
            var letter_pool = new List<G4_CrytoLetter>();
            var cur_word = new StringBuilder();
            for (int j = 0; j < so_row; j++)
            {
                var target = so_word_data[j, i];
                CheckCreateValidWord(target, ref cur_word, letter_pool, ref valid_word_count);
            }
            CreateValidWord(ref cur_word, letter_pool, ref valid_word_count);
        }

        //generate given letter
        GenertateGivenLetter();
        SaveData();
    }

    private void CheckCreateValidWord(G4_CrytoLetter target, ref StringBuilder cur_word, List<G4_CrytoLetter> letter_pool, ref int valid_word_count )
    {
        if (target is null || string.IsNullOrEmpty(target.letter))
        {
            if (cur_word.Length > 0)
            {
                CreateValidWord(ref cur_word, letter_pool, ref valid_word_count);
                cur_word = new StringBuilder();
            }
        }
        else
        {
            cur_word.Append(target.letter);
            letter_pool.Add(target);
        }
    }
    private void CreateValidWord(ref StringBuilder cur_word, List<G4_CrytoLetter> letter_pool, ref int valid_word_count)
    {
        var new_word = cur_word.ToString();
        if (new_word.Length > 1)
        {
            so_answer.Add(new_word);
            foreach (var item in letter_pool)
            {
                item.word_id.Add(valid_word_count);
            }
            valid_word_count++;
        }
        letter_pool.Clear();
    }
    private void ClearWordIDOfLetter()
    {
        for (int i = 0; i < so_row; i++)
        {
            for (int j = 0; j < so_column; j++)
            {
                so_word_data[i, j].word_id = new List<int>();
            }
        }
    }
    private void GenertateGivenLetter()
    {
        string longest = "";
        int max_lenght = 0;
        foreach (var item in so_answer)
        {
            if (item.Length > max_lenght)
            {
                max_lenght = item.Length;
                longest = item.ToString();
            }
        }
        so_given_letters = Consts.ShuffleString(longest);
    }



    [HideIf("Editing")]
    [ShowInInspector]
    private void CreateNewWordDataTable()
    {
        Editing = true;
        so_word_data = new G4_CrytoLetter[so_row, so_column];
        for (int i = 0; i < so_row; i++)
        {
            for (int j = 0; j < so_column; j++)
            {
                so_word_data[i, j] = new G4_CrytoLetter();
                so_word_data[i, j].row_id = i;
                so_word_data[i, j].col_id = j;
                so_word_data[i, j].word_id = new List<int>();
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
                    so_word_data[i, j] = cached_word_data[i, j];
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
        cached_word_data = new G4_CrytoLetter[so_row, so_column];
        cached_word_data = (G4_CrytoLetter[,])so_word_data.Clone();
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

    private void UpperCaseBonus()
    {
        for (int i = 0; i < so_bonus_word.Count; i++)
        {
            so_bonus_word[i] = so_bonus_word[i].ToUpper().Trim();
        }
    }

    private bool CheckGivenLetters(string value)
    {
        if(value.Length <= Mathf.Max(so_column, so_row))
            return true;
        return false;
    }

    private bool CheckBonusWords(List<string> value)
    {
        if (value is null)
            return true;
        foreach (var item in value)
        {
            if (!CheckGivenLetters(item))
                return false;
        }
        return true;
    }
}
