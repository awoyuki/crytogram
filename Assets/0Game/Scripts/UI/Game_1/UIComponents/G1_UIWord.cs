using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class G1_UIWord : MonoBehaviour
{
    [SerializeField] G1_UILetter letter_prefab;
    [SerializeField] G1_UIPunctuation punctuation_prefab;
    [SerializeField] LayoutGroup layoutGroup;
    [SerializeField] G1_UIWordLocker LockPanel;

    public List<G1_UILetter> list_letter = new List<G1_UILetter>();
    public List<G1_UIPunctuation> list_punctuation = new List<G1_UIPunctuation>();

    public int isLocked { get; private set; }

    public void InitWord(G2_WordCrypto word)
    {
        ClearWord();
        var letter_array = word.character_array;
        for (int i = 0; i < letter_array.Length; i++)
        {
            var character = letter_array[i];
            if (char.IsPunctuation(letter_array[i].character))
            {
                var letter = Instantiate(punctuation_prefab, transform);
                letter.InitLetter(character.character, this, character.status);
                list_punctuation.Add(letter);
            }
            else
            {
                var letter = Instantiate(letter_prefab, transform);
                letter.InitLetter(character.character, this, character.status);
                list_letter.Add(letter);
            }
        }
        LockPanel.transform.SetAsLastSibling();
        InitStatus(word.isLocked);
    }

    public void InitStatus(int locked_status)
    {
        isLocked = locked_status;
        LockPanel.Init(locked_status);
    }

    public void ChangeStatus(G1_UILetter key)
    {
        isLocked--;
        LockPanel.UnlockWord(key, isLocked);
    }

    private void ClearWord()
    {
        foreach (G1_UICharacter item in list_letter)
        {
            DestroyImmediate(item);
        }
        list_letter.Clear();
    }

    public G1_UILetter FirstLetter => list_letter[0];
    public G1_UILetter LastLetter => list_letter[list_letter.Count - 1];

    public void CheckLockPerWord() 
    {
        if (FirstLetter.letter_status == G1_LetterStatus.DoubleLock)
            FirstLetter.letter_status = G1_LetterStatus.Lock;

        if (LastLetter.letter_status == G1_LetterStatus.DoubleLock)
            LastLetter.letter_status = G1_LetterStatus.Lock;
    }
}
