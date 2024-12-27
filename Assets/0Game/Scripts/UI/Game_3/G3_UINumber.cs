using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class G3_UINumber : MonoBehaviour
{
    [SerializeField] Image bg;
    public Text numberText;
    public G3_CellPrefab parentCell;
    private void Start()
    {
        parentCell = GetComponentInParent<G3_CellPrefab>();
    }
  
    public void ChangeBGColor(Color color)
    {
        bg.color = color;
    }
    public void ChangeNumberText(string text)
    {
        numberText.text = text;
        numberText.fontStyle = FontStyle.BoldAndItalic;
        numberText.color = Color.blue;
    }
}
