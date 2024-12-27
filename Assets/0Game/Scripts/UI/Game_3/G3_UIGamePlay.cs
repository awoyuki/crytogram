using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor.VisionOS;
using UnityEngine;
using static G3_CellPrefab;

public class G3_UIGamePlay : MonoBehaviour
{
    private static G3_UIGamePlay instance;
    public static G3_UIGamePlay Instance
    {
        get
        {
            if (instance is null)
            {
                instance = FindObjectOfType<G3_UIGamePlay>();
            }
            return instance;
        }
    }
    private G3_DataContainer dataContainer;

    public G3_DataContainer DataContainer
    {
        get
        {
            if (dataContainer is null)
            {
                dataContainer = new G3_DataContainer();
            }
            return dataContainer;
        }
    }
    public G3_Generator generator;
    public G3_CellPrefab currentCell;
    public G3_UINumber[] allUINumberList;
    public List<G3_KeyPrefab> keyPrefabs;
    public List<G3_PencilKeyPrefab> pencilKeyPrefabs;
    public ButtonEffectLogic useHint;
    public ButtonEffectLogic showPencilNumber;
    public ButtonEffectLogic checkError;
    public ButtonEffectLogic undo;
    public ButtonEffectLogic redo;
    private void OnDestroy()
    {
        instance = null;
    }

    private void Start()
    {
        useHint.onClick.AddListener(UseHint);
        showPencilNumber.onClick.AddListener(ShowPencilNumber);
        checkError.onClick.AddListener(CheckError);
        undo.onClick.AddListener(UndoMove);
        redo.onClick.AddListener(RedoMove);
    }

    public void Init(G3_SO_DataInfoLevel data)
    {
        generator.Init(data);
        if (!string.IsNullOrEmpty(DataContainer.LevelProcess))
        {
            var text_array = DataContainer.LevelProcess.ToCharArray();
            for (int i = 0; i < allUINumberList.Length - 1; i++)
            {
                if (text_array[i] != '0')
                {
                    allUINumberList[i].numberText.text = text_array[i].ToString();
                }
                else
                {
                    allUINumberList[i].numberText.text = "";
                }
            }
        }
        if (!string.IsNullOrEmpty(DataContainer.LevelStatusProcess))
        {
            var status_array = DataContainer.LevelStatusProcess.ToCharArray();
            for (int i = 0; i < generator.createdObjects.Count - 1; i++)
            {
                generator.createdObjects[i].ChangeStatus((G3_CellStatus)Char.GetNumericValue(status_array[i]));
            }
        }
        ChangeUITextNum();
        generator.CountEachKeyPrefab(data);

    }
    void ChangeUITextNum()
    {
        foreach (G3_CellPrefab cell in generator.createdObjects)
        {
            if (cell.cell_status!= G3_CellStatus.Existed && cell.mainUINumber.numberText.text != "")
            {
                cell.mainUINumber.ChangeNumberText(cell.mainUINumber.numberText.text);
            }
        }
    }
    public bool CheckWinGame()
    {
        foreach(var cell in generator.createdObjects)
        {
            if (cell.mainUINumber.numberText.text == string.Empty)
            {
                return false;
            }
        }
        return true;
    }
    public string GetProcess()
    {
        StringBuilder result = new StringBuilder();
        foreach (var item in allUINumberList)
        {
            string process_id;
            if (item.numberText.text != "")
            {
                process_id = item.numberText.text;
            }
            else
            {
                process_id = "0";
            }
            result.Append(process_id);
        }
        return result.ToString();
    }
    public string GetStatusProcess()
    {
        StringBuilder result = new StringBuilder();
        foreach (var item in generator.createdObjects)
        {
            int process_id = (int)item.cell_status;
            result.Append(process_id);
        }
        return result.ToString();
    }
    public void WinGame()
    {
        Debug.Log("Win");
    }
    //public void UseHint()
    //{
    //    currentCell.Fill
    //}
    private Stack<(int index, string prevValue, string newValue)> undoStack = new Stack<(int index, string prevValue, string newValue)>();
    private Stack<(int index, string prevValue, string newValue)> redoStack = new Stack<(int index, string prevValue, string newValue)>();

