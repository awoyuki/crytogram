using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Linq;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class G2_UiDelete : MonoBehaviour
{
    [SerializeField] ButtonEffectLogic btn;
    private void Start()
    {
        btn.onClick.AddListener(DeleteCharacter);
    }
    public void DeleteCharacter()
    {
        var cur_UILetter = G2_UIGameplay.Instance.uiKeyboard.curentUILetter;
        if(cur_UILetter.letter_status != G2_LetterStatus.Completed)
        {
            G2_UIGameplay.Instance.onChangeLetterStatus?.Invoke(G2_LetterStatus.Process, cur_UILetter);
        }
        cur_UILetter.word?.ChangeStatus(G2_UIWord.WordStatus.Selected);
        //G2_DataController.instance.LevelProcess = G2_UIController.Instance.uiGameplay.uiSentence.GetCryptoProcess();
    }
}
