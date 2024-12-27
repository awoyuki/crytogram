using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;
using static Events;

public abstract class BaseLevelManager : MonoBehaviour
{
    private static BaseLevelManager _instance;
    public static BaseLevelManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<BaseLevelManager>();
            }
            return _instance;
        }
    }

    private void OnDestroy()
    {
        _instance = null;
    }

    public void StartLevel(int index)
    {
        GameManager.Instance.ChangeGameState(GameState.InProgress);
        UIController.Instance.uiGameplayMask.UpdateLevelCounter(index);
        var so_index = OnStartLevel(index);
        DataController.instance.CurrentLevelID = so_index;
        onStartLevel?.Invoke(so_index);
    }
    /// <summary>
    ///  Return so_index of data level
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    protected abstract int OnStartLevel(int index);


    public void LoseLevel()
    {
        GameManager.Instance.ChangeGameState(GameState.Lose);
        onEndLevel?.Invoke(false);
        StartCoroutine(IE_LoseLevel());
    }

    IEnumerator IE_LoseLevel()
    {
        OnLoseLevel();
        yield return Yielder.Get(0.5f);
        UIController.Instance.uiLosePopup.Show();
    }

    public void WinLevel()
    {
        GameManager.Instance.ChangeGameState(GameState.Win);
        ResetLevel();
        onEndLevel?.Invoke(true);
        DataController.instance.CurrentLevelCount++;
        StartCoroutine(IE_WinLevel());
    }

    IEnumerator IE_WinLevel()
    {
        OnWinLevel();
        yield return Yielder.Get(1.0f);
        UIController.Instance.uiWinPopup.Show();
    }

    protected virtual void OnWinLevel()
    {

    }
    protected virtual void OnLoseLevel()
    {

    }


    public virtual void ResetLevel()
    {
    }

    public virtual void Revive()
    {
        GameManager.Instance.ChangeGameState(GameState.InProgress);
    }

    public abstract bool DoesLevelStillOnProcess(int level_id);

    public void StartCurrentLevel()
    {
        var cur_level = DataController.instance.CurrentLevelCount;
        if (cur_level >= DataController.instance.TotalLevel)
        {
            UIController.Instance.uiGameplayMask.BackToHome();
            return;
        }
        StartLevel(cur_level);
    }
}
