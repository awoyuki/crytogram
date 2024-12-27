using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "G3_SO_DataListLevel", menuName = "ScriptableObject/DataList/G3_SO_DataListLevel")]
public class G3_SO_DataListLevel : ScriptableObject
{
    public List<G3_SO_DataInfoLevel> dataLevelList;
}