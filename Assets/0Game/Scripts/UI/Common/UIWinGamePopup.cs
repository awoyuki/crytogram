using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWinGamePopup : UIPanel
{
    [SerializeField] ButtonEffectLogic btn_continue;

    protected override void Awake()
    {
        base.Awake();
        btn_continue.onClick.AddListener(OnClickContinueBtn);
        ;
    }

    public void LoadInformation(bool win)
    {
        Debug.Log("Load");
    }

    private void OnClickContinueBtn()
    {
        Hide();
        BaseLevelManager.Instance.StartCurrentLevel();
    }
}
