using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using static Events;
using static Consts;


public struct G1_LetterCounter
{
    public char character;
    public List<G1_UILetter> ui_letters;

    public void CheckCompleted(bool onStart = true)
    {
        int count = 0;
        foreach (var item in ui_letters)
        {
            if (item.letter_status == G1_LetterStatus.Completed)
                count++;
        }
        if (count == ui_letters.Count)
        {
            foreach (var item in ui_letters)
            {
                item.ChangeStatus(G1_LetterStatus.Done);
                if (!onStart) item.CompletedAnim();
            }
        }
    }
}


public class G1_UISentence : MonoBehaviour
{

    [SerializeField] Transform content;
    [SerializeField] G1_UIWord word_prefab;

    public ScrollRect scrollRect;
    public List<G1_UIWord> list_word = new List<G1_UIWord>();
    public List<G1_UIWord> list_locked_word = new List<G1_UIWord>();
    public List<G1_UILetter> list_letter = new List<G1_UILetter>();
    public List<G1_UILetter> list_letter_uncompleted = new List<G1_UILetter>();
    public List<G1_UIPunctuation> list_punctuation = new List<G1_UIPunctuation>();

    public List<G1_LetterCounter> characterCounterArr = new List<G1_LetterCounter>();


    public static int GetAlphabetID(char value)
    {
        for (int i = 0; i < englishAlphabetArr.Length; i++)
        {
            if (englishAlphabetArr[i] == value)
                return i + 1;
        }
        return -1;
    }

    public void LoadSentenceFromData(G1_SO_DataInfoLevel data)
    {
        englishAlphabetArr = ShuffleArray(englishAlphabetArr);
        var word_array = data.word_cryptos_array;
        for (int i = 0; i < word_array.Length; i++)
        {
            var word = Instantiate(word_prefab, content);
            word.InitWord(word_array[i]);
            list_word.Add(word);
            list_letter.AddRange(word.list_letter);
            list_punctuation.AddRange(word.list_punctuation);
            if (word_array[i].isLocked > 0)
                list_locked_word.Add(word);
        }
        InitCharacterCounter();
        LoadCryptoProcessData();
        InitLettersUncompleted();
        CheckLetterCompleted();
        SelectNextLetter();
    }

    private void LoadCryptoProcessData()
    {
        // Load word cryto status
        var word_cryto_array = G1_UIGameplay.Instance.DataContainer.LevelWordsStatus.ToCharArray();
        if (word_cryto_array.Length > 0)
        {
            for (int i = 0; i < list_word.Count; i++)
            {
                list_word[i].InitStatus((int)Char.GetNumericValue(word_cryto_array[i]));
            }
        }

        // Load character cryto status
        var crypto_array = G1_UIGameplay.Instance.DataContainer.LevelProcess.ToCharArray();
        if (crypto_array.Length > 0)
        {
            for (int i = 0; i < list_letter.Count; i++)
            {
                list_letter[i].ChangeStatus((G1_LetterStatus)Char.GetNumericValue(crypto_array[i]));
            }
        }
        else
        {
            //Check unsual set-up and rebase them

            for (int i = 0; i < list_word.Count; i++)
            {
                list_word[i].CheckLockPerWord();
            }

            /*for (int i = 0; i < list_letter.Count; i++)
            {
                var current_letter = list_letter[i];
                if (current_letter.IsFirstOrLastLetter())
                {
                    if (current_letter.letter_status != G1_LetterStatus.DoubleLock && current_letter.letter_status != G1_LetterStatus.Lock)
                        continue;

                    if(current_letter.word.list_letter.Count == 1)
                    {
                        current_letter.ChangeStatus(G1_LetterStatus.Process);
                    }
                    else 
                    {
                        G1_UILetter checking_letter = null;
                        if (i - 1 > 0 && i + 1 < list_letter.Count)
                        {
                            if(current_letter == current_letter.word.FirstLetter)
                                checking_letter = list_letter[i + 1];
                            else if (current_letter == current_letter.word.LastLetter)
                                checking_letter = list_letter[i + 1];
                        }
                            
                        if (current_letter.letter_status == G1_LetterStatus.DoubleLock)
                            current_letter.ChangeStatus(G1_LetterStatus.Lock);

                        if (checking_letter != null && ( checking_letter.letter_status == G1_LetterStatus.DoubleLock || checking_letter.letter_status == G1_LetterStatus.Lock))
                        {
                            if(checking_letter.letter_status == G1_LetterStatus.DoubleLock)
                                checking_letter.ChangeStatus(checking_letter.IsFirstOrLastLetter() ? G1_LetterStatus.Process : G1_LetterStatus.Lock);
                            else if (checking_letter.letter_status == G1_LetterStatus.Lock && checking_letter.IsFirstOrLastLetter())
                                checking_letter.ChangeStatus(G1_LetterStatus.Process);
                        }
                    }
                }
            }*/
        }

    }

