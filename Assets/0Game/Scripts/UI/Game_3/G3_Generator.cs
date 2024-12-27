using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using System;
using Unity.Mathematics;
using Sirenix.OdinInspector;
public class G3_Generator : MonoBehaviour
{
    public G3_CellPrefab cellPrefab;
    public Transform gridParent;
    public int[,] grid;
    public List<G3_CellPrefab> createdObjects;

    public G3_SO_DataInfoLevel data;

    public void LoadGrid(G3_SO_DataInfoLevel data)
    {
        grid = new int[data.matrix.GetLength(0), data.matrix.GetLength(1)];
        for (int i = 0; i < data.matrix.GetLength(0); i++)
        {
            for (int j = 0; j < data.matrix.GetLength(1); j++)
            {
                grid[i, j] = data.matrix[i, j];
            }
        }
    }
    public void Init(G3_SO_DataInfoLevel data)
    {
        this.data = data;
        LoadGrid(data);
        DrawGrid();
        SetUpGrid();
    }
    public void CountEachKeyPrefab(G3_SO_DataInfoLevel data)
    {
        foreach (var keyPrefab in G3_UIGamePlay.Instance.keyPrefabs)
        {
            int count = 0;
            foreach (var cell in createdObjects)
            {
                if (cell.mainUINumber.numberText.text == keyPrefab.txt_Number.text)
                {
                    count++;
                }
            }
            keyPrefab.quantity = count;
            keyPrefab.txt_Quantity.text = count.ToString();
            Debug.Log(keyPrefab.txt_Number.text + ": " + keyPrefab.quantity);
        }
        gridParent.GetComponent<GridLayoutGroup>().constraintCount = data.matrix.GetLength(0);
        int cellSize = 1260 / data.matrix.GetLength(0);
        gridParent.GetComponent<GridLayoutGroup>().cellSize = new Vector2(cellSize, cellSize);
    }
    //[Button]
    public void DestroyAllCreatedObjects()
    {
        foreach (G3_CellPrefab obj in createdObjects)
        {
            DestroyImmediate(obj.gameObject);
        }
        createdObjects.Clear();
    }
    public string ConvertNumberToChar(int number)
    {
        if (number >= 10 && number <= 16)
        {
            return ((char)('A' + (number - 10))).ToString();
        }
        else
        {
            return number.ToString(); 
        }
    }
    public void SetUpGrid()
    {
        for (int col = 0; col < grid.GetLength(0); col++)
        {
            for (int row = 0; row < grid.GetLength(1); row++)
            {
                G3_CellPrefab cell = createdObjects[row+9*col];
                Text cellText = cell.mainUINumber.numberText;
                if (grid[col, row] != 0)
                {
                    Debug.Log(grid[col, row]);
                    cellText.text = ConvertNumberToChar(grid[col, row]);//grid[col, row].ToString();
                    cell.ChangeStatus(G3_CellPrefab.G3_CellStatus.Existed);
                }
                else
                {
                    cell.ChangeStatus(G3_CellPrefab.G3_CellStatus.Normal);
                    cellText.text = "";
                }
            }
        }
        foreach (G3_KeyPrefab key in G3_UIGamePlay.Instance.keyPrefabs)
        {
            if (G3_UIGamePlay.Instance.keyPrefabs.IndexOf(key) > grid.GetLength(0) - 1)
            {
                key.gameObject.SetActive(false);
            }
            else
            {
                key.gameObject.SetActive(true);
            }
        }
        foreach (G3_PencilKeyPrefab p_key in G3_UIGamePlay.Instance.pencilKeyPrefabs)
        {
            if (G3_UIGamePlay.Instance.pencilKeyPrefabs.IndexOf(p_key) > grid.GetLength(0) - 1)
            {
                p_key.gameObject.SetActive(false);
            }
            else
            {
                p_key.gameObject.SetActive(true);
            }
        }
        foreach (G3_CellPrefab cell in createdObjects)
        {
            foreach (G3_UINumber p_num in cell.pencilUINumbers)
            {
                if (cell.pencilUINumbers.IndexOf(p_num) > grid.GetLength(0) - 1)
                {
                    p_num.gameObject.SetActive(false);
                }
                else
                {
                    p_num.gameObject.SetActive(true);
                }
            }
        }
    }
    void DrawGrid()
    {

        for (int col = 0; col < grid.GetLength(0); col++)
        {
            for (int row = 0; row < grid.GetLength(1); row++)
            {
                G3_CellPrefab cell = Instantiate(cellPrefab, gridParent);
                createdObjects.Add(cell);
                cell.pos = new Vector2(col,row);
                //Text cellText = cell.mainUINumber.numberText;
                //if (grid[col, row] != 0)
                //{
                //    cellText.text = ConvertNumberToChar(grid[col, row]);//grid[col, row].ToString();
                //    cell.isExisted = true;
                //}
                //else
                //{
                //    cell.isExisted = false;
                //    cellText.text = "";
                //}
            }
        }
        G3_UIGamePlay.Instance.allUINumberList = gridParent.GetComponentsInChildren<G3_UINumber>(true);
    }

