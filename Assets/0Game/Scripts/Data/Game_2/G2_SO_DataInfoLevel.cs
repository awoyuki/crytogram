using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "G2_SO_DataInfoLevel", menuName = "ScriptableObject/DataInfoLevel/G2_SO_DataInfoLevel")]
public class G2_SO_DataInfoLevel : ScriptableObject
{
    public int so_index;
    public string so_sentence = "";
    public string so_status_crypto_en = "";
    public string so_status_text_en = "";
    //public string so_keyboard_status_en;
    public List<SO_DataHintWord> dataHintWord;
}
[System.Serializable]
public class SO_DataHintWord
{
    public string so_describe = "";
    public string so_letter = "";

    public SO_DataHintWord(string so_describe, string so_letter)
    {
        this.so_describe = so_describe;
        this.so_letter = so_letter;
    }

    public SO_DataHintWord()
    {
    }
}
