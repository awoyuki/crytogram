using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class G7_Loading : MonoBehaviour
{
    public CanvasGroup canvas_group_panel;
    public ButtonEffectLogic button_next;
    public ButtonEffectLogic button_home;

    private float fade_duration = 0.5f;

    private void Start()
    {
        button_next.onClick.AddListener(Button_Next_Click);
    }

    public void Show(Action onShowDone = null)
    {
        gameObject.SetActive(true);
        canvas_group_panel.interactable = false;
        canvas_group_panel.DOFade(1, fade_duration).From(0).OnComplete(() =>
        {
            canvas_group_panel.interactable = true;
            onShowDone?.Invoke();
        });
    }

    private void Button_Next_Click()
    {
        G7_UIGamePlay.Instance.LoadNextLevel();
        canvas_group_panel.interactable = false;
        canvas_group_panel.DOFade(0, fade_duration).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
