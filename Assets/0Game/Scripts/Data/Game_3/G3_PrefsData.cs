using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public static class G3_PrefsData
{
    #region Game

    public static string LevelProcess
    {
        get => PlayerPrefs.GetString("G3_level_proc", "");
        set => PlayerPrefs.SetString("G3_level_proc", value);
    }

    public static string LevelStatusProces
    {
        get => PlayerPrefs.GetString("G3_level_status_proc_", "");
        set => PlayerPrefs.SetString("G3_level_status_proc_", value);
    }

    #endregion
}


public class G3_DataContainer
{
    private string level_process;
    private string level_status_proces;
    public G3_DataContainer()
    {
        level_process = G3_PrefsData.LevelProcess;
        level_status_proces = G3_PrefsData.LevelStatusProces;
    }

    public string LevelProcess
    {
        get => level_process;
        set
        {
            level_process = value;
            G3_PrefsData.LevelProcess = value;
        }
    }
    public string LevelStatusProcess
    {
        get => level_status_proces;
        set
        {
            level_status_proces = value;
            G3_PrefsData.LevelStatusProces = value;
        }
    }
}
