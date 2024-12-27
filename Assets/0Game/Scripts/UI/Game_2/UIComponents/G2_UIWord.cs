using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class G2_UIWord : MonoBehaviour
{
    [SerializeField] G2_UILetter letter_prefab;
    [SerializeField] G2_UIPunctuation punctuation_prefab;
    [SerializeField] LayoutGroup layoutGroup;
    public G2_UIHintWord hintWord;
    public List<G2_UILetter> list_letter = new List<G2_UILetter>();
    public List<G2_UIPunctuation> list_punctuation = new List<G2_UIPunctuation>();
    public WordStatus word_status = WordStatus.Normal;

    public void InitWord(string word)
    {
        ClearWord();
        var letter_array = word.ToCharArray();
        for (int i = 0; i < letter_array.Length; i++)
        {
            if (char.IsPunctuation(letter_array[i]))
            {
                var letter = Instantiate(punctuation_prefab, transform);
                letter.InitLetter(letter_array[i], this, G2_LetterStatus.Process);
                list_punctuation.Add(letter);
            }
            else
            {
                var letter = Instantiate(letter_prefab, transform);
                letter.InitLetter(letter_array[i], this, G2_LetterStatus.Process);
                list_letter.Add(letter);
            }
        }
    }

    public void EnableLayoutGroup(bool enabled)
    {
        layoutGroup.enabled = enabled;
    }

    private void ClearWord()
    {
        foreach (G2_UICharacter item in list_letter)
        {
            DestroyImmediate(item);
        }
        list_letter.Clear();
    }
    public void ReloadStatus()
    {
        switch (word_status)
        {
            case WordStatus.Normal:
                hintWord.image.color = Color.clear;
                break;
            case WordStatus.Selected:
                hintWord.image.color = Color.white;
                break;
            case WordStatus.Wrong:
                hintWord.image.color = Color.red;
                break;
            case WordStatus.True:
                List<G2_UILetter> lettersToComplete = new List<G2_UILetter>();
                foreach (G2_UILetter uILetter in list_letter)
                {
                    uILetter.ChangeStatus(G2_LetterStatus.Completed);
                    foreach (G2_UILetter uI_sentence_Letter in G2_UIGameplay.Instance.uiSentence.list_letter_uncompleted)
                    {
                        if (uI_sentence_Letter.character == uILetter.character)
                        {
                            lettersToComplete.Add(uI_sentence_Letter);
                        }
                    }
                }
                G2_UIGameplay.Instance.TrueAnswer();
                hintWord.image.color = Color.cyan;
                DOVirtual.DelayedCall(1f, () =>
                {
                    hintWord.image.DOColor(Color.clear, 1f);
                    foreach (G2_UILetter letter in lettersToComplete)
                    {
                        letter.ChangeStatus(G2_LetterStatus.Completed);
                    }
                });
                break;

        }
    }
    public void Check(List<G2_UILetter> list)
    {
        bool allNotActivate = true;
        foreach (G2_UILetter letter in list)
        {
            if (letter.letter_status == G2_LetterStatus.Process)
            {
                allNotActivate = false;
                break;
            }
        }
        if (allNotActivate)
        {
            bool isAllTrue = true;

            foreach (G2_UILetter uILetter in list_letter)
            {
                if (uILetter.character.ToString() != uILetter.txt_letter.text)
                {
                    Debug.Log("Wrong");
                    ChangeStatus(WordStatus.Wrong);
                    isAllTrue = false;
                    break;
                }
            }

            if (isAllTrue)
            {
                ChangeStatus(WordStatus.True);
            }
        }

    }
    public void ChangeStatus(WordStatus WordStatus)
    {
        word_status = WordStatus;
        ReloadStatus();
    }
    public enum WordStatus
    {
        Normal, Selected, Wrong, True
    }
}
