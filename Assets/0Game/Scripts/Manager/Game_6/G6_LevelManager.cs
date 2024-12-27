using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G6_LevelManager : BaseLevelManager
{
    public override bool DoesLevelStillOnProcess(int level_id)
    {
        return false;
    }

    protected override int OnStartLevel(int index)
    {
        return 0;
    }


}
