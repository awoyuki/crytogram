using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "G1_SO_DataListLevel", menuName = "ScriptableObject/DataList/G1_SO_DataListLevel")]
public class G1_SO_DataListLevel : ScriptableObject
{
    public List<G1_SO_DataInfoLevel> dataLevelList;
}