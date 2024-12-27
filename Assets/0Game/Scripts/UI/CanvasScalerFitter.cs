using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScalerFitter : MonoBehaviour
{
    void Start()
    {
        var canvasScaler = GetComponent<CanvasScaler>();

        // var ratio = (float)Screen.height / (float)Screen.width;
        // canvasScaler.matchWidthOrHeight = ratio >= 1.78f ? 1 : 0;
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = (float)720 / (float)1280;

        canvasScaler.matchWidthOrHeight = (screenRatio >= targetRatio) ? 1 : 0;
    }
}
