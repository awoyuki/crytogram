using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G3_CellPrefab : MonoBehaviour
{
    public G3_UINumber mainUINumber;
    public List<G3_UINumber> pencilUINumbers;
    public ButtonEffectLogic btn;
    public G3_CellStatus cell_status;
    [SerializeField] Color selectedColor;
    public Vector2 pos;
    private void Awake()
    {
        btn.onClick.AddListener(OnButtonClick);
    }
   
    public void OnButtonClick()
    {
        G3_UIGamePlay.Instance.currentCell = this;

        foreach(G3_KeyPrefab key in G3_UIGamePlay.Instance.keyPrefabs)
        {

            if (key.txt_Number.text == mainUINumber.numberText.text && mainUINumber.numberText.text != "")
            {
                if(cell_status!=G3_CellStatus.Existed)
                {
                    key.ChangeStatus(G3_KeyPrefab.G3_KeyStatus.Delete);
                }
                
            }
            else
            {
                key.ChangeStatus(G3_KeyPrefab.G3_KeyStatus.Fill);
            }
            
        }
        foreach(G3_PencilKeyPrefab p_key in G3_UIGamePlay.Instance.pencilKeyPrefabs)
        {
            bool isMatched = false;
            foreach (G3_UINumber num in pencilUINumbers)
            {
                if (num.numberText.gameObject.activeInHierarchy && num.numberText.text == p_key.txt_Number.text)
                {
                    p_key.ChangeStatus(G3_PencilKeyPrefab.G3_PencilKeyStatus.Delete);
                    isMatched = true;
                    break;
                }
            }
            if (!isMatched)
            {
                p_key.ChangeStatus(G3_PencilKeyPrefab.G3_PencilKeyStatus.Fill);
            }
        }


        foreach (G3_UINumber num in G3_UIGamePlay.Instance.allUINumberList)
        {
            if (num.parentCell.cell_status!=G3_CellStatus.Wrong)
            {
                if (num.numberText.gameObject.activeInHierarchy && num.numberText.text != "" && num.numberText.text == mainUINumber.numberText.text)
                {
                    Debug.Log("Select");
                    num.ChangeBGColor(selectedColor);//Selected
                }
                else
                {
                    num.ChangeBGColor(Color.clear);//normal
                }
            }
        }
        HighlightRowColumnBlock();
    }
    void HighlightRowColumnBlock()
    {
        int row = (int)pos.x;
        int col = (int)pos.y;

        int subRow = G3_UIGamePlay.Instance.generator.data.subGridRows;
        int subCol = G3_UIGamePlay.Instance.generator.data.subGridCols;
        var listCell = G3_UIGamePlay.Instance.generator.createdObjects;
        foreach (G3_CellPrefab cell in listCell)
        {
            if (cell.pos.x == row || cell.pos.y == col || IsInSameBlock(cell.pos, row, col,subRow,subCol))
            {
                if (cell.cell_status != G3_CellStatus.Wrong)
                {
                    cell.mainUINumber.ChangeBGColor(Color.yellow);//Relation
                }

            }
            else if(cell.mainUINumber.numberText.text != mainUINumber.numberText.text && cell.cell_status != G3_CellStatus.Wrong)
            {
                cell.mainUINumber.ChangeBGColor(Color.clear);
            }
        }
        if (cell_status!=G3_CellStatus.Wrong)
        {
            mainUINumber.ChangeBGColor(selectedColor);
        }
        
    }

    bool IsInSameBlock(Vector2 cellPosition, int row, int col, int subgridRows, int subgridCols)
    {
        int startRow = (row / subgridRows) * subgridRows;
        int startCol = (col / subgridCols) * subgridCols;
        return (cellPosition.x >= startRow && cellPosition.x < startRow + subgridRows) &&
               (cellPosition.y >= startCol && cellPosition.y < startCol + subgridCols);
    }

    public List<G3_CellPrefab> GetRelatedCells()
    {
        int selectedRow = (int)pos.x;
        int selectedCol = (int)pos.y;
        int subRow = G3_UIGamePlay.Instance.generator.data.subGridRows;
        int subCol = G3_UIGamePlay.Instance.generator.data.subGridCols;
        var relatedCells = new List<G3_CellPrefab>();
        var allCells = G3_UIGamePlay.Instance.generator.createdObjects;

        foreach (G3_CellPrefab cell in allCells)
        {
            bool inRowOrColumn = cell.pos.x == selectedRow || cell.pos.y == selectedCol;
            bool inBlock = IsInSameBlock(cell.pos, selectedRow, selectedCol,subRow,subCol);

            if (inRowOrColumn || inBlock)
            {
                if (cell.pos != pos)
                {
                    relatedCells.Add(cell);
                }
            }
        }

        return relatedCells;
    }
    
    public void Fill(string txt)
    {
        
        if (cell_status != G3_CellStatus.Existed)
        {
            ChangeStatus(G3_CellStatus.Normal);
            foreach (G3_UINumber number in pencilUINumbers)
            {
                number.numberText.text=""/*gameObject.SetActive(false)*/;
            }
            mainUINumber.ChangeNumberText(txt);
            foreach (G3_UINumber num in G3_UIGamePlay.Instance.allUINumberList)
            {
                if(/*num.numberText.gameObject.activeInHierarchy &&*/ num.numberText.text == txt)
                {
                    num.ChangeBGColor(selectedColor);
                }
            }
            HighlightRowColumnBlock();
            if (G3_UIGamePlay.Instance.CheckWinGame())
            {
                G3_UIGamePlay.Instance.WinGame();
            }
        }
        
    }
    public void Empty()
    {
        if (cell_status != G3_CellStatus.Existed)
        {
            ChangeStatus(G3_CellStatus.Normal);
            foreach (G3_UINumber num in G3_UIGamePlay.Instance.allUINumberList)
            {
                if (num.numberText.text == mainUINumber.numberText.text)
                {
                    num.ChangeBGColor(Color.clear);
                }
            }
            HighlightRowColumnBlock();

            mainUINumber.numberText.text = "";
        }

    }
    public void ChangeStatus(G3_CellStatus newStatus)
    {
        cell_status = newStatus;
        switch (newStatus)
        {
            case G3_CellStatus.Existed:
                break;
            case G3_CellStatus.Normal:
                mainUINumber.ChangeNumberText(mainUINumber.numberText.text);
                break;
            case G3_CellStatus.Wrong:
                mainUINumber.ChangeNumberText(mainUINumber.numberText.text);
                mainUINumber.ChangeBGColor(Color.red);
                break;
        }
    }
    public enum G3_CellStatus
    {
        None = 0, Existed = 1 ,Normal = 2, Wrong=3
    }
}
