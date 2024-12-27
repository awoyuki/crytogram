using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class G2_UIGameplay : MonoBehaviour
{
    private static G2_UIGameplay instance;
    public static G2_UIGameplay Instance
    {
        get
        {
            if(instance is null)
            {
                instance = FindObjectOfType<G2_UIGameplay>();
            }
            return instance;
        }
    }


    private G2_DataContainer dataContainer;

    public G2_DataContainer DataContainer
    {
        get
        {
            if (dataContainer is null)
            {
                dataContainer = new G2_DataContainer();
            }
            return dataContainer;
        }
    }


    public G2_UIKeyboard uiKeyboard;
    public G2_UISentence uiSentence;
    public G2_UIHint uiHint;
    public ButtonEffectLogic btn_hint, btn_hint_iap;


    #region Events

    public delegate void OnSelectLetter(G2_UILetter uILetter);
    public OnSelectLetter onSelectLetter;

    public delegate void OnChangeLetterStatus(G2_LetterStatus letterStatus, G2_UILetter uILetter);
    public OnChangeLetterStatus onChangeLetterStatus;

    public delegate void OnChangeLetterText(string txt, G2_UILetter uILetter);
    public OnChangeLetterText onChangeLetterText;

    #endregion


    protected void Awake()
    {
        Events.onEndLevel += UIController.Instance.uiWinPopup.LoadInformation;
        btn_hint.onClick.AddListener(uiHint.HintHandle);
        btn_hint_iap.onClick.AddListener(uiHint.HintIAPHandle);
        uiHint.UpdateHintUI();
    }

    private void OnDestroy()
    {
        instance = null;
        Events.onEndLevel -= UIController.Instance.uiWinPopup.LoadInformation;
        btn_hint.onClick.RemoveListener(uiHint.HintHandle);
        btn_hint_iap.onClick.RemoveListener(uiHint.HintIAPHandle);
    }

    public void EnableHint()
    {
        var cur_UILetter = uiKeyboard.curentUILetter;
        var keyPrefabs = uiKeyboard.uiKeyPrefabs;
        for (int i = 0; i < keyPrefabs.Length; i++)
        {
            if (keyPrefabs[i].txt_Character.text == cur_UILetter.character.ToString())
            {
                keyPrefabs[i].OnButtonClick();
                onChangeLetterStatus?.Invoke(G2_LetterStatus.Completed, cur_UILetter);
                break;
            }
        }
        dataContainer.CurrentHint--;
        uiHint.UpdateHintUI();
    }

    public void TrueAnswer()
    {
        //var uiGameplay = G2_UIController.Instance.uiGameplay;
        //uiGameplay.uiSentence.CheckLetterCompleted();
        //G2_DataController.instance.LevelProcess = G2_UIController.Instance.uiGameplay.uiSentence.GetCryptoProcess();

    }

    public void StartGame()
    {
        
    }
    

}
