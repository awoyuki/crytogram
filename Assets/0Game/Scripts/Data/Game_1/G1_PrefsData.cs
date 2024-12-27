using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public static class G1_PrefsData
{
    #region Game


    public static int CurrentHint
    {
        get => PlayerPrefs.GetInt("G1_cur_hint", 0);
        set => PlayerPrefs.SetInt("G1_cur_hint", value);
    }

    public static int CurrentCollection
    {
        get => PlayerPrefs.GetInt("G1_cur_collection", 0);
        set => PlayerPrefs.SetInt("G1_cur_collection", value);
    }

    public static string LevelProcess
    {
        get => PlayerPrefs.GetString("G1_level_proc", "");
        set => PlayerPrefs.SetString("G1_level_proc", value);
    }

    public static string LevelWordsStatus
    {
        get => PlayerPrefs.GetString("G1_level_words_stat", "");
        set => PlayerPrefs.SetString("G1_level_words_stat", value);
    }

    public static int LevelMistake
    {
        get => PlayerPrefs.GetInt("G1_level_mis", 0);
        set => PlayerPrefs.SetInt("G1_level_mis", value);
    }

    #endregion

}

public class G1_DataContainer
{
    private int current_hint;
    private int current_collection;
    private string level_process;
    private string level_word_status;
    private int level_mistake;

    public G1_DataContainer()
    {
        current_hint = G1_PrefsData.CurrentHint;
        current_collection = G1_PrefsData.CurrentCollection;
        level_process = G1_PrefsData.LevelProcess;
        level_word_status = G1_PrefsData.LevelWordsStatus;
        level_mistake = G1_PrefsData.LevelMistake;
    }

    public int CurrentHint
    {
        get => current_hint;
        set
        {
            current_hint = value;
            G1_PrefsData.CurrentHint = value; 
        }
    }

    public int CurrentCollection
    {
        get => current_collection;
        set
        {
            current_collection = value;
            G1_PrefsData.CurrentCollection = value;
        }
    }

    public string LevelProcess
    {
        get => level_process;
        set
        {
            level_process = value;
            G1_PrefsData.LevelProcess = value;
        }
    }

    public string LevelWordsStatus
    {
        get => level_word_status;
        set
        {
            level_word_status = value;
            G1_PrefsData.LevelWordsStatus = value;
        }
    }

    public int LevelMistake
    {
        get => level_mistake;
        set
        {
            level_mistake = value;
            G1_PrefsData.LevelMistake = value;
        }
    }
}