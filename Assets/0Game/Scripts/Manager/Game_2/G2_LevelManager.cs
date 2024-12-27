using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Events;


public class G2_LevelManager : BaseLevelManager
{


    protected override int OnStartLevel(int index)
    {
        var data = DataController.instance.G2_SO_DataList.dataLevelList[index];
        G2_UIGameplay.Instance.uiSentence.RemoveSentence();
        G2_UIGameplay.Instance.uiSentence.LoadSentenceFromData(data);
        return data.so_index;
    }

    public override void ResetLevel()
    {
        G2_UIGameplay.Instance.DataContainer.LevelProcess = "";
    }


    public override bool DoesLevelStillOnProcess(int level_id)
    {
        if (level_id == DataController.instance.CurrentLevelID)
        {
            var process = G2_UIGameplay.Instance.DataContainer.LevelProcess;
            if (string.IsNullOrEmpty(process))
                return false;
            return true;
        }
        return false;
    }
}


