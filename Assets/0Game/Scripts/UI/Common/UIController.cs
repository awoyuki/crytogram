using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    public UIHomepage uiHomepage;
    public UIGameplayMask uiGameplayMask;
    public UIWinGamePopup uiWinPopup;
    public UILoseGamePopup uiLosePopup;
    public Transform uiGameplayContainer;


    private void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
    }

}
