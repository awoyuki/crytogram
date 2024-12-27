using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using static Events;


public class G1_LevelManager : BaseLevelManager
{

    public override void Revive()
    {
        GameManager.Instance.ChangeGameState(GameState.InProgress);
        G1_UIGameplay.Instance.DataContainer.LevelMistake--;
        G1_UIGameplay.Instance.uiMistakes.UpdateMistaken(G1_UIGameplay.Instance.DataContainer.LevelMistake);
    }


    protected override int OnStartLevel(int index)
    {
        var data = DataController.instance.G1_SO_DataList.dataLevelList[index];
        bool needReset = !DoesLevelStillOnProcess(data.so_index);
        if (needReset) ResetLevel();

        var uiGameplay = G1_UIGameplay.Instance;
        uiGameplay.DataContainer.LevelMistake = uiGameplay.DataContainer.LevelMistake >= uiGameplay.MaxMistaken ? 0 : uiGameplay.DataContainer.LevelMistake;
        uiGameplay.uiMistakes.UpdateMistaken(uiGameplay.DataContainer.LevelMistake);
        uiGameplay.uiSentence.RemoveSentence();
        uiGameplay.uiSentence.LoadSentenceFromData(data);

        return data.so_index;
    }


    public override void ResetLevel()
    {
        var dataContainer = G1_UIGameplay.Instance.DataContainer;
        dataContainer.LevelWordsStatus = "";
        dataContainer.LevelProcess = "";
        dataContainer.LevelMistake = 0;
    }

    public override bool DoesLevelStillOnProcess(int level_id)
    {
        if (level_id == DataController.instance.CurrentLevelID)
        {
            var process = G1_UIGameplay.Instance.DataContainer.LevelProcess;
            if (string.IsNullOrEmpty(process))
                return false;
            return true;
        }
        return false;
    }

}
