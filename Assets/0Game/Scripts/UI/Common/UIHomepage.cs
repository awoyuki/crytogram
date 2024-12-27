using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHomepage : UIPanel
{
    [SerializeField] UIGameTypeButton[] gameTypeButtons;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        for (int i = 0; i < gameTypeButtons.Length; i++)
        {
            gameTypeButtons[i].InitData(i);
        }

    }

    public override void Show(object data = null)
    {
        base.Show(data);
        for (int i = 0; i < gameTypeButtons.Length; i++)
        {
            gameTypeButtons[i].ReloadInfo();
        }
    }


}
