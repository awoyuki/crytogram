using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public static class G2_PrefsData
{
    #region Game

    public static int CurrentHint
    {
        get => PlayerPrefs.GetInt("G2_cur_hint",0);
        set => PlayerPrefs.SetInt("G2_cur_hint", value);
    }

    public static string LevelProcess
    {
        get => PlayerPrefs.GetString("G2_level_proc", "");
        set => PlayerPrefs.SetString("G2_level_proc", value);
    }

    public static string LevelTextProces
    {
        get => PlayerPrefs.GetString("G2_level_text_proc_", "");
        set => PlayerPrefs.SetString("G2_level_text_proc_", value);
    }

    #endregion
}


public class G2_DataContainer
{
    private int current_hint;
    private string level_process;
    private string level_text_process;

    public G2_DataContainer()
    {
        current_hint = G2_PrefsData.CurrentHint;
        level_process = G2_PrefsData.LevelProcess;
        level_text_process = G2_PrefsData.LevelTextProces;
    }

    public int CurrentHint
    {
        get => current_hint;
        set
        {
            current_hint = value;
            G2_PrefsData.CurrentHint = value;
        }
    }

    public string LevelProcess
    {
        get => level_process;
        set
        {
            level_process = value;
            G2_PrefsData.LevelProcess = value;
        }
    }

    public string LevelTextProces
    {
        get => level_text_process;
        set
        {
            level_text_process = value;
            G2_PrefsData.LevelTextProces = value;
        }
    }
}
