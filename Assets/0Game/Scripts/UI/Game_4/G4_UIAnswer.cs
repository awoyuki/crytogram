using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using System.Text;

public class G4_UIAnswer : MonoBehaviour
{
    [SerializeField] Image bg_answer;
    [SerializeField] Animator animator_answer;
    [SerializeField] Animator animator_bonus;
    [SerializeField] G4_UILetter letterPrefabs;

    private List<G4_UILetter> letterList = new List<G4_UILetter>();
    private StringBuilder str_anwser = new StringBuilder();
    private List<G4_UILetter> usingLetterList = new List<G4_UILetter>();

    private AnswerState answerState = AnswerState.None;

    enum AnswerState
    {
        None, True, Wrong, Answered, Bonus, BonusAnswered
    }

    public void AnswerTrue()
    {
        animator_answer?.Play("G4_Answered", 0, 0);
        answerState = AnswerState.True;
    }

    public void AnswerWrong()
    {
        animator_answer?.Play("G4_AnswerWrong", 0, 0);
        answerState = AnswerState.Wrong;
    }

    public void Answered()
    {
        animator_answer?.Play("G4_Answered", 0, 0);
        answerState = AnswerState.Answered;
    }

    public void AnsweredBonus()
    {
        animator_answer?.Play("G4_Answered", 0, 0);
        animator_bonus.Play("G4_BonusFlick", 0, 0);
        answerState = AnswerState.BonusAnswered;
    }

    public void AnswerBonus()
    {
        animator_answer?.Play("G4_Answered", 0, 0);
        animator_bonus.Play("G4_BonusFlick", 0, 0);
        answerState = AnswerState.Bonus;
    }


    public void Show()
    {
        ClearTextField();
        gameObject.SetActive(true);
        bg_answer.DOKill();
        bg_answer.DOFade(1, 0.1f);
        foreach (var item in usingLetterList)
        {
            item.FadeShow();
        }
    }

    public void OnAnimationFinish()
    {
        switch (answerState)
        {
            case AnswerState.True:
                Hide();
                break;
            case AnswerState.Wrong:
                foreach (var item in usingLetterList)
                {
                    item.FadeHide();
                }
                Hide();
                break;
            case AnswerState.Answered:
                foreach (var item in usingLetterList)
                {
                    item.FadeHide();
                }
                Hide();
                break;
            case AnswerState.Bonus:
                Hide();
                break;
            case AnswerState.BonusAnswered:
                foreach (var item in usingLetterList)
                {
                    item.FadeHide();
                }
                Hide();
                break;
        }
    }

    public void Hide()
    {
        bg_answer.DOKill();
        bg_answer.DOFade(0, 0.2f).OnComplete(() =>
        {
            gameObject.SetActive(false);
            ClearTextField();
        });
    }

    private void ClearTextField()
    {
        str_anwser = new StringBuilder();
        usingLetterList.Clear();
        foreach (var item in letterList)
        {
            item.gameObject.SetActive(false);
        }
    }

    public void AddAnswer(string answer)
    {
        str_anwser.Append(answer);
        var index = (str_anwser.Length - 1);
        G4_UILetter new_letter = null;
        if (index < letterList.Count)
        {
            new_letter = letterList[index];
            new_letter.gameObject.SetActive(true);
        }
        else
        {
            new_letter = Instantiate(letterPrefabs, transform);
            letterList.Add(new_letter);
        }
        new_letter.LetterKeyboardInit(answer);
        usingLetterList.Add(new_letter);
    }

    public void RemoveAnswer(int index)
    {
        if (index < letterList.Count)
        {
            var letter = letterList[index];
            letter.gameObject.SetActive(false);
            str_anwser.Remove(index, 1);
            usingLetterList.Remove(letter);
        }
    }

    public string GetAnswer()
    {
        return str_anwser.ToString();
    }

    public void PlayBonusAnim()
    {
        StartCoroutine(IE_PlayBonusAnim());
    }

    public void PlayAnswerAnim(List<G4_UILetter> letter_list_anim)
    {
        ReorderListAnim(letter_list_anim);
        StartCoroutine(IE_PlayAnswerAnim(letter_list_anim));

    }

    public void PlayAnsweredAnim(List<G4_UILetter> letter_list_anim)
    {
        var isVertical = ReorderListAnim(letter_list_anim);
        for (int i = 0; i < letter_list_anim.Count; i++)
        {
            letter_list_anim[i].ShakeBlink(isVertical);
        }
    }

    private bool ReorderListAnim(List<G4_UILetter> letter_list_anim)
    {
        //reorder
        bool isVertical = false;
        for (int i = 0; i < letter_list_anim.Count - 1; i++)
        {
            if (letter_list_anim[i].index_col == letter_list_anim[i + 1].index_col)
            {
                isVertical = true;
                int minIndex = i;
                for (int j = i + 1; j < letter_list_anim.Count; j++)
                {
                    if (letter_list_anim[j].index_row < letter_list_anim[minIndex].index_row)
                    {
                        minIndex = j;
                    }
                }

                var temp = letter_list_anim[minIndex];
                letter_list_anim[minIndex] = letter_list_anim[i];
                letter_list_anim[i] = temp;
            }
            else
            {
                int minIndex = i;
                for (int j = i + 1; j < letter_list_anim.Count; j++)
                {
                    if (letter_list_anim[j].index_col < letter_list_anim[minIndex].index_col)
                    {
                        minIndex = j;
                    }
                }

                var temp = letter_list_anim[minIndex];
                letter_list_anim[minIndex] = letter_list_anim[i];
                letter_list_anim[i] = temp;
            }
        }
        return isVertical;
    }

    IEnumerator IE_PlayAnswerAnim(List<G4_UILetter> letter_list_anim)
    {
        for (int i = 0; i < usingLetterList.Count; i++)
        {
            if (i >= letter_list_anim.Count)
                yield break;
            yield return Yielder.Get(0.05f);
            usingLetterList[i].MoveToAnswerPanel(letter_list_anim[i], 0.3f);
        }
    }

    IEnumerator IE_PlayBonusAnim()
    {
        var pos = G4_UIGameplay.Instance.img_bonus.transform.position;
        for (int i = 0; i < usingLetterList.Count; i++)
        {
            yield return Yielder.Get(0.05f);
            usingLetterList[i].MoveToAnswerPanel(pos, 0.3f);
        }
    }


}
