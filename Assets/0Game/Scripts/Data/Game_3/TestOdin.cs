using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = " Test", menuName = "ScriptableObject/Test")]
public class TestOdin : SerializedScriptableObject
{
    [TableMatrix(HorizontalTitle = "Read Only Matrix")]
    public int[,] ReadOnlyMatrix = new int[5, 5];

    [TableMatrix(HorizontalTitle = "X axis", VerticalTitle = "Y axis")]
    public InfoMessageType[,] LabledMatrix = new InfoMessageType[6, 6];

    [TableMatrix(HorizontalTitle = "Square Celled Matrix", SquareCells = true)]
    public Texture2D[,] SquareCelledMatrix;

    [TableMatrix(SquareCells = true)]
    public Mesh[,] PrefabMatrix;

}