    public void RemoveSentence()
    {
        foreach (var item in list_word)
        {
            DestroyImmediate(item.gameObject);
        }
        list_word.Clear();
        list_letter.Clear();
        list_letter_uncompleted.Clear();
        list_punctuation.Clear();
        list_locked_word.Clear();
        characterCounterArr.Clear();
    }

    private void InitCharacterCounter()
    {
        characterCounterArr.Clear();
        for (int i = 0; i < Consts.englishAlphabetArr.Length; i++)
        {
            var newChar = new G1_LetterCounter();
            newChar.character = Consts.englishAlphabetArr[i];
            newChar.ui_letters = new List<G1_UILetter>();
            foreach (var item in list_letter)
            {
                if (item is G1_UILetter && Consts.englishAlphabetArr[i] == item.character)
                    newChar.ui_letters.Add(item);
            }
            characterCounterArr.Add(newChar);
        }
    }

    private void InitLettersUncompleted()
    {
        list_letter_uncompleted.Clear();
        foreach (var item in list_letter)
        {
            if (item.letter_status != G1_LetterStatus.Completed && item.letter_status != G1_LetterStatus.Done)
                list_letter_uncompleted.Add(item);
            else
                item.CheckUnlockLetter();
        }
    }

    public void CheckLetterCompleted(bool onStart = true)
    {
        for (int i = 0; i < characterCounterArr.Count; i++)
        {
            characterCounterArr[i].CheckCompleted(onStart);
        }

        G1_UIGameplay.Instance.DataContainer.LevelProcess = GetCryptoProcess();
        if (list_letter_uncompleted.Count <= 0 && list_locked_word.Count <= 0)
            BaseLevelManager.Instance.WinLevel();
    }

    public void CheckUnlockPanelWord(G1_UILetter cur_UILetter)
    {
        if (cur_UILetter.letter_status == G1_LetterStatus.HasKey)
        {
            var locked_word = list_locked_word[0];
            locked_word.ChangeStatus(cur_UILetter);
            if (locked_word.isLocked == 0)
                list_locked_word.Remove(locked_word);
            G1_UIGameplay.Instance.DataContainer.LevelWordsStatus = GetWordsStatus();
        }
    }

    public string GetCryptoProcess()
    {
        string result = "";
        foreach (var item in list_letter)
        {
            int process_id = (int)item.letter_status;
            result += process_id;
        }
        return result;
    }

    public string GetWordsStatus()
    {
        string result = "";
        foreach (var item in list_word)
        {
            result += item.isLocked;
        }
        return result;
    }


    public void SelectNextLetter()
    {
        var curID = list_letter_uncompleted.IndexOf(G1_UIGameplay.Instance.uiKeyboard.curentUILetter);
        while (true)
        {
            if (curID >= list_letter_uncompleted.Count)
                return;

            int nextID = (curID + 1) % list_letter_uncompleted.Count;
            var letter = list_letter_uncompleted[nextID];
            if (Consts.InteractableLetterStatus.Contains(letter.letter_status) && letter.word.isLocked <= 0)
            {
                letter.OnClickBtn();
                ScrollTo(letter.word.rect());
                return;
            }
            else
            {
                curID++;
                continue;
            }
        }
    }

    public void ScrollTo(RectTransform target)
    {
        Vector3[] viewportCorners = new Vector3[4];
        scrollRect.viewport.GetWorldCorners(viewportCorners);

        Vector3[] targetCorners = new Vector3[4];
        target.GetWorldCorners(targetCorners);

        bool isOutOfView = targetCorners[0].y < viewportCorners[0].y ||
                           targetCorners[1].y > viewportCorners[1].y;

        if (isOutOfView)
        {
            Vector2 childLocalPosition = target.localPosition;
            float normalizedPosition = 1 - Mathf.Abs(childLocalPosition.y / content.rect().rect.height);
            normalizedPosition = Mathf.Clamp01(normalizedPosition);
            scrollRect.DOKill();
            scrollRect.DOVerticalNormalizedPos((float)Math.Floor(normalizedPosition * 10f) / 10f, 0.2f).SetEase(Ease.OutCubic);
        }
    }    

}