    public void MakeMove(int cellIndex,string newValue)
    {
        string prevValue = allUINumberList[cellIndex].numberText.text;
        undoStack.Push((cellIndex, prevValue, newValue));
        redoStack.Clear();
        allUINumberList[cellIndex].numberText.text = newValue;
    }
    public void UndoMove()
    {
        if (undoStack.Count > 0)
        {
            var (index, prevValue, newValue) = undoStack.Pop();
            redoStack.Push((index, prevValue, newValue));
            allUINumberList[index].numberText.text = prevValue;
            allUINumberList[index].parentCell.OnButtonClick();
            foreach (G3_CellPrefab cell in generator.createdObjects)
            {
                if (cell.cell_status==G3_CellStatus.Wrong)
                {
                    cell.ChangeStatus(G3_CellStatus.Normal);
                }
            }
        }
        UpdateData();
    }
    public void RedoMove()
    {
        if (redoStack.Count > 0)
        {
            var (index, prevValue, newValue) = redoStack.Pop();
            undoStack.Push((index, prevValue, newValue));
            allUINumberList[index].numberText.text = newValue;
            allUINumberList[index].parentCell.OnButtonClick();
        }
        UpdateData();
    }

    public void UpdateData()
    {
        DataContainer.LevelProcess = GetProcess();
        DataContainer.LevelStatusProcess = GetStatusProcess();
    }
    public void RestartGame()
    {
        DataContainer.LevelProcess = string.Empty;
        DataContainer.LevelStatusProcess = string.Empty;
        undoStack.Clear();
        redoStack.Clear();
        generator.DestroyAllCreatedObjects();
        Init(generator.data);
        foreach(G3_KeyPrefab key in keyPrefabs)
        {
            key.ChangeStatus(G3_KeyPrefab.G3_KeyStatus.Fill);
        }
        foreach (G3_PencilKeyPrefab p_key in pencilKeyPrefabs)
        {
            p_key.ChangeStatus(G3_PencilKeyPrefab.G3_PencilKeyStatus.Fill);
        }
        //generator.LoadGrid();
        //generator.SetUpGrid();
        //ChangeUITextNum();
    }
    public void UseHint()
    {
        List<G3_CellPrefab> wrongCells = new List<G3_CellPrefab>();
        foreach (G3_CellPrefab cell in generator.createdObjects)
        {
            if (cell != currentCell)
            {
                if (cell.mainUINumber.numberText.text != "" && cell.mainUINumber.numberText.text != generator.data.sol_matrix[(int)cell.pos.x, (int)cell.pos.y].ToString())
                {
                    wrongCells.Add(cell);
                }
            }
            
        }
        if (wrongCells.Count > 0)
        {
            foreach (G3_CellPrefab cell in wrongCells)
            {
                cell.ChangeStatus(G3_CellStatus.Wrong);
            }
        }
        else
        {
            var cur_level = DataController.instance.CurrentLevelCount;
            var data = DataController.instance.G3_SO_DataList.dataLevelList[cur_level];
            string result = data.sol_matrix[(int)currentCell.pos.x, (int)currentCell.pos.y].ToString();
            foreach(G3_KeyPrefab key in keyPrefabs)
            {
                if(key.txt_Number.text == result)
                {
                    currentCell.Fill(result);
                    currentCell.ChangeStatus(G3_CellStatus.Existed);
                }
                key.CountSpecificNumber();
                key.txt_Quantity.text = key.quantity.ToString();
            }
            //currentCell.isExisted = true;
        }
        currentCell.OnButtonClick();
        UpdateData();
    }
    public void ShowPencilNumber()
    {
        foreach (G3_CellPrefab cell in generator.createdObjects)
        {
            if (string.IsNullOrEmpty(cell.mainUINumber.numberText.text))
            {
                var listRelated = cell.GetRelatedCells(); 
                foreach (G3_UINumber num in cell.pencilUINumbers)
                {
                    bool isPossible = true;
                    string numText = (cell.pencilUINumbers.IndexOf(num) + 1).ToString();

                    foreach (G3_CellPrefab r_cell in listRelated)
                    {
                        if (r_cell.mainUINumber.numberText.text == numText)
                        {
                            isPossible = false; 
                            break;
                        }
                    }
                    num.numberText.text = isPossible ? numText : "";
                }
            }
        }
        UpdateData();
    }
    public void CheckError()
    {
        List<G3_CellPrefab> wrongCells = new List<G3_CellPrefab>();
        foreach (G3_CellPrefab cell in generator.createdObjects)
        {
            if (cell != currentCell)
            {
                if (cell.mainUINumber.numberText.text != "" && cell.mainUINumber.numberText.text != generator.data.sol_matrix[(int)cell.pos.x, (int)cell.pos.y].ToString())
                {
                    wrongCells.Add(cell);
                }
            }

        }
        if (wrongCells.Count > 0)
        {
            foreach (G3_CellPrefab cell in wrongCells)
            {
                cell.ChangeStatus(G3_CellStatus.Wrong);
            }
        }
    }
}
