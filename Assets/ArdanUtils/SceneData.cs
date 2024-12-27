using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneData : SingletonBehivour<SceneData>
{
    public GameObject content;
    public Text txtLoading;

    private IEnumerator Start()
    {
        DontDestroyOnLoad(gameObject);
        yield return new WaitForSeconds(3);
        LoadScene(SceneEnum.Home);
    }

    [EventAttribute(EventKey.LoadScene)]
    void LoadScene(SceneEnum s)
    {
        StartCoroutine(IELoadscene((int) s));
    }

    IEnumerator IELoadscene(int index)
    {
        content.SetActive(true);
        var sync = SceneManager.LoadSceneAsync(index);
        sync.allowSceneActivation = false;
        while (!sync.isDone)
        {
            if (sync.progress >= 0.9f)
            {
                Debug.Log("Sync >= 0.9f");
                break;
            }

            txtLoading.text = "Loading " + (int)((sync.progress) * 100f) + "%";
            yield return null;
        }

        txtLoading.text = "Loading 100%";
        yield return null;
        content.SetActive(false);
        sync.allowSceneActivation = true;
    }
}

#if UNITY_EDITOR
public static class SceneExtension
{
    [MenuItem("Tool/Scene/Splash &,")]
    public static void OpenSplash()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/_GAME/Scene/splash.unity");
    }
    [MenuItem("Tool/Scene/Home &.")]
    public static void OpenHome()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/_GAME/Scene/home.unity");
    }
    [MenuItem("Tool/Scene/Ingame &/")]
    public static void OpenIngame()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/_GAME/Scene/ingame.unity");
    }
    [MenuItem("Tool/Scene/Play &;")]
    public static void Play()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/_GAME/Scene/splash.unity");
        EditorApplication.isPlaying = true;
    }
}
#endif
