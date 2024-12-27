using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    public Transform main;

    public virtual void Show(object data)
    {
        main.localScale = Vector3.one * 0.2f;
        main.DOScale(1, 0.2f);
        gameObject.SetActive(true);
        //  SoundControl.Instance.PlayShot(SoundControl.Instance.click);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
     //   SoundControl.Instance.PlayShot(SoundControl.Instance.click);
    }
}
