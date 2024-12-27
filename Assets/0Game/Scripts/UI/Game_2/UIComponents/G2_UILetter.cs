using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class G2_UILetter : G2_UICharacter
{
    [SerializeField] Animator animator;
    [SerializeField] Text txt_id;
    [SerializeField] Text txt_wrongletter;
    [SerializeField] Image bg_img;
    [SerializeField] ButtonEffectLogic btn;
    [SerializeField] GameObject hint;
    [SerializeField] RectTransform mousePointer;

    public G2_LetterStatus letter_status = G2_LetterStatus.Process;

    public G2_UIWord word;
    //public G2_UIHintWord hintWord;
    private int index;
    private void Awake()
    {
        btn.onClick.AddListener(OnClickBtn);
        G2_UIGameplay.Instance.onSelectLetter += ChangeButtonStatment;
        G2_UIGameplay.Instance.onChangeLetterStatus += OnChangeLetterStatus;
        G2_UIGameplay.Instance.onChangeLetterText += OnChangeLetterText;
    }


    public void ActiveHint(Transform uiHint)
    {
        transform.SetParent(uiHint);
        //hint?.SetActive(false);
    }

    public void ResetPosition()
    {
        transform.SetParent(word.transform);
        transform.SetSiblingIndex(index);
        //hint?.SetActive(true);
    }


    public override void InitLetter(char letter, G2_UIWord word, G2_LetterStatus letterStatus)
    {
        character = letter;
        txt_id.text = G2_UISentence.GetAlphabetID(letter).ToString();
        txt_letter.text = letter.ToString();
        ChangeStatus(letterStatus);
        this.word = word;
        index = transform.GetSiblingIndex();
    }

    public void ChangeStatus(G2_LetterStatus letterStatus)
    {
        letter_status = letterStatus;
        //G2_DataController.instance.LevelProcess = G2_UIController.Instance.uiGameplay.uiSentence.GetCryptoProcess();
        ReloadStatus();
    }

    public void PlayLetterAnim(string name)
    {
       
        animator?.Play(name, 0, 0);
       
    }

    public void OnClickBtn()
    {
        //if (G2_LevelManager.instance.game_state == GameState.Hint)
        //{
        //    G2_LevelManager.instance.UseHint(this);
        //    return;
        //}
        //if (G2_LevelManager.instance.game_state != GameState.InProgress ||
        //    letter_status == LetterStatus.Completed)
        //    return;
        //if (letter_status == LetterStatus.Process)
        //{

        //}

        G2_UIGameplay.Instance.onSelectLetter?.Invoke(this);
    }
   
    private void ChangeButtonStatment(G2_UILetter uILetter)
    {
        if (uILetter.character == character)
        {
            bg_img.color = Color.green;
        }
        else
        {
            bg_img.color = Color.white;
        }
        if (uILetter.word.word_status != G2_UIWord.WordStatus.True)
        {
            foreach (G2_UIWord word in G2_UIGameplay.Instance.uiSentence.list_word)
            {
                if (word.word_status != G2_UIWord.WordStatus.Wrong)
                {
                    if (word == uILetter.word)
                    {
                        word.ChangeStatus(G2_UIWord.WordStatus.Selected);
                    }
                    else
                    {
                        word.ChangeStatus(G2_UIWord.WordStatus.Normal);
                    }
                }
            }
        }
        if (uILetter == this)
        {
            PlayLetterAnim("SelectLetter");
        }
        else
        {
            PlayLetterAnim("BlankState");
        }
    }

    private void OnChangeLetterStatus(G2_LetterStatus letterStatus, G2_UILetter uILetter)
    {
        if (character == uILetter.character)
        {
            ChangeStatus(letterStatus);
        }
    }
    private void OnChangeLetterText(string txt, G2_UILetter uILetter)
    {
        if (character == uILetter.character)
        {
            ChangeText(txt);
            word?.Check(word.list_letter);
        }
    }
    public void ChangeText(string txt)
    {
        txt_letter.text = txt;
    }
    public void ReloadStatus()
    {
        
        switch (letter_status)
        {
            case G2_LetterStatus.None:
                txt_letter.gameObject.SetActive(false);
                mousePointer.localPosition = new Vector2(-25,25);
                txt_id.gameObject.SetActive(false);
                break;
            case G2_LetterStatus.Process:
                txt_letter.gameObject.SetActive(false);
                mousePointer.localPosition = new Vector2(-25, 25);
                txt_id.gameObject.SetActive(true);
                break;
            case G2_LetterStatus.Activate:
                txt_letter.gameObject.SetActive(true);
                mousePointer.localPosition = new Vector2(25, 25);
                txt_id.gameObject.SetActive(true);
                break;
            case G2_LetterStatus.Completed:
                txt_letter.gameObject.SetActive(true);
                txt_id.gameObject.SetActive(true);
                txt_letter.text=character.ToString();
                PlayLetterAnim("BlankState");
                bg_img.color = Color.cyan;
                btn.onClick.RemoveListener(OnClickBtn);
                G2_UIGameplay.Instance.uiSentence.list_letter_uncompleted.Remove(this);
                G2_UIGameplay.Instance.onSelectLetter -= ChangeButtonStatment;
                break;
        }
    }

#if UNITY_EDITOR
    public void ReloadStatusEditor()
    {
        txt_letter.gameObject.SetActive(true);
        txt_id.gameObject.SetActive(true);
    }
    //[Button]
    //private void Completed()
    //{
    //    letter_status = LetterStatus.Completed;
    //    ReloadStatus();
    //}
    //[Button]
    //private void ResetStatus()
    //{
    //    letter_status = LetterStatus.Process;
    //    ReloadStatus();
    //}


#endif


}

public enum G2_LetterStatus
{
    None = 0, Process = 1,  Activate = 2, Completed = 5
}

public abstract class G2_UICharacter: MonoBehaviour
{
    public Text txt_letter;
    [HideInInspector] public char character;
    public abstract void InitLetter(char letter, G2_UIWord word, G2_LetterStatus letterStatus);
}