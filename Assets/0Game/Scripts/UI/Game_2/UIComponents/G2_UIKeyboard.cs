using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

[System.Serializable]

public class G2_UIKeyboard : MonoBehaviour
{
    [SerializeField] private char[] alphabetArray;
    public G2_UIKeyPrefab[] uiKeyPrefabs;
    public Transform keyboardParent;
    public G2_UILetter curentUILetter; /*{ get; private set; }*/
    private void Awake()
    {
        alphabetArray = "QWERTYUIOPASDFGHJKL0ZXCVBNM1".ToCharArray();
        Events.onStartLevel += Init;
        G2_UIGameplay.Instance.onSelectLetter += OnSelectLetter;

    }

    private void OnDestroy()
    {
        Events.onStartLevel -= Init;
        G2_UIGameplay.Instance.onSelectLetter -= OnSelectLetter;
    }

    public void Init(int so_index)
    {
        List<G2_UIKeyPrefab> nonInteractableKeys = new List<G2_UIKeyPrefab>();

        foreach (var uiKey in uiKeyPrefabs)
        {
            uiKey.InitKey();
            if(uiKey.GetUILetter().Count <= 0)
                nonInteractableKeys.Add(uiKey);
        }
    }

    public void OnSelectLetter(G2_UILetter letter)
    {
        curentUILetter = letter;
        //if(letter.hintWord!=null) 
        //{
        //    G2_UIController.Instance.uiGameplay.uiSentence.ScrollTo(letter.GetComponent<RectTransform>());
        //}
        //G2_UIController.Instance.uiGameplay.uiSentence.ScrollTo(letter.GetComponent<RectTransform>());
    }
}
