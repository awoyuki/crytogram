using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class G1_CharacterCrypto
{
    public char character;
    public G1_LetterStatus status;

    public G1_CharacterCrypto(char character, G1_LetterStatus status)
    {
        this.character = character;
        this.status = status;
    }
}

[Serializable]
public class G2_WordCrypto
{
    public G1_CharacterCrypto[] character_array;
    public int isLocked;

    public G2_WordCrypto(G1_CharacterCrypto[] character_array)
    {
        this.character_array = character_array;
        isLocked = 0;
    }

}


[CreateAssetMenu(fileName = "G1_SO_DataInfoLevel", menuName = "ScriptableObject/DataInfoLevel/G1_SO_DataInfoLevel")]
public class G1_SO_DataInfoLevel : ScriptableObject
{
    public int so_index;
    public G2_WordCrypto[] word_cryptos_array;
    public string so_authorName;
    public string so_describe;
    public G1_CharacterCrypto[] CharacterArray
    {
        get
        {
            List<G1_CharacterCrypto> result = new List<G1_CharacterCrypto>();
            foreach (var item in word_cryptos_array)
            {
                result.AddRange(item.character_array);
            }
            return result.ToArray();
        }
        
    }

    public string CrytoProgress
    {
        get
        {
            var cryto_string = "";
            foreach (var word in word_cryptos_array)
            {
                foreach (var letter in word.character_array)
                {
                    if (char.IsLetter(letter.character))
                        cryto_string += (int)letter.status;
                }
            }
            return cryto_string;
        }
    }

    public string WordsStatus
    {
        get
        {
            var status = "";
            foreach (var item in word_cryptos_array)
            {
                status += item.isLocked;
            }
            return status;
        }
    }

    public int MaxKey
    {
        get
        {
            int count = 0;
            foreach (var word in word_cryptos_array)
            {
                foreach (var letter in word.character_array)
                {
                    if (letter.status == G1_LetterStatus.HasKey)
                        count++;
                }
            }
            return count;
        }
    }

    public int MaxLock
    {
        get
        {
            int count = 0;
            foreach (var item in word_cryptos_array)
            {
                if (item.isLocked > 0)
                    count++;
            }
            return count;
        }
    }
}