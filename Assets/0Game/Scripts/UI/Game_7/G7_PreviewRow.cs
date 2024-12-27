using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class G7_PreviewRow : MonoBehaviour
{
    [Header("DEV_INIT")] 
    public List<Image> list_pixel;

    public void Restart()
    {
        foreach (var pixel in list_pixel)
        {
            pixel.color = Color.white;
        }
    }
}
