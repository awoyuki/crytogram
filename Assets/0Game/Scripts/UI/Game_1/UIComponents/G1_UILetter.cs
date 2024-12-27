using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class G1_UILetter : G1_UICharacter
{
    [SerializeField] Animator animator;
    [SerializeField] Text txt_id;
    [SerializeField] Text txt_wrongletter;
    [SerializeField] Image bg_img;
    [SerializeField] ButtonEffectLogic btn;
    [SerializeField] GameObject hint;
    [SerializeField] GameObject container;
    [SerializeField] G1_UILetterLock letterLock;
    [SerializeField] ReOrderUI reOrderUI;

    public G1_LetterStatus letter_status = G1_LetterStatus.Process;

    public G1_UIWord word;
    public GameObject key;


    private void Awake()
    {
        btn.onClick.AddListener(OnClickBtn);
        G1_UIGameplay.Instance.onSelectLetter += ChangeButtonStatment;
    }


    public void ActiveHint()
    {
        if (word.isLocked > 0)
            return;
        if(Consts.InteractableLetterStatus.Contains(letter_status))
            hint?.SetActive(true);
    }

    public void ResetHint()
    {
        hint?.SetActive(false);
    }


    public override void InitLetter(char letter, G1_UIWord word, G1_LetterStatus letterStatus)
    {
        character = letter;
        txt_id.text = G1_UISentence.GetAlphabetID(letter).ToString();
        txt_letter.text = letter.ToString();
        this.word = word;
        ChangeStatus(letterStatus);
    }

    public void ChangeStatus(G1_LetterStatus letterStatus)
    {
        letter_status = letterStatus;
        ReloadStatus();            
    }

    public void CheckUnlockLetter()
    {
        var left_letter_id = word.list_letter.IndexOf(this) - 1;
        var right_letter_id = word.list_letter.IndexOf(this) + 1;
        if(left_letter_id >= 0)
            word.list_letter[left_letter_id].UnlockLetter(true);
        if(right_letter_id < word.list_letter.Count) 
            word.list_letter[right_letter_id].UnlockLetter(false);
    }

    public void AnswerLetterAnim(string letter, bool result)
    {
        if (result)
        {
            animator?.Play("G1_TrueAnswer", 0, 0);
        }
        else
        {
            animator?.Play("G1_WrongAnswer", 0, 0);
            txt_wrongletter.text = letter;
        }
    }

    public void CompletedAnim()
    {
        animator?.Play("G1_Completed", 0, 0);
    }

    public void OnClickBtn()
    {
        if (GameManager.Instance.GameState == GameState.Hint)
        {
            G1_UIGameplay.Instance.UseHint(this);
            return;
        }
        if (GameManager.Instance.GameState != GameState.InProgress || 
            letter_status == G1_LetterStatus.Lock || letter_status == G1_LetterStatus.DoubleLock || 
            letter_status == G1_LetterStatus.LockLeft || letter_status == G1_LetterStatus.LockRight )
            return;

        G1_UIGameplay.Instance.onSelectLetter?.Invoke(this);
    }

    private void ChangeButtonStatment(G1_UILetter uILetter)
    {
        if (uILetter == this)
        {
            bg_img.color = Color.green;
        }
        else
        {
            bg_img.color = Color.white;
        }
    }

    public void ReloadStatus()
    {
        switch (letter_status)
        {
            case G1_LetterStatus.None:
                container.SetActive(true);
                key.SetActive(false);
                letterLock.gameObject.SetActive(false);
                txt_letter.gameObject.SetActive(true);
                txt_id.gameObject.SetActive(true);
                break;
            case G1_LetterStatus.Process:
                container.SetActive(true);
                key.SetActive(false);
                letterLock.gameObject.SetActive(false);
                txt_letter.gameObject.SetActive(false);
                break;
            case G1_LetterStatus.HasKey:
                container.SetActive(true);
                key.SetActive(true);
                letterLock.gameObject.SetActive(false);
                txt_letter.gameObject.SetActive(false);
                break;
            case G1_LetterStatus.Completed:
                container.SetActive(true);
                key.SetActive(false);
                letterLock.gameObject.SetActive(false);
                txt_letter.gameObject.SetActive(true);
                txt_id.gameObject.SetActive(true);
                bg_img.color = Color.white;
                btn.onClick.RemoveListener(OnClickBtn);
                G1_UIGameplay.Instance.onSelectLetter -= ChangeButtonStatment;
                break;
            case G1_LetterStatus.Done:
                container.SetActive(true);
                key.SetActive(false);
                letterLock.gameObject.SetActive(false);
                txt_letter.gameObject.SetActive(true);
                txt_id.gameObject.SetActive(false);
                btn.onClick.RemoveListener(OnClickBtn);
                G1_UIGameplay.Instance.onSelectLetter -= ChangeButtonStatment;
                break;
            case G1_LetterStatus.Lock:
                container.SetActive(false);
                key.SetActive(false);
                letterLock.gameObject.SetActive(true);
                letterLock.InitLock(G1_LetterStatus.Lock);
                break;
            case G1_LetterStatus.DoubleLock:
                container.SetActive(false);
                key.SetActive(false);
                letterLock.gameObject.SetActive(true);
                letterLock.InitLock(G1_LetterStatus.DoubleLock);
                break;
            case G1_LetterStatus.LockRight:
                container.SetActive(false);
                key.SetActive(false);
                letterLock.gameObject.SetActive(true);
                letterLock.InitLock(G1_LetterStatus.LockRight);
                break;
            case G1_LetterStatus.LockLeft:
                container.SetActive(false);
                key.SetActive(false);
                letterLock.gameObject.SetActive(true);
                letterLock.InitLock(G1_LetterStatus.LockLeft);
                break;
        }
    }


    public void UnlockLetter(bool left)
    {
        switch (letter_status)
        {
            case G1_LetterStatus.Lock: case G1_LetterStatus.LockLeft: case G1_LetterStatus.LockRight:
                letterLock.UnLock();
                ChangeStatus(G1_LetterStatus.Process);
                break;
            case G1_LetterStatus.DoubleLock:
                letterLock.UnDoubleLock(left);
                ChangeStatus(left ? G1_LetterStatus.LockRight : G1_LetterStatus.LockLeft);
                break;
        }
    }

}

public enum G1_LetterStatus
{
    None = 0, Process = 1, Lock = 2, DoubleLock = 3, HasKey = 4, Completed = 5, Done = 6, LockLeft = 7, LockRight = 8, HasCollectable = 9
}

public abstract class G1_UICharacter: MonoBehaviour
{
    [SerializeField] protected Text txt_letter;
    [HideInInspector] public char character;
    public abstract void InitLetter(char letter, G1_UIWord word, G1_LetterStatus letterStatus);
}