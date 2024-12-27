using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class G4_UIGameplay : MonoBehaviour
{
    private static G4_UIGameplay instance;
    public static G4_UIGameplay Instance
    {
        get
        {
            if (instance is null)
            {
                instance = FindObjectOfType<G4_UIGameplay>();
            }
            return instance;
        }
    }

    private G4_DataContainer dataContainer; 

    public G4_DataContainer DataContainer
    {
        get
        {
            if (dataContainer is null)
            {
                dataContainer = new G4_DataContainer();
            }
            return dataContainer;
        }
    }

    public G4_UIKeyboard keyboard;
    public GridLayoutGroup layoutGroup;

    [SerializeField] G4_UILetter UILetterPrefab;
    [SerializeField] ButtonEffectLogic btn_hint, btn_hint_iap;
    [SerializeField] Text txt_hint, txt_bonus;
    [SerializeField] public Image img_bonus;

    private List<G4_UILetter> uiLetters = new List<G4_UILetter>();
    private List<G4_UILetter> activeuiLetters = new List<G4_UILetter>();
    private RectTransform rectTransform;

    public List<string> answers = new List<string>();
    public List<string> answereds = new List<string>();
    public List<string> bonusAnswers = new List<string>();
    public List<string> bonusAnswereds = new List<string>();

    private int total_row = 0;
    private int total_col = 0;

    protected void Awake()
    {
        rectTransform = layoutGroup.rect();
        btn_hint.onClick.AddListener(HintHandle);
        btn_hint_iap.onClick.AddListener(HintIAPHandle);
        UpdateHintUI();
    }

    private void OnDestroy()
    {
        instance = null;
    }

    private void ClearUILetterList()
    {
        foreach (var item in uiLetters)
        {
            item.gameObject.SetActive(false);
        }
    }


    public void GenerateLevel(G4_SO_DataInfoLevel data)
    {
        total_row = data.so_row;
        total_col = data.so_column;
        var cur_word_data = data.so_word_data;

        layoutGroup.enabled = true;
        layoutGroup.constraintCount = total_col;

        activeuiLetters.Clear();
        ClearUILetterList();

        float maxSize = 210.0f;
        float size = Mathf.Min((rectTransform.rect.width - (layoutGroup.padding.left * 2)) / total_col, (rectTransform.rect.height - 50) / total_row);
        size = Mathf.Min(size, maxSize);

        layoutGroup.cellSize = new Vector2(size, size);

        //Generate Answers
        answers = new List<string>(data.so_answer);
        bonusAnswers = new List<string>(data.so_bonus_word);
        answereds.Clear();
        bonusAnswereds.Clear();

        //Create base panel
        for (int i = 0; i < total_row; i++)
        {
            for (int j = 0; j < total_col; j++)
            {
                G4_UILetter uiLetter;

                if (i * total_col + j >= uiLetters.Count)
                {
                    uiLetter = Instantiate(UILetterPrefab, layoutGroup.transform);
                    uiLetters.Add(uiLetter);
                }
                else
                {
                    uiLetter = uiLetters[i * total_col + j];
                    uiLetter.gameObject.SetActive(true);
                }

                var letterData = cur_word_data[i, j];
                if (letterData is null || string.IsNullOrEmpty(letterData.letter))
                    uiLetter.DeactiveLetter();
                else
                {
                    uiLetter.ActiveLetter(letterData);
                    activeuiLetters.Add(uiLetter);
                }
            }
        }

        //Init Keyboard
        keyboard.CreateLetterKeyboards(data);
        FillWordProcessData();
        FillProcessData();

        if (data.so_bonus_word is not null && data.so_bonus_word.Count > 0)
            FillWordBonusProcessData();
    }

    private void FillWordProcessData()
    {
        var word_proc = DataContainer.LevelWordProcess;
        StringBuilder word_process = new StringBuilder(word_proc);
        if (word_process.Length > 0)
        {
            for (int i = 0; i < answers.Count; i++)
            {
                if (i >= answers.Count)
                    break;
                if (word_process[i] == '1')
                    answereds.Add(answers[i]);
            }
        }
        else
        {
            word_process.Append('0', answers.Count);
            DataContainer.LevelWordProcess = word_process.ToString();
        }
    }
    private void FillWordBonusProcessData()
    {
        var word_proc = DataContainer.LevelWordBonusProcess;
        StringBuilder word_process = new StringBuilder(word_proc);
        if (word_process.Length > 0)
        {
            for (int i = 0; i < bonusAnswers.Count; i++)
            {
                if (i >= bonusAnswers.Count)
                    break;
                if (word_process[i] == '1')
                    bonusAnswereds.Add(bonusAnswers[i]);
            }
            txt_bonus.text = bonusAnswereds.Count.ToString();
            img_bonus.gameObject.SetActive(true);
        }
        else
        {
            word_process.Append('0', bonusAnswers.Count);
            DataContainer.LevelWordBonusProcess = word_process.ToString();
            txt_bonus.text = "0";
            img_bonus.gameObject.SetActive(false);
        }
    }

    private void FillProcessData()
    {
        var proc = DataContainer.LevelProcess;
        StringBuilder process = new StringBuilder(proc);
        if(process.Length > 0)
        {
            var strings = proc.Split("_");
            var str_count = strings.Length;
            for (int i = 0; i < str_count; i++)
            {
                var str_length = strings[i].Length;
                for (int j = 0; j < str_length; j++)
                {
                    var uiLetter = uiLetters[i * str_length + j];
                    uiLetter.status = (strings[i][j] == '1');
                    if (uiLetter.status && activeuiLetters.Contains(uiLetter))
                    {
                        uiLetter.ShowLetter();
                    }
                }
            }
        }
        else
        {
            string[] array_str = new string[total_row];
            for (int i = 0; i < total_row; i++)
            {
                StringBuilder child = new StringBuilder("");
                array_str[i] = child.Append('0', total_col).ToString();
            }
            process.AppendJoin('_', array_str);
            DataContainer.LevelProcess = process.ToString();
        }
    }


    public void CorrectTheAnswer(string answer)
    {
        var id = answers.IndexOf(answer);
        var proc = DataContainer.LevelProcess;
        StringBuilder process = new StringBuilder(proc);
        List<G4_UILetter> letter_list_anim = new List<G4_UILetter>();
        foreach (var item in activeuiLetters)
        {
            if (item.word_ids.Contains(id))
            {
                item.status = true;
                letter_list_anim.Add(item);
                var str_index = SaveLetterData(item);
                process[str_index] = '1';
            }
        }
        keyboard.uiAnswer.PlayAnswerAnim(letter_list_anim);
        answereds.Add(answer);
        DataContainer.LevelProcess = process.ToString();
        SaveWordData(id);
        CheckWin();
    }

    public void ShowAnswered(string answer)
    {
        var id = answers.IndexOf(answer);
        List<G4_UILetter> letter_list_anim = new List<G4_UILetter>();
        foreach (var item in activeuiLetters)
        {
            if (item.word_ids.Contains(id))
            {
                letter_list_anim.Add(item);
            }
        }
        keyboard.uiAnswer.PlayAnsweredAnim(letter_list_anim);
    }

    public void CorrectBonusAnswer(string answer)
    {
        var id = bonusAnswers.IndexOf(answer);
        var proc = DataContainer.LevelWordBonusProcess;
        StringBuilder process = new StringBuilder(proc);
        process[id] = '1';
        DataContainer.LevelWordBonusProcess = process.ToString();
        bonusAnswereds.Add(answer);
        txt_bonus.text = bonusAnswereds.Count.ToString();
        if(!img_bonus.gameObject.activeInHierarchy)
            img_bonus.gameObject.SetActive(true);
        keyboard.uiAnswer.PlayBonusAnim();
    }

    private void CheckWin()
    {
        if (answereds.Count >= answers.Count)
        {
            BaseLevelManager.Instance.WinLevel();
        }
    }

    private void SaveWordData(int index)
    {
        var word_proc = DataContainer.LevelWordProcess;
        StringBuilder word_process = new StringBuilder(word_proc);
        word_process[index] = '1';
        DataContainer.LevelWordProcess = word_process.ToString();
    }

    private int SaveLetterData(G4_UILetter letter)
    {
        return total_col * letter.index_row + letter.index_col + letter.index_row;
    }


    private void HintHandle()
    {
        if (GameManager.Instance.GameState != GameState.InProgress || DataContainer.CurrentHint <= 0)
            return;


        List<G4_UILetter> remainLetter = new List<G4_UILetter>();
        foreach (var item in activeuiLetters)
        {
            if (!item.status)
                remainLetter.Add(item);
        }

        if (remainLetter.Count <= 0)
            return;

        var proc = DataContainer.LevelProcess;
        StringBuilder process = new StringBuilder(proc);
        var index_rad = UnityEngine.Random.Range(0, remainLetter.Count);
        var letter = remainLetter[index_rad];
        letter.ShowLetter();
        var str_index = SaveLetterData(letter);
        process[str_index] = '1';
        DataContainer.LevelProcess = process.ToString();
        DataContainer.CurrentHint--;
        UpdateHintUI();

        remainLetter.Remove(letter);
        for (int i = 0; i < answers.Count; i++)
        {
            var word_checking = answers[i];
            if (answereds.Contains(word_checking))
                continue;

            if (letter.word_ids.Contains(i))
            {
                bool isCompleted = true;
                foreach(var letter_check in remainLetter)
                {
                    if (!letter_check.status && letter_check.word_ids.Contains(i))
                    {
                        isCompleted = false;
                        break;
                    }
                }
                if (isCompleted)
                {
                    answereds.Add(word_checking);
                    SaveWordData(i);
                }
            }
        }
        CheckWin();
    }


    private void HintIAPHandle()
    {
        if (GameManager.Instance.GameState != GameState.InProgress)
            return;

        DataContainer.CurrentHint += 20;
        UpdateHintUI();
    }

    public void UpdateHintUI()
    {
        txt_hint.text = DataContainer.CurrentHint.ToString();
    }

}
