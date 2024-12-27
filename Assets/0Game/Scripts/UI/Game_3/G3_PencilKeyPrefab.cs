using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static G3_KeyPrefab;
public class G3_PencilKeyPrefab : MonoBehaviour
{
    public ButtonEffectLogic btn;
    [SerializeField] Image sprite;
    public Text txt_Number;
    public int number;
    public G3_PencilKeyStatus status;
    [SerializeField] Color initColor;
    private void Awake()
    {
        btn.onClick.AddListener(OnButtonClick);
        initColor = sprite.color;
    }

    public void OnButtonClick()
    {
        var currentCell = G3_UIGamePlay.Instance.currentCell;
        int id = Array.IndexOf(G3_UIGamePlay.Instance.allUINumberList, currentCell.pencilUINumbers[number - 1]);
        switch (status)
        {
            case G3_PencilKeyStatus.Fill:
                
                if(currentCell != null && currentCell.mainUINumber.numberText.text == "")
                {
                    G3_UIGamePlay.Instance.MakeMove(id, txt_Number.text);
                    currentCell.pencilUINumbers[number - 1].numberText.text = txt_Number.text;
                    
                    ChangeStatus(G3_PencilKeyStatus.Delete);
                }
                break;
            case G3_PencilKeyStatus.Delete:
                G3_UIGamePlay.Instance.MakeMove(id, "");
                currentCell.pencilUINumbers[number - 1].numberText.text = "";
               
                ChangeStatus(G3_PencilKeyStatus.Fill);
                break;
        }
        G3_UIGamePlay.Instance.UpdateData();
    }
    public void ChangeStatus(G3_PencilKeyStatus newStatus)
    {
        status = newStatus;
        switch (newStatus)
        {
            case G3_PencilKeyStatus.Fill:
                sprite.color = initColor;
                break;
            case G3_PencilKeyStatus.Delete:
                sprite.color = Color.cyan;
                break;
        }
    }
    public enum G3_PencilKeyStatus
    {
        Fill, Delete
    }
}
