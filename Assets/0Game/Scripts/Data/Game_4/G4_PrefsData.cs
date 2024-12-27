using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class G4_PrefsData 
{
    #region Game

    public static int CurrentHint
    {
        get => PlayerPrefs.GetInt("G4_cur_hint", 0);
        set => PlayerPrefs.SetInt("G4_cur_hint", value);
    }

    public static string LevelProcess
    {
        get => PlayerPrefs.GetString("G4_level_proc", "");
        set => PlayerPrefs.SetString("G4_level_proc", value);
    }

    public static string LevelWordProcess
    {
        get => PlayerPrefs.GetString("G4_level_word_proc", "");
        set => PlayerPrefs.SetString("G4_level_word_proc", value);
    }
    public static string LevelWordBonusProcess
    {
        get => PlayerPrefs.GetString("G4_level_word_bonus_proc", "");
        set => PlayerPrefs.SetString("G4_level_word_bonus_proc", value);
    }

    #endregion
}

public class G4_DataContainer
{
    private int current_hint;
    private string level_process;
    private string level_word_process;
    private string level_word_bonus_process;

    public G4_DataContainer()
    {
        current_hint = G4_PrefsData.CurrentHint;
        level_process = G4_PrefsData.LevelProcess;
        level_word_process = G4_PrefsData.LevelWordProcess;
        level_word_bonus_process = G4_PrefsData.LevelWordBonusProcess;
    }

    public int CurrentHint
    {
        get => current_hint;
        set
        {
            current_hint = value;
            G4_PrefsData.CurrentHint = value;
        }
    }

    public string LevelProcess
    {
        get => level_process;
        set
        {
            level_process = value;
            G4_PrefsData.LevelProcess = value;
        }
    }

    public string LevelWordProcess
    {
        get => level_word_process;
        set
        {
            level_word_process = value;
            G4_PrefsData.LevelWordProcess = value;
        }
    }
    public string LevelWordBonusProcess
    {
        get => level_word_bonus_process;
        set
        {
            level_word_bonus_process = value;
            G4_PrefsData.LevelWordBonusProcess = value;
        }
    }
}