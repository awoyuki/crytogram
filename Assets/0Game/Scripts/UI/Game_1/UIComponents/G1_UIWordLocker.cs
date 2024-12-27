using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class G1_UIWordLocker : MonoBehaviour
{
    [SerializeField] Image panel;
    [SerializeField] Image[] screws;
    [SerializeField] Image[] keys;

    public void Init(int lock_status)
    {
        if (lock_status == 0)
        {
            gameObject.SetActive(false);
            return;
        }

        if (lock_status == 4)
        {
            gameObject.SetActive(true);
            return;
        }

        for (int i = lock_status; i < 4; i++)
        {
            screws[screws.Length - 1 - i].gameObject.SetActive(false);
        }
    }

    public void UnlockWord(G1_UILetter uikey, int lock_index)
    {
        if (lock_index < 0 || lock_index >= 4)
            return;

        var screw = screws[screws.Length - 1 - lock_index];
        var key = keys[screws.Length - 1 - lock_index];
        key.gameObject.SetActive(true);
        key.transform.DOMove(screw.transform.position, 1.0f).From(uikey.key.transform.position).SetEase(Ease.InOutCubic).OnComplete(() =>
        {
            key.gameObject.SetActive(false);
            screw.transform.DOLocalRotate(new Vector3(0, 0, 360), 1.0f, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.InOutCubic).OnComplete(() =>
            {
                screw.transform.DOScale(1.1f, 0.5f).OnComplete(() =>
                {
                    screw.DOFade(0, 1.0f);
                    screw.rect().DOLocalMoveY(-25.0f, 1.0f).SetRelative(true).OnComplete(() =>
                    {
                        screw.gameObject.SetActive(false);
                        if (lock_index == 0)
                        {
                            panel.transform.DOScale(1.1f, 0.5f).OnComplete(() =>
                            {
                                panel.transform.DOMove(new Vector2(-1.5f, 2.5f), 1.0f).SetRelative(true);
                                panel.transform.DOLocalRotate(new Vector3(0, 0, 10.0f), 1.0f).SetRelative(true);
                                panel.DOFade(0, 1.0f);
                            });
                        }
                    });
                });
            });
        });
    }



}
