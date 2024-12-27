using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PrefsData
{

    public static int GetCurrentLevelCount(GameMode gameMode)
    {
        return PlayerPrefs.GetInt("cur_level_count_" + ((int)gameMode).ToString(), 0);
    }

    public static void SetCurrentLevelCount(GameMode gameMode, int value)
    {
        PlayerPrefs.SetInt("cur_level_count_" + ((int)gameMode).ToString(), value);
    }

    public static int GetCurrentLevelID(GameMode gameMode)
    {
        return PlayerPrefs.GetInt("cur_level_ID_" + ((int)gameMode).ToString(), -1);
    }

    public static void SetCurrentLevelID(GameMode gameMode, int value)
    {
        PlayerPrefs.SetInt("cur_level_ID_" + ((int)gameMode).ToString(), value);
    }
}
