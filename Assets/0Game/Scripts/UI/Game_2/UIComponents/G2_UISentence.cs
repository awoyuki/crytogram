using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;
using static Consts;

public class G2_UISentence : MonoBehaviour
{
    public struct LetterCounter
    {
        public char character;
        public List<G2_UILetter> ui_letters;

        public void CheckCompleted()
        {
            int count = 0;
            foreach (var item in ui_letters)
            {
                if (item.letter_status == G2_LetterStatus.Completed)
                    count++;
            }
            if(count == ui_letters.Count)
            {
                foreach (var item in ui_letters)
                {
                    //item.ChangeStatus(LetterStatus.Done);
                }
            }
        }
    }

    [SerializeField] ScrollRect scrollRect;
    [SerializeField] Transform content;
    [SerializeField] Transform content1;
    [SerializeField] G2_UIWord word_prefab;
    [SerializeField] G2_UIHintWord hint_word_prefab;
    public G2_SO_DataInfoLevel data_level;

    public List<G2_UIWord> list_word = new List<G2_UIWord>();
    public List<G2_UIHintWord> list_hint_word = new List<G2_UIHintWord>();
    public List<G2_UILetter> list_letter = new List<G2_UILetter>();
    public List<G2_UILetter> list_letter_uncompleted = new List<G2_UILetter>();
    public List<G2_UIPunctuation> list_punctuation = new List<G2_UIPunctuation>();

    public List<LetterCounter> characterCounterArr = new List<LetterCounter>();

    public static int GetAlphabetID(char value)
    {
        for (int i = 0; i < englishAlphabetArr.Length; i++)
        {
            if (englishAlphabetArr[i] == value)
                return i + 1;
        }
        return -1;
    }
    public void LoadSentenceFromData(G2_SO_DataInfoLevel data)
    {
        data_level = data;
        englishAlphabetArr = ShuffleArray(englishAlphabetArr);
        var word_array = data.so_sentence.ToUpper().Split(' ');
        for (int i = 0; i < word_array.Length; i++)
        {
            var word = Instantiate(word_prefab, content);
            word.InitWord(word_array[i]);
            list_word.Add(word);
            list_letter.AddRange(word.list_letter);
            list_punctuation.AddRange(word.list_punctuation);
        }
        InitCharacterCounter();
        InitHintWord(data);
        LoadCryptoProcessData();
        InitLettersUncompleted();
        CheckLetterCompleted();
        G2_UIGameplay.Instance.onSelectLetter?.Invoke(list_letter_uncompleted[data.so_sentence.Count(char.IsLetter)-1]);
        SelectNextLetter();
        foreach (var word in list_word)
        {
            word?.Check(word.list_letter);
        }
    }
    public void InitHintWord(G2_SO_DataInfoLevel data)
    {
        foreach (G2_UIHintWord item in list_hint_word)
        {
            DestroyImmediate(item.gameObject);
        }
        list_hint_word.Clear();
        for (int i = 0;i < data.dataHintWord.Count;i++)
        {
            var hint_word = Instantiate(hint_word_prefab, content1);
            hint_word.word.InitWord(data.dataHintWord[i].so_letter.ToUpper());
            hint_word.UpdateDes(data.dataHintWord[i].so_describe);
            list_hint_word.Add(hint_word);
            list_word.Add(hint_word.word);
            list_letter.AddRange(hint_word.word.list_letter);
            list_punctuation.AddRange(hint_word.word.list_punctuation);
        }

    }
    private void LoadCryptoProcessData()
    {    
        var process = G2_UIGameplay.Instance.DataContainer.LevelProcess;
        var array = (string.IsNullOrEmpty(process) ? data_level.so_status_crypto_en.ToCharArray() : process.ToCharArray());
        var text_process = G2_UIGameplay.Instance.DataContainer.LevelTextProces;
        var text_array = (string.IsNullOrEmpty(text_process) ? data_level.so_status_text_en.ToCharArray() : text_process.ToCharArray());

        for (int i = 0; i < list_letter.Count; i++)
        {
            list_letter[i].ChangeStatus((G2_LetterStatus)Char.GetNumericValue(array[i]));
            list_letter[i].ChangeText(text_array[i].ToString());
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
        characterCounterArr.Clear();
    }

    private void InitCharacterCounter()
    {
        characterCounterArr.Clear();
        for (int i = 0; i < englishAlphabetArr.Length; i++)
        {
            var newChar = new LetterCounter();
            newChar.character = englishAlphabetArr[i];
            newChar.ui_letters = new List<G2_UILetter>();
            foreach (var item in list_letter)
            {
                if (item is G2_UILetter && englishAlphabetArr[i] == item.character)
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
            if (item.letter_status != G2_LetterStatus.Completed)
                list_letter_uncompleted.Add(item);
            //else
            //    item.CheckUnlockLetter();
        }
    }

    public void CheckLetterCompleted()
    {
        G2_UIGameplay.Instance.DataContainer.LevelProcess = GetCryptoProcess();
        G2_UIGameplay.Instance.DataContainer.LevelTextProces = GetTextProcess();

        if (list_letter_uncompleted.Count <= 0)
            G2_LevelManager.Instance.WinLevel();
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

    public string GetTextProcess()
    {
        string result = "";
        foreach (var item in list_letter)
        {
            string process_id = item.txt_letter.text;
            result += process_id;
        }
        return result;
    }

    public void SelectNextLetter()
    {
        var curID = list_letter_uncompleted.IndexOf(G2_UIGameplay.Instance.uiKeyboard.curentUILetter);
        
        while (true)
        {
            if (curID >= list_letter_uncompleted.Count || list_letter_uncompleted.Count==0)
                return;

            int nextID = (curID + 1) % list_letter_uncompleted.Count;
            var letter = list_letter_uncompleted[nextID];
            if (letter.letter_status != G2_LetterStatus.Completed)
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
