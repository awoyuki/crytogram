using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class G1_UIKeyboard : MonoBehaviour
{
    [SerializeField] private char[] alphabetArray;
    [SerializeField] private G1_UIKeyPrefab[] uiKeyPrefabs;
    public Transform keyboardParent;
    public G1_UILetter curentUILetter { get; private set; }
    public G1_UiMoveKeyPrefab moveButtonPrefab;
    private void Awake()
    {
        alphabetArray = "QWERTYUIOPASDFGHJKL0ZXCVBNM1".ToCharArray();
        Events.onStartLevel += Init;
        G1_UIGameplay.Instance.onSelectLetter += OnSelectLetter;
    }

    private void OnDestroy()
    {
        Events.onStartLevel -= Init;
    }

    public void Init(int so_index)
    {
        foreach (var uiKey in uiKeyPrefabs)
        {
            uiKey.InEffective();
        }
        List<G1_UIKeyPrefab> nonInteractableKeys = new List<G1_UIKeyPrefab>();

        foreach (var uiKey in uiKeyPrefabs)
        {
            uiKey.InitKey();
            uiKey.CheckEffective();
            if(uiKey.GetUILetter().Count <= 0)
                nonInteractableKeys.Add(uiKey);
        }


        int numberToDeactivate = (nonInteractableKeys.Count) / 4;
        for (int i = 0; i < nonInteractableKeys.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, nonInteractableKeys.Count);
            var temp = nonInteractableKeys[i];
            nonInteractableKeys[i] = nonInteractableKeys[randomIndex];
            nonInteractableKeys[randomIndex] = temp;
        }

        for (int i = 0; i < numberToDeactivate; i++)
        {
            nonInteractableKeys[i].DeEffective();
        }
    }

    public void OnSelectLetter(G1_UILetter letter)
    {
        curentUILetter = letter;
    }

    public void ReloadAllButton()
    {
        foreach (var item in uiKeyPrefabs)
        {
            item.CheckEffective();
        }
    }
}
