using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanel : MonoBehaviour
{
    public Transform _main;
    public Transform _mainCenter;
    public Button _btnClose;

    public bool IsShowing { get; private set; }

    protected virtual void Awake()
    {
        if (_btnClose != null)
        {
            _btnClose.onClick.AddListener(BtnCloseClick);
        }
        IsShowing = gameObject.activeInHierarchy;
        //_animator = GetComponent<Animator>();
    }
    public virtual void Init()
    {

    }
    public virtual void Show(object data = null)
    {
        gameObject.SetActive(true);
        IsShowing = true;
    }

    protected virtual void BtnCloseClick()
    {
        if (GameManager.Instance.GameState == GameState.Hint)
            return;
        Hide();
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
        IsShowing = false;
    }

}
