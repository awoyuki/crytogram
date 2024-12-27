using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "G2_SO_DataListLevel", menuName = "ScriptableObject/DataList/G2_SO_DataListLevel")]
public class G2_SO_DataListLevel : ScriptableObject
{
    public List<G2_SO_DataInfoLevel> dataLevelList;
}