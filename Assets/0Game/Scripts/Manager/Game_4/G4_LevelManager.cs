using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G4_LevelManager : BaseLevelManager
{    

    public override bool DoesLevelStillOnProcess(int level_id)
    {
        if (level_id == DataController.instance.CurrentLevelID)
        {
            var process = G4_UIGameplay.Instance.DataContainer.LevelProcess;
            if (string.IsNullOrEmpty(process))
                return false;
            return true;
        }
        return false;
    }

    protected override int OnStartLevel(int index)
    {
        var data = DataController.instance.G4_SO_DataList.dataLevelList[index];
        bool needReset = !DoesLevelStillOnProcess(data.so_index);
        if (needReset) ResetLevel();
        G4_UIGameplay.Instance.GenerateLevel(data);
        return data.so_index;
    }

    public override void ResetLevel()
    {
        base.ResetLevel();
        G4_UIGameplay.Instance.DataContainer.LevelProcess = "";
        G4_UIGameplay.Instance.DataContainer.LevelWordProcess = "";
        G4_UIGameplay.Instance.DataContainer.LevelWordBonusProcess = "";
    }

}
