using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using static G3_PencilKeyPrefab;
using static PlayerPrefAttribute;
using System;
using static UnityEngine.GraphicsBuffer;
using UnityEditor;
public class G3_KeyPrefab : MonoBehaviour
{
    public ButtonEffectLogic btn;
    [SerializeField] Image sprite;
    [SerializeField] Color initColor;
    public Text txt_Number;
    public Text txt_Quantity;
    public int quantity;
    public G3_KeyStatus status;
    private void Awake()
    {
        btn.onClick.AddListener(OnButtonClick);
        initColor=sprite.color;
    }
    
    public void OnButtonClick()
    {
        var listRelated = G3_UIGamePlay.Instance.currentCell.GetRelatedCells();
        int id = Array.IndexOf(G3_UIGamePlay.Instance.allUINumberList, G3_UIGamePlay.Instance.currentCell.mainUINumber);
        if (G3_UIGamePlay.Instance.currentCell.cell_status!=G3_CellPrefab.G3_CellStatus.Existed)
        {
            switch (status)
            {
                case G3_KeyStatus.Fill:
                    List<G3_CellPrefab> matchingCells = new List<G3_CellPrefab>();
                    foreach (G3_CellPrefab cell in listRelated)
                    {
                        if (cell.mainUINumber.numberText.text == txt_Number.text)
                        {
                            matchingCells.Add(cell);
                        }
                    }

                    if (matchingCells.Count > 0)
                    {
                        foreach (G3_CellPrefab m_cell in matchingCells)
                        {
                            m_cell.mainUINumber.numberText.transform.DOScale(1.5f, 0.5f).OnComplete(() =>
                            {
                                m_cell.mainUINumber.numberText.transform.DOScale(1f, 0.5f);
                            });
                        }
                    }
                    else
                    {
                        var cur_Text = G3_UIGamePlay.Instance.currentCell.mainUINumber.numberText.text;
                        if (cur_Text != "")
                        {
                            foreach (G3_KeyPrefab key in G3_UIGamePlay.Instance.keyPrefabs)
                            {
                                if (key.txt_Number.text == cur_Text)
                                {
                                    key.ChangeStatus(G3_KeyStatus.Fill);
                                    key.CountSpecificNumber();
                                    key.txt_Quantity.text = key.quantity.ToString();
                                }
                            }
                        }
                        foreach (G3_CellPrefab cell in listRelated)
                        {
                            foreach (G3_UINumber num in cell.pencilUINumbers)
                            {
                                if (/*num.numberText.gameObject.activeInHierarchy &&*/ num.numberText.text == txt_Number.text)
                                {
                                    num.numberText.text = "";
                                }
                            }
                        }
                        foreach (G3_PencilKeyPrefab key in G3_UIGamePlay.Instance.pencilKeyPrefabs)
                        {
                            key.ChangeStatus(G3_PencilKeyStatus.Fill);
                        }
                        G3_UIGamePlay.Instance.MakeMove(id, txt_Number.text);
                        G3_UIGamePlay.Instance.currentCell.Fill(txt_Number.text);
                        CountSpecificNumber();
                        txt_Quantity.text = quantity.ToString();
                        ChangeStatus(G3_KeyStatus.Delete);
                    }
                    break;
                case G3_KeyStatus.Delete:
                   
                    G3_UIGamePlay.Instance.MakeMove(id, "");
                    G3_UIGamePlay.Instance.currentCell.Empty();
                    CountSpecificNumber();
                    txt_Quantity.text = quantity.ToString();
                    ChangeStatus(G3_KeyStatus.Fill);
                    break;
            }
        }
        G3_UIGamePlay.Instance.UpdateData();
        
    }
    public void CountSpecificNumber()
    {
        int count = 0;
        foreach(G3_CellPrefab cell in G3_UIGamePlay.Instance.generator.createdObjects)
        {
            if(cell.mainUINumber.numberText.text == txt_Number.text)
            {
                count++;
            }
        }
        quantity = count;
    }


    public void ChangeStatus(G3_KeyStatus newStatus)
    {
        status = newStatus;
        switch(newStatus)
        {
            case G3_KeyStatus.Fill:
                sprite.color = initColor;
                break;
            case G3_KeyStatus.Delete:
                sprite.color = Color.cyan;
                break;
        }
    }
    public enum G3_KeyStatus
    {
        Fill, Delete
    }
}
