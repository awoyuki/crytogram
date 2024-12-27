using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILoseGamePopup : UIPanel
{
    [SerializeField] ButtonEffectLogic btn_continue , btn_home;
    protected override void Awake()
    {
        base.Awake();
        btn_continue.onClick.AddListener(OnClickContinueBtn);
        btn_home.onClick.AddListener(OnClickHomeBtn);
    }

    private void OnClickContinueBtn()
    {
        Hide();
        G1_LevelManager.Instance.Revive();
    }
    private void OnClickHomeBtn()
    {
        Hide();
        G1_LevelManager.Instance.ResetLevel();
        UIController.Instance.uiGameplayMask.BackToHome();
    }
}
