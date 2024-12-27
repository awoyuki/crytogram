using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class G4_UIKeyboard : MonoBehaviour
{
    [SerializeField] CircleLayoutGroup layoutGroup;
    [SerializeField] G4_UILetterKeyboard letterKeyboardPrefabs;
    private List<G4_UILetterKeyboard> letterKeyboardsPool = new List<G4_UILetterKeyboard>();
    private List<G4_UILetterKeyboard> letterKeyboards = new List<G4_UILetterKeyboard>();

    public List<G4_UILetterKeyboard> cur_letter_keyboard_list = new List<G4_UILetterKeyboard>();

    [HideInInspector] public int cur_letter_index = -1;
    public G4_UIAnswer uiAnswer;

    public bool IsCreating { get; private set; }

    private Coroutine cor_drawing_line;


    public void CreateLetterKeyboards(G4_SO_DataInfoLevel data)
    {
        letterKeyboards.Clear();

        foreach (var item in letterKeyboardsPool)
        {
            item.gameObject.SetActive(false);
        }


        for (int i = 0; i < data.so_given_letters.Length; i++)
        {
            G4_UILetterKeyboard item;
            if (i >= letterKeyboardsPool.Count)
            {
                item = Instantiate(letterKeyboardPrefabs, layoutGroup.transform);
                letterKeyboardsPool.Add(item);
            }
            else
            {
                item = letterKeyboardsPool[i];
                item.gameObject.SetActive(true);
            }
            item.Init(data.so_given_letters[i], this);
            letterKeyboards.Add(item);
        }
        ResizeButton();
    }


    private void ResizeButton()
    {
        var numberChild = letterKeyboards.Count;
        if (numberChild <= 0)
            return;

        var layoutSize = layoutGroup.rect().sizeDelta.x;
        var childSize = Mathf.Sqrt(2 * layoutSize * layoutSize * (1 - Mathf.Cos(2 * Mathf.PI / numberChild))) / (3 * Mathf.Sqrt(2));
        var radius = Mathf.Max((layoutSize * Mathf.Cos(Mathf.PI / numberChild)) / (1.7f * Mathf.Sqrt(2)), 180);

        layoutGroup.FixedSize = new Vector2(childSize, childSize);
        layoutGroup.radius = radius;

    }

    public void StartCreateAnswer(G4_UILetterKeyboard uiletter)
    {
        uiAnswer.Show();
        IsCreating = true;
        uiAnswer.AddAnswer(uiletter.letter.ToString());
        cur_letter_keyboard_list.Add(uiletter);
        cur_letter_index = 0;
        foreach (var item in letterKeyboards)
        {
            item.BindingCreatingAnswer();
        }
        DrawingLine();
    }

    public void AddLetterToAnswer(G4_UILetterKeyboard uiletter)
    {
        uiAnswer.AddAnswer(uiletter.letter.ToString());
        cur_letter_keyboard_list.Add(uiletter);
        cur_letter_index++;
        DrawingLine();
    }
    public void RemoveLetterFromAnswer(G4_UILetterKeyboard uiletter)
    {
        uiAnswer.RemoveAnswer(cur_letter_index);
        cur_letter_keyboard_list.Remove(uiletter);
        cur_letter_index--;
        DrawingLine();
    }

    public void RemoveAnswer()
    {
        StartCoroutine(IE_WaitForAnswerHide());
        cur_letter_keyboard_list.Clear();
        cur_letter_index = -1;
        foreach (var item in letterKeyboards)
        {
            item.RemoveBindingCreatingAnswer();
        }
        if(cor_drawing_line is not null)
        {
            StopCoroutine(cor_drawing_line);
            cor_drawing_line = null;
        }
    }

    private IEnumerator IE_WaitForAnswerHide()
    {
        yield return new WaitUntil(() => !uiAnswer.gameObject.activeInHierarchy);
        IsCreating = false;
    }

    public void DrawingLine()
    {
        if (cor_drawing_line is not null)
        {
            // Set end point
            if(cur_letter_index > 0)
            {
                var previous_key = cur_letter_keyboard_list[cur_letter_index - 1];
                var cur_key = cur_letter_keyboard_list[cur_letter_index];
                previous_key.LockDrawingLineToWorldPos(cur_key.transform.position, true);
            }
            StopCoroutine(cor_drawing_line);
            cor_drawing_line = null;
        }
        if(cur_letter_index < letterKeyboards.Count - 1)
            cor_drawing_line = StartCoroutine(IE_DrawingLine());
    }


    private IEnumerator IE_DrawingLine()
    {
        var cur_key = cur_letter_keyboard_list[cur_letter_index];
        while (true)
        {            
            cur_key.UpdateDrawingLine();
            yield return null;
        }
    }

    public void ReturnAnswer()
    {
        var list_answer = G4_UIGameplay.Instance.answers;
        var list_answered = G4_UIGameplay.Instance.answereds;
        var bonus_answer = G4_UIGameplay.Instance.bonusAnswers;
        var bonus_answered = G4_UIGameplay.Instance.bonusAnswereds;

        var txt_answer = uiAnswer.GetAnswer();

        if (list_answered.Contains(txt_answer))
        {
            //answered
            uiAnswer.Answered();
            G4_UIGameplay.Instance.ShowAnswered(txt_answer);
        }
        else if (list_answer.Contains(txt_answer))
        {
            G4_UIGameplay.Instance.CorrectTheAnswer(txt_answer);
            uiAnswer.AnswerTrue();
        }
        else if (bonus_answered.Contains(txt_answer))
        {
            //answered bonus
            uiAnswer.AnsweredBonus();
        }
        else if (bonus_answer.Contains(txt_answer))
        {
            G4_UIGameplay.Instance.CorrectBonusAnswer(txt_answer);
            uiAnswer.AnswerBonus();
        }
        else
        {
            //wrong
            uiAnswer.AnswerWrong();
        }
        RemoveAnswer();
    }
}
