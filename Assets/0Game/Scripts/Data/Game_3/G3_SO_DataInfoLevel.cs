using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "G3_SO_DataInfoLevel", menuName = "ScriptableObject/DataInfoLevel/G3_SO_DataInfoLevel")]
public class G3_SO_DataInfoLevel : SerializedScriptableObject
{
    public int so_index;

    public int subGridRows;
    public int subGridCols;

    [TableMatrix(HorizontalTitle = "Custom Matrix", DrawElementMethod = "DrawMatrixElement", RowHeight = 40, SquareCells = true,Transpose =true)]
    public int[,] matrix;

    [ShowInInspector, DoNotDrawAsReference]
    [TableMatrix(HorizontalTitle = "Solve Matrix", DrawElementMethod = "DrawMatrixElement", RowHeight = 40, SquareCells = true, Transpose = true)]
    public int[,] sol_matrix;

    private static int DrawMatrixElement(Rect rect, int value)
    {
        var styleField = new GUIStyle(GUI.skin.textField); 
        styleField.alignment = TextAnchor.MiddleCenter;
        styleField.fontStyle = FontStyle.Bold;
        styleField.fontSize = 25;
        switch (value)
        {
            case 0:
                styleField.normal.textColor = Color.clear;
                break;
            default:
                styleField.normal.textColor = Color.white;
                break;
        }
        value = EditorGUI.IntField(rect, value, styleField);
        return value;
    }
    [ShowInInspector, DoNotDrawAsReference]
    private void SolveSudoku()
    {
        Array.Copy(matrix, sol_matrix, matrix.Length);
        if (!SolveGrid(sol_matrix))
        {
            Debug.LogError("No solution found for the given Sudoku grid.");
        }
    }
    [ShowInInspector, DoNotDrawAsReference]
    private void Clear()
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                matrix[i, j] = 0;
            }
        }
        int sol_rows = sol_matrix.GetLength(0);
        int sol_cols = sol_matrix.GetLength(1);

        for (int i = 0; i < sol_rows; i++)
        {
            for (int j = 0; j < sol_cols; j++)
            {
                sol_matrix[i, j] = 0;
            }
        }
    }
    private bool SolveGrid(int[,] grid)
    {
        int n = grid.GetLength(0);
        int m = grid.GetLength(1);
        (int row, int col) = GetCellWithSmallestDomain(grid, n, m);

        if (row == -1)
            return true;

        for (int num = 1; num <= n; num++)
        {
            if (IsValidMove(row, col, num, grid, subGridRows, subGridCols))
            {
                grid[row, col] = num;
                if (SolveGrid(grid))
                    return true;

                grid[row, col] = 0; 
            }
        }

        return false;
    }

    private (int, int) GetCellWithSmallestDomain(int[,] grid, int n, int m)
    {
        int minDomainSize = int.MaxValue;
        (int, int) bestCell = (-1, -1);

        for (int row = 0; row < n; row++)
        {
            for (int col = 0; col < m; col++)
            {
                if (grid[row, col] == 0)
                {
                    int domainSize = GetDomain(row, col, grid, n, m).Count;
                    if (domainSize < minDomainSize)
                    {
                        minDomainSize = domainSize;
                        bestCell = (row, col);
                    }
                }
            }
        }

        return bestCell;
    }

    private List<int> GetDomain(int row, int col, int[,] grid, int n, int m)
    {
        HashSet<int> domain = new HashSet<int>(Enumerable.Range(1, n));

        for (int i = 0; i < n; i++)
        {
            domain.Remove(grid[row, i]); 
            domain.Remove(grid[i, col]); 
        }

        int subGridStartRow = (row / subGridRows) * subGridRows;
        int subGridStartCol = (col / subGridCols) * subGridCols;

        for (int i = 0; i < subGridRows; i++)
        {
            for (int j = 0; j < subGridCols; j++)
            {
                domain.Remove(grid[subGridStartRow + i, subGridStartCol + j]);
            }
        }

        return domain.ToList();
    }


    private bool IsValidMove(int row, int col, int num, int[,] grid, int subGridRows, int subGridCols)
    {
        int n = grid.GetLength(0);
        int m = grid.GetLength(1);
        for (int c = 0; c < m; c++)
        {
            if (grid[row, c] == num)
            {
                return false;
            }
        }
        for (int r = 0; r < n; r++)
        {
            if (grid[r, col] == num)
            {
                return false;
            }
        }
        int startRow = (row / subGridRows) * subGridRows;
        int startCol = (col / subGridCols) * subGridCols;

        for (int r = startRow; r < startRow + subGridRows; r++)
        {
            for (int c = startCol; c < startCol + subGridCols; c++)
            {
                if (grid[r, c] == num)
                {
                    return false;
                }
            }
        }

        return true;
    }



}
