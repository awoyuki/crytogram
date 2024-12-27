using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G3_LevelManager : BaseLevelManager
{
    protected override int OnStartLevel(int index)
    {
        var data = DataController.instance.G3_SO_DataList.dataLevelList[index];
        bool needReset = !DoesLevelStillOnProcess(data.so_index);
        if (needReset) ResetLevel();
        G3_UIGamePlay.Instance.Init(data);
        return data.so_index;
    }


    public override bool DoesLevelStillOnProcess(int level_id)
    {
        if (level_id == DataController.instance.CurrentLevelID)
        {
            var process = G3_UIGamePlay.Instance.DataContainer.LevelProcess;
            if (string.IsNullOrEmpty(process))
                return false;
            return true;
        }
        return false;
    }

    public override void ResetLevel()
    {
        base.ResetLevel();
        G3_UIGamePlay.Instance.RestartGame();
    }

}
