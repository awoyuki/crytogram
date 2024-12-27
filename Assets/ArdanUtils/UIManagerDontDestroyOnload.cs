using UnityEngine;


public class UIManagerDontDestroyOnload : SingletonBehivour<UIManagerDontDestroyOnload>
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}