using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu(fileName = "SO_DataListGameType", menuName = "ScriptableObject/SO_DataListGameType")]
public class SO_DataListGameType : ScriptableObject
{
    public List<SO_DataGameType> data_game_type_list = new List<SO_DataGameType>();
}

[Serializable]
public class SO_DataGameType
{
    public int so_index;
    public string so_name;
    public GameMode gameMode;
    public BaseLevelManager gameTypePrefabs;
    public GameObject panelIcon;
}