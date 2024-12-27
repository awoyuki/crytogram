using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class G2_UIKeyPrefab : MonoBehaviour
{
    public ButtonEffectLogic btn;
    [SerializeField] Image sprite;
    public Text txt_Character;

    private G2_UISentence.LetterCounter letterCounter;

    private void Awake()
    {
        btn.onClick.AddListener(OnButtonClick);
    }

    public void InitKey()
    {
        var letterGroups = G2_UIGameplay.Instance.uiSentence.characterCounterArr;
        foreach (var item in letterGroups)
        {
            if(item.character.ToString() == txt_Character.text)
            {
                letterCounter = item;
                return;
            }
        }
    }

    public List<G2_UILetter> GetUILetter()
    {
        return letterCounter.ui_letters;
    }


    public void OnButtonClick()
    {
        if (GameManager.Instance.GameState != GameState.InProgress)
            return;

        var cur_UILetter = G2_UIGameplay.Instance.uiKeyboard.curentUILetter;
        G2_UIGameplay.Instance.onChangeLetterStatus?.Invoke(G2_LetterStatus.Activate, cur_UILetter);
        G2_UIGameplay.Instance.onChangeLetterText?.Invoke(txt_Character.text, cur_UILetter);
        
        bool allCompleted = true;
        foreach (var item in cur_UILetter.word.list_letter)
        {
            if (item.letter_status != G2_LetterStatus.Completed)
            {
                allCompleted = false;
                break;
            }
        }
        if (allCompleted)
        {
            List<G2_UILetter> lettersToComplete = new List<G2_UILetter>();
            foreach (var uILetter in cur_UILetter.word.list_letter)
            {
                foreach (var uI_sentence_Letter in G2_UIGameplay.Instance.uiSentence.list_letter_uncompleted)
                {
                    if (uI_sentence_Letter.character == uILetter.character)
                    {
                        lettersToComplete.Add(uI_sentence_Letter);
                    }
                }
            }
            foreach (var letter in lettersToComplete)
            {
                letter.ChangeStatus(G2_LetterStatus.Completed);
            }
        }
        var uiGameplay = G2_UIGameplay.Instance;
        uiGameplay.uiSentence.CheckLetterCompleted();
        G2_UIGameplay.Instance.uiSentence.SelectNextLetter();
    }
    //public void DeEffective()
    //{
    //    btn.interactable = false;
    //    btn.hasEffect = false;
    //    txt_Character.color = txtColor;
    //    sprite.color= sprColor;
    //}
    //public void InEffective()
    //{
    //    btn.interactable = true;
    //    btn.hasEffect = true;
    //    txt_Character.color = Color.black;
    //    sprite.color = Color.white;
    //}
    //public void EffectiveExist()
    //{
    //    btn.interactable = true;
    //    txt_Character.color = Color.black;
    //    sprite.color = Color.green;
    //}
}
