using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G7_LevelManager : BaseLevelManager
{


    protected override int OnStartLevel(int index)
    {
        var data = DataController.instance.G7_SO_DataList.dataLevelList[index];
        bool needReset = !DoesLevelStillOnProcess(data.level_index);
        if (needReset) ResetLevel();
        G7_UIGamePlay.Instance.LoadCurrentLevel(index);
        return data.level_index;
    }

    public override bool DoesLevelStillOnProcess(int level_id)
    {
        /*if (level_id == DataController.instance.CurrentLevelID)
        {
            var process = G7_UIGamePlay.Instance.DataContainer.LevelProcess;
            if (string.IsNullOrEmpty(process))
                return false;
            return true;
        }*/
        return false;
    }
    
}

public enum G7_SquareState{
    None, Fill, Cross, 
}
