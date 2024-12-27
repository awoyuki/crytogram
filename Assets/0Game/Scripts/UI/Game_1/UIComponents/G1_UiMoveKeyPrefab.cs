using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Linq;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class G1_UiMoveKeyPrefab : MonoBehaviour
{
    public bool isLeft;
    public G1_UISentence uiSentence;
    [SerializeField] ButtonEffectLogic btn;
    private void Start()
    {
        btn.onClick.AddListener(SelectNextCharacter);
    }
    public void SelectNextCharacter()
    {
        if (GameManager.Instance.GameState != GameState.InProgress)
            return;

        var listChar = uiSentence.list_letter_uncompleted;

        if (listChar.Count <= 0)
            return;

        int selectedIndex = listChar.IndexOf(G1_UIGameplay.Instance.uiKeyboard.curentUILetter);
        int direction = isLeft ? -1 : 1;

        for (int i = (selectedIndex + direction + listChar.Count) % listChar.Count; i != selectedIndex; i = (i + direction + listChar.Count) % listChar.Count)
        {
            var currentChar = listChar[i];
            if (char.IsLetter(currentChar.character) && Consts.InteractableLetterStatus.Contains(currentChar.letter_status) && currentChar.word.isLocked <= 0)
            {
                currentChar.OnClickBtn();
                G1_UIGameplay.Instance.uiSentence.ScrollTo(currentChar.word.rect());
                return;
            }
        }

    }
}
