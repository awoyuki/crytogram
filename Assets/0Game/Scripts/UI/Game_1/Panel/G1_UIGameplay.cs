using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class G1_UIGameplay : MonoBehaviour
{
    private static G1_UIGameplay instance;
    public static G1_UIGameplay Instance
    {
        get
        {
            if (instance is null)
            {
                instance = FindObjectOfType<G1_UIGameplay>();
            }
            return instance;
        }
    }
    private G1_DataContainer dataContainer;

    public G1_DataContainer DataContainer
    {
        get
        {
            if (dataContainer is null)
            {
                dataContainer = new G1_DataContainer();
            }
            return dataContainer;
        }
    }

    public G1_UIKeyboard uiKeyboard;
    public G1_UISentence uiSentence;
    public G1_UIMistakes uiMistakes;
    public G1_UIHint uiHint;
    public ButtonEffectLogic btn_hint, btn_hint_iap;

    private int max_mistaken_count = 3;
    public int MaxMistaken => max_mistaken_count;


    #region Events

    public delegate void OnSelectLetter(G1_UILetter uILetter);
    public OnSelectLetter onSelectLetter;

    #endregion

    private void Awake()
    {
        Events.onEndLevel += UIController.Instance.uiWinPopup.LoadInformation;
        btn_hint.onClick.AddListener(uiHint.HintHandle);
        btn_hint_iap.onClick.AddListener(uiHint.HintIAPHandle);
    }

    private void OnDestroy()
    {
        instance = null;
        Events.onEndLevel -= UIController.Instance.uiWinPopup.LoadInformation;
        btn_hint.onClick.RemoveListener(uiHint.HintHandle);
        btn_hint_iap.onClick.RemoveListener(uiHint.HintIAPHandle);
    }


    public void TrueAnswer()
    {
        var cur_UILetter = uiKeyboard.curentUILetter;
        var listChar = uiSentence.list_letter_uncompleted;
        uiSentence.CheckUnlockPanelWord(cur_UILetter);
        cur_UILetter.ChangeStatus(G1_LetterStatus.Completed);
        cur_UILetter.CheckUnlockLetter();
        uiSentence.SelectNextLetter();
        listChar.Remove(cur_UILetter);
        uiSentence.CheckLetterCompleted(false);
    }

    public void WrongAnswer()
    {
        var mistake = DataContainer.LevelMistake;
        mistake++;
        uiMistakes.UpdateMistaken(mistake);
        if (DataContainer.LevelMistake >= MaxMistaken)
        {
            G1_LevelManager.Instance.LoseLevel();
        }
        else
        {
            DataContainer.LevelMistake = mistake;
        }
    }

    public void EnableHint(bool enable)
    {
        if (enable)
        {
            GameManager.Instance.ChangeGameState(GameState.Hint);
            uiHint.Show();
        }
        else
        {
            GameManager.Instance.ChangeGameState(GameState.InProgress);
            uiHint.Hide();
        }
    }

    public void UseHint(G1_UILetter uILetter)
    {
        uILetter.AnswerLetterAnim("", true);
        uiSentence.CheckUnlockPanelWord(uILetter);
        uILetter.ChangeStatus(G1_LetterStatus.Completed);
        uILetter.CheckUnlockLetter();
        DataContainer.CurrentHint--;
        EnableHint(false);

        if (uiKeyboard.curentUILetter == uILetter)
        {
            uiSentence.SelectNextLetter();
        }
        uiSentence.list_letter_uncompleted.Remove(uILetter);
        uiSentence.CheckLetterCompleted(false);
        uiKeyboard.ReloadAllButton();

    }

}
