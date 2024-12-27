using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class G1_UIPunctuation : G1_UICharacter
{
    public override void InitLetter(char letter, G1_UIWord word, G1_LetterStatus letterStatus)
    {
        txt_letter.text = letter.ToString();
        character = letter;
    }
}
