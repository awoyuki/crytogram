
using Codice.CM.Client.Differences;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(G1_SO_DataInfoLevel))]
public class G1_LevelDataEditor : Editor
{
    G1_SO_DataInfoLevel levelData;
    string sentence = "";
    //G1_CharacterCrypto currentCharacter;
    bool editing;
    int charsInLine = 10;
    float hardPercent = 0.2f;

    public override void OnInspectorGUI()
    {
        levelData = (G1_SO_DataInfoLevel)target;
        EditorGUILayout.LabelField("Level ID: ");
        levelData.so_index = EditorGUILayout.IntField(levelData.so_index);
        EditorGUILayout.LabelField("Author Name: ");
        levelData.so_authorName = EditorGUILayout.TextField(levelData.so_authorName);
        EditorGUILayout.LabelField("Describes: ");
        levelData.so_describe = EditorGUILayout.TextField(levelData.so_describe);
        EditorGUILayout.LabelField("Character In Line: ");
        charsInLine = EditorGUILayout.IntSlider(charsInLine, 10, 30);
        EditorGUILayout.LabelField("Generate Hard Percent: ");
        hardPercent = EditorGUILayout.Slider(hardPercent, 0, 1);

        EditorGUILayout.LabelField("Sentence: ");
        if (levelData.word_cryptos_array == null || levelData.word_cryptos_array.Length == 0 || editing)
        {
            sentence = EditorGUILayout.TextField(sentence.ToUpper());
            if (GUILayout.Button(editing ? "Save sentence (editing)" : "Create sentence"))
            {

                if (string.IsNullOrEmpty(sentence))
                {
                    Debug.LogError("char_crypto_list is null or empty");
                }
                else
                {
                    var word_array = sentence.Split(' ');
                    levelData.word_cryptos_array = new G2_WordCrypto[word_array.Length];
                    for (int i = 0; i < word_array.Length; i++)
                    {
                        G1_CharacterCrypto[] letter_crypto_array = new G1_CharacterCrypto[word_array[i].Length];
                        for (int j = 0; j < word_array[i].Length; j++)
                        {
                            var letter = word_array[i][j];
                            if (char.IsPunctuation(letter))
                                letter_crypto_array[j] = new G1_CharacterCrypto(letter, G1_LetterStatus.None);
                            else
                                letter_crypto_array[j] = new G1_CharacterCrypto(letter, G1_LetterStatus.Process);

                        }
                        levelData.word_cryptos_array[i] = new G2_WordCrypto(letter_crypto_array);
                    }

                    //Generated Default Level
                    GeneratedLevel(0.2f);

                    Debug.LogFormat("<color=#03fc0b>Create new sentence</color>");
                }
                if (editing)
                {
                    editing = false;
                }
                SaveData();
            }
        }
        else
        {            
            int wCached = 0;
            bool nextLine = true;
            int time_out = 0;
            while (nextLine)
            {
                int charPerLine = 0;
                using (var Horizontal = new EditorGUILayout.HorizontalScope(HorizontalLayoutStyle))
                {
                    GUILayout.FlexibleSpace();
                    for (int w = wCached; w < levelData.word_cryptos_array.Length; w++)
                    {
                        var word = levelData.word_cryptos_array[w];
                        using (var Vertical = new EditorGUILayout.VerticalScope())
                        {
                            GUI.backgroundColor = new Color32(102, 101, 102, 255);
                            if (GUILayout.Button(word.isLocked > 0 ? LockButton : UnLockButton))
                            {
                                word.isLocked = word.isLocked > 0 ? 0 : 4;
                            }
                            using (var HorizontalChild = new EditorGUILayout.HorizontalScope(HorizontalLayoutStyle))
                            {
                                for (int i = 0; i < word.character_array.Length; i++)
                                {
                                    var drawChar = word.character_array[i];
                                    GUI.backgroundColor = GetColorStatus(drawChar.status, word.isLocked > 0);
                                    if (char.IsPunctuation(drawChar.character))
                                    {
                                        GUILayout.Label(drawChar.character.ToString() + "\n ", CharacterButtonStyle);
                                    }
                                    else
                                    {
                                        if (GUILayout.Button($"{drawChar.character}\n{Consts.EditableLetterStatus.IndexOf(drawChar.status)}", CharacterButtonStyle))
                                        {
                                            ChangeLetterStatus(drawChar);
                                        }
                                        charPerLine++;
                                    }
                                }
                            }
                        }   
                        if (w >= levelData.word_cryptos_array.Length - 1)
                        {
                            nextLine = false;
                            break;
                        }
                        if (charPerLine >= charsInLine)
                        {
                            wCached = w + 1;
                            break;
                        }
                        GUILayout.Label(" ", CharacterLabelStyle);
                    }
                    GUILayout.FlexibleSpace();
                }
                time_out++;
                if(time_out > 60)
                {
                    Debug.LogError("Time out!");
                    nextLine = false;
                    break;
                }
                GUILayout.Space(10);
            }
            GUILayout.Space(20);
            using (var Horizontal = new EditorGUILayout.HorizontalScope(HorizontalLayoutStyle))
            {
                for (int i = 0; i < Consts.EditableLetterStatus.Count; i++)
                {
                    var letterStatus = Consts.EditableLetterStatus[i];
                    GUI.backgroundColor = GetColorStatus(letterStatus);
                    GUILayout.Button((i + ". " + letterStatus.ToString()));
                }
            }
            GUI.backgroundColor = Color.white;
            GUILayout.Space(20);
            if (CheckNeededKey() && CheckWordValid() && GUILayout.Button("---- SAVE ----"))
            {
                Debug.LogFormat("<color=#03fc0b>Save sentence</color>");
                SaveData();
            }
            GUILayout.Space(20);
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Edit sentence"))
            {
                sentence = "";
                string[] word_array = new string[levelData.word_cryptos_array.Length];
                for (int i = 0; i < word_array.Length; i++)
                {
                    foreach (var character in levelData.word_cryptos_array[i].character_array)
                    {
                        word_array[i] += character.character;
                    }
                }
                sentence = string.Join(" ", word_array);
                editing = true;
            }
            
            GUI.backgroundColor = Color.yellow;
            if (GUILayout.Button("Regenerated"))
            {
                GeneratedLevel(hardPercent);
            }

        }
    }

