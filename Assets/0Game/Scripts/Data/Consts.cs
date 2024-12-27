using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Consts
{
    #region Common

    public static T[] ShuffleArray<T>(T[] array)
    {
        T[] new_array = new T[array.Length];
        array.CopyTo(new_array, 0);

        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            T temp = new_array[i];
            new_array[i] = new_array[j];
            new_array[j] = temp;
        }
        return new_array;
    }
    public static List<T> ShuffleList<T>(List<T> list)
    {
        List<T> new_list = new List<T>(list);
        for (int i = new_list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            T temp = new_list[i];
            new_list[i] = new_list[j];
            new_list[j] = temp;
        }
        return new_list;
    }

    public static string ShuffleString(this string str)
    {
        char[] array = str.ToCharArray();
        System.Random rng = new System.Random();
        int n = array.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            var value = array[k];
            array[k] = array[n];
            array[n] = value;
        }
        return new string(array);
    }

    /// <summary>
    /// Return random true false base on percent_true from 0 to 1
    /// </summary>
    /// <param name="percent_true"></param>
    /// <returns></returns>
    public static bool RandomBool(float percent_true)
    {
        return Random.value < percent_true;
    }

    public static string txt_coming_soon = "Coming Soon";



    #endregion

    #region Game 1

    public static char[] englishAlphabetArr = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

    public static List<G1_LetterStatus> EditableLetterStatus = new List<G1_LetterStatus>()
    { G1_LetterStatus.Process, G1_LetterStatus.Completed, G1_LetterStatus.Lock, G1_LetterStatus.DoubleLock, G1_LetterStatus.HasKey, G1_LetterStatus.HasCollectable};

    public static List<G1_LetterStatus> InteractableLetterStatus = new List<G1_LetterStatus>()
    { G1_LetterStatus.Process, G1_LetterStatus.HasKey, G1_LetterStatus.HasCollectable };

    #endregion

}
