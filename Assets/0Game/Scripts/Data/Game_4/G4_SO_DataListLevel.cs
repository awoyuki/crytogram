using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "G4_SO_DataListLevel", menuName = "ScriptableObject/DataList/G4_SO_DataListLevel")]
public class G4_SO_DataListLevel : ScriptableObject
{
    public List<G4_SO_DataInfoLevel> dataLevelList;
}