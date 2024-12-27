using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class G1_UIKeyPrefab : MonoBehaviour
{
    public ButtonEffectLogic btn;
    [SerializeField] Image sprite;
    public Text txt_Character;
    public Color txtColor;
    public Color sprColor;

    private G1_LetterCounter letterCounter;

    private void Awake()
    {
        btn.onClick.AddListener(OnButtonClick);
    }

    public void InitKey()
    {
        var letterGroups = G1_UIGameplay.Instance.uiSentence.characterCounterArr;
        foreach (var item in letterGroups)
        {
            if(item.character.ToString() == txt_Character.text)
            {
                letterCounter = item;
                return;
            }
        }
    }

    public List<G1_UILetter> GetUILetter()
    {
        return letterCounter.ui_letters;
    }


    public void OnButtonClick()
    {
        if (GameManager.Instance.GameState != GameState.InProgress)
            return;

        var cur_UILetter = G1_UIGameplay.Instance.uiKeyboard.curentUILetter;
        if (!Consts.InteractableLetterStatus.Contains(cur_UILetter.letter_status))
            return;

        var result = txt_Character.text.ToCharArray()[0] == cur_UILetter.character;
        cur_UILetter.AnswerLetterAnim(txt_Character.text, result);
        if (result)
        {
            G1_UIGameplay.Instance.TrueAnswer();
            CheckEffective();
        }
        else
        {
            G1_UIGameplay.Instance.WrongAnswer();
        }
    }
    public void CheckEffective()
    {
        var lettersGroup = letterCounter.ui_letters;
        foreach (var item in lettersGroup)
        {
            if (item.letter_status == G1_LetterStatus.Done)
            {
                DeEffective();
                return;
            }
            if (item.letter_status == G1_LetterStatus.Completed)
            {
                EffectiveExist();
                return;
            }
        }
        

    }
    public void DeEffective()
    {
        btn.interactable = false;
        btn.hasEffect = false;
        txt_Character.color = txtColor;
        sprite.color= sprColor;
    }
    public void InEffective()
    {
        btn.interactable = true;
        btn.hasEffect = true;
        txt_Character.color = Color.black;
        sprite.color = Color.white;
    }
    public void EffectiveExist()
    {
        btn.interactable = true;
        txt_Character.color = Color.black;
        sprite.color = Color.green;
    }
}
