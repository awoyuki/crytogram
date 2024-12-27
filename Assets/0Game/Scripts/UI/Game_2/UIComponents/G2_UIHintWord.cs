using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using static G2_UIHintWord;
using TMPro;
public class G2_UIHintWord : MonoBehaviour
{
    [SerializeField] Text describeText;
    public Image image;

    public G2_UIWord word;
    //public HintWordStatus hint_word_status = HintWordStatus.Normal;
    //public void Check(List<G2_UILetter> list)
    //{
    //    Debug.Log("Check");
    //    bool allNotActivate = true;
    //    foreach (G2_UILetter letter in list)
    //    {
    //        if (letter.letter_status == LetterStatus.Process)
    //        {
    //            allNotActivate = false;
    //            break;
    //        }
    //    }
    //    if (allNotActivate)
    //    {
    //        Debug.Log("full");
    //        bool isAllTrue = true;

    //        foreach (G2_UILetter uILetter in word.list_letter)
    //        {
    //            if (uILetter.character.ToString() != uILetter.txt_letter.text)
    //            {
    //                Debug.Log("Wrong");
    //                ChangeStatus(HintWordStatus.Wrong);
    //                isAllTrue = false;
    //                break;
    //            }
    //        }

    //        if (isAllTrue)
    //        {
    //            ChangeStatus(HintWordStatus.True);
    //        }
    //    }
        
    //}
    //public void ChangeStatus(HintWordStatus hintWordStatus)
    //{
    //    hint_word_status = hintWordStatus;
    //    ReloadStatus();
    //}
    public void UpdateDes(string txt)
    {
        describeText.text = txt;
    }
    //public void ReloadStatus()
    //{
    //    switch (hint_word_status)
    //    {
    //        case HintWordStatus.Normal:
    //            image.color = Color.clear;
    //            break;
    //        case HintWordStatus.Selected:
    //            image.color = Color.white;
    //            break;
    //        case HintWordStatus.Wrong:
    //            image.color = Color.red;
    //            break;
    //        case HintWordStatus.True:
    //            foreach (G2_UILetter uILetter in word.list_letter)
    //            {
    //                uILetter.ChangeStatus(LetterStatus.Completed);
    //                List<G2_UILetter> lettersToComplete = new List<G2_UILetter>();
    //                foreach (G2_UILetter uI_sentence_Letter in G2_UIController.Instance.uiGameplay.uiSentence.list_letter_uncompleted)
    //                {
    //                    if (uI_sentence_Letter.character == uILetter.character)
    //                    {
    //                        lettersToComplete.Add(uI_sentence_Letter);
    //                    }
    //                }
    //                foreach (G2_UILetter letter in lettersToComplete)
    //                {
    //                    letter.ChangeStatus(LetterStatus.Completed);
    //                }
    //            }
    //            G2_LevelManager.instance.TrueAnswer();
    //            image.color = Color.cyan;
    //            DOVirtual.DelayedCall(2f, () =>
    //            {
    //                image.DOColor(Color.clear, 1f);
    //            });
    //            break;
          
    //    }
    //}
    //public enum HintWordStatus
    //{
    //    Normal, Selected,Wrong, True
    //}
}