    Color GetColorStatus(G1_LetterStatus letterStatus, bool word_locked = false)
    {
        float dark_percent = 0;
        if (word_locked) dark_percent = 0.3f;
        switch (letterStatus)
        {
            case G1_LetterStatus.Process:
                return Color.Lerp(new Color32(140, 139, 140, 255), Color.black, dark_percent);
            case G1_LetterStatus.Lock:
                return Color.Lerp(new Color32(252, 165, 3, 255), Color.black, dark_percent);
            case G1_LetterStatus.DoubleLock:
                return Color.Lerp(new Color32(252, 128, 3, 255), Color.black, dark_percent);
            case G1_LetterStatus.HasKey:
                return Color.Lerp(Color.yellow, Color.black, dark_percent);
            case G1_LetterStatus.Completed:
                return Color.Lerp(Color.green, Color.black, dark_percent);
            case G1_LetterStatus.HasCollectable:
                return Color.Lerp(Color.cyan, Color.black, dark_percent);
        }
        return Color.Lerp(new Color32(140, 139, 140, 255), Color.black, dark_percent);
    }

    void SaveData()
    {

        EditorUtility.SetDirty(levelData);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    void ChangeLetterStatus(G1_CharacterCrypto currentCharacter)
    {
        var cur_status = currentCharacter.status;
        if (Consts.EditableLetterStatus.Contains(cur_status))
        {
            var index = (Consts.EditableLetterStatus.IndexOf(cur_status) + 1) % Consts.EditableLetterStatus.Count;
            currentCharacter.status = Consts.EditableLetterStatus[index];
        }
        
    }

    Texture UnLockButton => (Texture)AssetDatabase.LoadAssetAtPath("Packages/com.unity.timeline/Editor/StyleSheets/Images/Icons/TrackLockButtonDisabled.png", typeof(Texture));
    Texture LockButton => (Texture)AssetDatabase.LoadAssetAtPath("Packages/com.unity.timeline/Editor/StyleSheets/Images/Icons/d_TrackLockButtonEnabled.png", typeof(Texture));

    GUIStyle CharacterButtonStyle
    {
        get
        {
            var style = new GUIStyle(GUI.skin.button);
            style.fixedWidth = 30;
            style.stretchWidth = false;

            return style;
        }
    }
    GUIStyle CharacterLabelStyle
    {
        get
        {
            var style = new GUIStyle();
            style.fixedWidth = 20;
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

    bool CheckNeededKey()
    {
        int key_had = 0;
        foreach (var item in levelData.CharacterArray)
        {
            if (item.status == G1_LetterStatus.HasKey)
                key_had++;
        }

        foreach (var item in levelData.word_cryptos_array)
        {
            if(item.isLocked > 0)
            {
                foreach (var letter in item.character_array)
                {
                    if(letter.status == G1_LetterStatus.HasKey)
                    {
                        GUIStyle style = new GUIStyle(GUI.skin.label);
                        style.normal.textColor = Color.red;
                        EditorGUILayout.LabelField($"Warning: There must be no HasKey status in locked word!", style);
                        return false;
                    }
                }
            }

        }

        var locks = levelData.MaxLock;
        var key_needed = locks * 4;        
        if(key_had < key_needed)
        {
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.normal.textColor = Color.yellow;
            EditorGUILayout.LabelField($"Warning: You need to assign {key_needed - key_had} HasKey status left!", style);
            return false;
        }
        else if (key_had > key_needed)
        {
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.normal.textColor = Color.yellow;
            EditorGUILayout.LabelField($"Warning: There're {key_had - key_needed} extra keys in the sentence! Remove it!", style);
            return false;
        }
        return true;
    }


    #region GenerateField


    private void GeneratedLevel(float hard_percent) // 0 - 1
    {
        // Reset
        var character_array = levelData.CharacterArray;
        foreach (var item in levelData.word_cryptos_array)
        {
            item.isLocked = 0;
        }

        List<G1_CharacterCrypto> characters_left = new List<G1_CharacterCrypto>();

        // Generate dictionary
        Dictionary<char, List<G1_CharacterCrypto>> char_crypto_dic = new Dictionary<char, List<G1_CharacterCrypto>>();
        foreach (var item in character_array)
        {
            if (char.IsPunctuation(item.character))
            {
                item.status = G1_LetterStatus.None;
                continue;
            }
            else if (char_crypto_dic.ContainsKey(item.character))
            {
                char_crypto_dic[item.character].Add(item);
                item.status = G1_LetterStatus.Process;
            }
            else
            {
                char_crypto_dic.Add(item.character, new List<G1_CharacterCrypto>() { item });
                item.status = G1_LetterStatus.Process;
            }
        }
        foreach (var item in char_crypto_dic)
        {
            if(item.Value.Count == 1)
            {
                item.Value[0].status = Consts.RandomBool(hardPercent) ? G1_LetterStatus.Completed : G1_LetterStatus.Process;
            }
            else
            {
                var comple_count = Mathf.RoundToInt(item.Value.Count / 1.8f * Mathf.Clamp(1-hardPercent, 0.4f, 1.0f));
                var arrayUsed = Consts.ShuffleList(item.Value);
                var arrayComplete = arrayUsed.GetRange(0, comple_count);
                var arrayProcess = arrayUsed.GetRange( Mathf.Max(0,comple_count - 1), arrayUsed.Count - comple_count);
                foreach (var charac in arrayComplete)
                {
                    charac.status = G1_LetterStatus.Completed;
                }
                foreach (var charac in arrayProcess)
                {
                    charac.status = G1_LetterStatus.Process;
                    characters_left.Add(charac);
                }
            }
        }

        //generated lock
        GeneratedLock(characters_left, hard_percent);

        //generated word lock
        if (hard_percent > 0.8f)
        {
            var word_length = new int[levelData.word_cryptos_array.Length];
            for (int i = 0; i < word_length.Length; i++)
            {
                word_length[i] = levelData.word_cryptos_array[i].character_array.Length;
            }
            var word_length_max = Mathf.Max(word_length) * Mathf.SmoothStep(0.8f, 1.0f, hard_percent);

            int word_lock = 0;
            float max_word_lock = levelData.word_cryptos_array.Length / 5.0f * Mathf.SmoothStep(0.8f, 1.0f, hard_percent);

            foreach (var item in levelData.word_cryptos_array)
            {
                if(item.character_array.Length <= word_length_max && item.character_array.Length > 3 && Consts.RandomBool(0.1f) && word_lock < Mathf.RoundToInt(max_word_lock))
                {
                    item.isLocked = 4;
                    word_lock++;
                    foreach (var letter in item.character_array)
                    {
                        characters_left.Remove(letter);
                    }
                }
                else
                {
                    item.isLocked = 0;
                }
            }

            //generated word key

            int key_count = 0;
            int max_key = word_lock * 4;

            for (int i = 0; i < max_key; i++)
            {
                var index = i * (characters_left.Count / max_key);
                if (index >= characters_left.Count)
                    index--;

                characters_left[index].status = G1_LetterStatus.HasKey;
                key_count++;
                if (key_count == max_key)
                    break;
            }
        }


    }

    private bool CheckWordValid()
    {
        foreach (var word in levelData.word_cryptos_array)
        {
            if (!IsWordValid(word))
            {
                GUIStyle style = new GUIStyle(GUI.skin.label);
                style.normal.textColor = Color.red;
                EditorGUILayout.LabelField($"Warning: Some word are invalid! Please check again", style);
                GUIStyle style_child = new GUIStyle(GUI.skin.label);
                style_child.normal.textColor = Color.yellow;
                EditorGUILayout.LabelField($"1: The number of letters must be greater than the locker in that word.", style_child);
                EditorGUILayout.LabelField($"2: Double lock cannot be the first or last letter.", style_child);
                EditorGUILayout.LabelField($"3: Double lock cannot be next to each other.", style_child);
                EditorGUILayout.LabelField($"4: If the first is locked, the next letter cannot be double lock.", style_child);
                return false;
            }
        }
        return true;
    }

    private bool IsWordValid(G2_WordCrypto word)
    {
        int count = 0;

        List<G1_CharacterCrypto> checking_chars = new List<G1_CharacterCrypto>();
        foreach (var item in word.character_array)
        {
            if (!char.IsPunctuation(item.character))
                checking_chars.Add(item);
        }

        for (int i = 0; i < checking_chars.Count; i++)
        {
            var item = checking_chars[i];
            if (item.status == G1_LetterStatus.Lock)
            {
                count++;
                if ((i == 0 || ( i - 1 > 0 && checking_chars[i - 1].status == G1_LetterStatus.DoubleLock )) && i + 1 < checking_chars.Count && checking_chars[i + 1].status == G1_LetterStatus.DoubleLock)
                    return false;

                if (i == checking_chars.Count - 1 && i - 1 > 0 && checking_chars[i - 1].status == G1_LetterStatus.DoubleLock)
                    return false;
            }
            if (item.status == G1_LetterStatus.DoubleLock)
            {
                count = count + 2;
                if (i + 1 < checking_chars.Count && checking_chars[i + 1].status == G1_LetterStatus.DoubleLock)
                    return false;   
                
            }

            if (i == 0 && item.status == G1_LetterStatus.DoubleLock)
                return false;

            if (i == checking_chars.Count - 1 && item.status == G1_LetterStatus.DoubleLock)
                return false;

        }
        if (count >= checking_chars.Count)
            return false;

        return true;
    }

    private void GeneratedLock(List<G1_CharacterCrypto> characters_left, float hard_percent)
    {
        var character_array = levelData.CharacterArray;
        var word_array = levelData.word_cryptos_array;

        int lock_count = 0;
        int dblock_count = 0;
        float max_lock = character_array.Length / 5.0f * Mathf.SmoothStep(0.3f, 1.0f, hard_percent);
        float max_dblock = character_array.Length / 5.0f * Mathf.SmoothStep(0.6f, 1.0f, hard_percent);

        List<G1_CharacterCrypto> chars_abt_remove = new List<G1_CharacterCrypto>();

        foreach (var word in word_array)
        {
            chars_abt_remove.Clear();
            int time_out = 0;
            while (true)
            {
                foreach (var item in word.character_array)
                {
                    if (item.status == G1_LetterStatus.Completed || char.IsPunctuation(item.character))
                        continue;

                    if (hard_percent > 0.3f)
                    {
                        if (Consts.RandomBool(0.1f) && lock_count < Mathf.RoundToInt(max_lock))
                        {
                            characters_left.Remove(item);
                            item.status = G1_LetterStatus.Lock;
                            lock_count++;
                        }
                        else
                            item.status = G1_LetterStatus.Process;
                    }
                    if (hard_percent > 0.6f && item.status != G1_LetterStatus.Lock)
                    {
                        if (Consts.RandomBool(0.1f) && dblock_count < Mathf.RoundToInt(max_dblock))
                        {
                            characters_left.Remove(item);
                            item.status = G1_LetterStatus.DoubleLock;
                            dblock_count++;
                        }
                        else
                            item.status = G1_LetterStatus.Process;
                    }
                }
                if (IsWordValid(word))
                    break;

                time_out++;
                if (time_out > 60)
                {
                    Debug.LogError("Time out!");
                    break;
                }
            }
            
        }
    }

    #endregion

}