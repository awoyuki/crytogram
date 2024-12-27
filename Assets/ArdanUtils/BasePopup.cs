using System;
using DG.Tweening;
using UnityEngine;

public class BasePopupData<T> where T : BasePopup<T>
{
    public virtual void Show()
    {
    }
}
public class BasePopup<T> : PopupSingleton<T> where T : EventBehaviour
{
    public Transform main;
    public ButtonEffectLogic btnClose;

    public virtual T Show()
    {
        gameObject.SetActive(true);
        var rect = GetComponent<RectTransform>();
        rect.offsetMax = Vector2.zero;
        rect.offsetMin = Vector2.zero;
        main.localScale = Vector3.one * 0.67f;
        main.DOScale(1, 0.33f);
        return this as T;
    }

    public virtual void Hide()
    {
        Debug.Log("Back " + gameObject.name);
        gameObject.SetActive(false);
    }

    private void Awake()
    {
        if (btnClose)
            btnClose.onDown.AddListener(Hide);
    }
}