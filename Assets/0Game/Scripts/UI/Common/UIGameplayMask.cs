using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameplayMask : UIPanel
{
    [SerializeField] ButtonEffectLogic btnBackToHome;
    [SerializeField] Text txt_level;

    protected override void Awake()
    {
        base.Awake();
        btnBackToHome.onClick.AddListener(BackToHome);
    }

    public void BackToHome()
    {
        if (GameManager.Instance.GameState == GameState.Hint)
            return;

        GameManager.Instance.ExitGameMode();
        UIController.Instance.uiHomepage.Show();
        btnBackToHome.gameObject.SetActive(false);
        txt_level.gameObject.SetActive(false);
    }

    public void OnEnterGameMode()
    {
        UIController.Instance.uiHomepage.Hide();
        btnBackToHome.gameObject.SetActive(true);
        txt_level.gameObject.SetActive(true);
    }

    public void UpdateLevelCounter(int cur_level)
    {
        txt_level.text = ("Level {0}").Replace("{0}", (cur_level + 1).ToString());
    }
}