    //static bool SolveGrid(int[,] grid)
    //{

    //    for (int i = 0; i < 81; i++)
    //    {
    //        int col = i / 9;
    //        int row = i % 9;
    //        if (grid[col, row] == 0)
    //        {
    //            for (int num = 1; num <= 9; num++)
    //            {
    //                if (IsValidMove(col, row, num, grid))
    //                {
    //                    grid[col, row] = num;

    //                    if (CheckGrid(grid))
    //                    {
    //                        counter += 1;
    //                        break;
    //                    }

    //                    else
    //                    {
    //                        if (SolveGrid(grid))
    //                        {
    //                            return true;
    //                        }
    //                    }
    //                }
    //            }
    //            break;
    //        }
    //        //grid[col,row] = 0;
    //    }
    //    return false;
    //}

    //private static bool IsValidMove(int col, int row, int num, int[,] grid )
    //{
    //    for (int i = 0; i < 9; i++)
    //    {
    //        if (grid[col, i] == num || grid[i, row] == num)
    //            return false;
    //    }

    //    int startRow = (col / 3) * 3;
    //    int startCol = (row / 3) * 3;

    //    for (int i = startRow; i < startRow + 3; i++)
    //    {
    //        for (int j = startCol; j < startCol + 3; j++)
    //        {
    //            if (grid[i, j] == num)
    //                return false;
    //        }
    //    }

    //    return true;
    //}

    //void GenerateGrid()
    //{
    //    List<int> numberList = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    //    Shuffle(numberList);
    //    for (int col = 0; col < 9; col++)
    //    {
    //        for (int row = 0; row < 9; row++)
    //        {
    //            grid[col, row] = 0;
    //        }
    //    }

    //    FillGrid(0, 0, grid);
    //    RemoveNumbers();
    //}
    //public void RemoveNumbers()
    //{
    //    counter = 1;
    //    while (attempts > 0)
    //    {
    //        int col = Random.Range(0, 9);
    //        int row = Random.Range(0, 9);
    //        while (grid[col, row] == 0)
    //        {
    //            col = Random.Range(0, 9);
    //            row = Random.Range(0, 9);
    //        }
    //        int backup = grid[col, row];
    //        grid[col, row] = 0;
    //        int[,] copyGrid = new int[9, 9];
    //        for (int r = 0; r < 9; r++)
    //        {
    //            for (int c = 0; c < 9; c++)
    //            {
    //                copyGrid[r, c] = grid[r, c];
    //            }
    //        }
    //        counter = 0;
    //        SolveGrid(copyGrid);
    //        if (counter != 1)
    //        {
    //            grid[col, row] = backup;
    //            attempts--;
    //        }
    //    }
    //}
    //static bool CheckGrid(int[,] grid)
    //{
    //    for (int col = 0; col < 9; col++)
    //    {
    //        for (int row = 0; row < 9; row++)
    //        {
    //            if (grid[col, row] == 0)
    //                return false;
    //        }
    //    }
    //    return true;
    //}
    //private static bool FillGrid(int r, int c, int[,] grid)
    //{
    //    if (r == 9)
    //    {
    //        return true;
    //    }
    //    if (c == 9)
    //    {
    //        return FillGrid(r + 1, 0, grid);
    //    }

    //    List<int> numbers = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    //    Shuffle(numbers);
    //    foreach (var num in numbers)
    //    {
    //        if (IsValidMove(r,c,num, grid))
    //        {
    //            grid[r, c] = num;

    //            if (FillGrid(r, c + 1, grid))
    //            {
    //                return true;
    //            }
    //        }
    //    }

    //    grid[r, c] = 0;
    //    return false;
    //}



    //private static void Shuffle<T>(List<T> list)
    //{
    //    int n = list.Count;
    //    while (n > 1)
    //    {
    //        n--;
    //        int k = Random.Range(0, n);
    //        T temp = list[k];
    //        list[k] = list[n];
    //        list[n] = temp;
    //    }
    //}
}
