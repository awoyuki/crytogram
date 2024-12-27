using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using static Events;

public class DataController : MonoBehaviour
{
    public static DataController instance;

    [SerializeField] public G1_SO_DataListLevel G1_SO_DataList;
    [SerializeField] public G2_SO_DataListLevel G2_SO_DataList;
    [SerializeField] public G3_SO_DataListLevel G3_SO_DataList;
    [SerializeField] public G4_SO_DataListLevel G4_SO_DataList;
    [SerializeField] public G7_SO_DataListLevel G7_SO_DataList;
    [SerializeField] public SO_DataListGameType so_ListGameType;

    private int current_level_count = 0;
    public int CurrentLevelCount
    {
        get => current_level_count;
        set
        {
            current_level_count = value;
            PrefsData.SetCurrentLevelCount(GameManager.Instance.GameMode, value);
        }
    }

    private int current_level_ID = -1;
    public int CurrentLevelID
    {
        get => current_level_ID;
        set
        {
            current_level_ID = value;
            PrefsData.SetCurrentLevelID(GameManager.Instance.GameMode, value);
        }
    }



    private void Awake()
    {
        instance = this;
    }


    public void LoadData(GameMode gameMode)
    {
        if (gameMode == GameMode.None)
            return;

        current_level_count = PrefsData.GetCurrentLevelCount(gameMode);
        current_level_ID = PrefsData.GetCurrentLevelID(gameMode);
    }

    public int TotalLevel
    {
        get
        {
            var gameMode = GameManager.Instance.GameMode;
            return TotalLevelByGameMode(gameMode);
        }
    }

    public int TotalLevelByGameMode(GameMode gameMode)
    {
        switch (gameMode)
        {
            case GameMode.None:
                return 0;
            case GameMode.Cryptogram1:
                return G1_SO_DataList.dataLevelList.Count;
            case GameMode.Cryptogram2:
                return G2_SO_DataList.dataLevelList.Count;
            case GameMode.Sudoku:
                return G3_SO_DataList.dataLevelList.Count;
            case GameMode.Wordscapes:
                return G4_SO_DataList.dataLevelList.Count;
            case GameMode.PictureCross:
                return G7_SO_DataList.dataLevelList.Count;
            default:
                return 0;
        }
    }

}
