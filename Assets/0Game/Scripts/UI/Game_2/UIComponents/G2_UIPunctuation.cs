using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class G2_UIPunctuation : G2_UICharacter
{
    public override void InitLetter(char letter, G2_UIWord word, G2_LetterStatus letterStatus)
    {
        txt_letter.text = letter.ToString();
        character = letter;
    }
}
