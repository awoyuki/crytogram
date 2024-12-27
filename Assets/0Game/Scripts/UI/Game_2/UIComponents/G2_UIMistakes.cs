using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class G2_UIMistakes : MonoBehaviour
{
    [SerializeField] Image[] dots;
    [SerializeField] Color def_color;
    [SerializeField] Color check_color;

    public void UpdateMistaken(int count)
    {
        var tmp = 0;
        foreach (var item in dots)
        {
            item.color = def_color;
            if(tmp < count)
                item.color = check_color;
            tmp++;
        }

    }

}
