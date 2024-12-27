using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReOrderUI : MonoBehaviour
{
    [SerializeField] List<MaskableGraphic> graphics = new List<MaskableGraphic>();
    [SerializeField] Material custom_mat;

    private void Awake()
    {
        Events.onGameStateChange += OnGameStateChangeEvent;
    }

    private void OnDestroy()
    {
        Events.onGameStateChange -= OnGameStateChangeEvent;
    }

    private void OnGameStateChangeEvent(GameState game_state)
    {
        if (game_state == GameState.Hint)
            ReorderToFront();
        else
            ResetOrder();
    }


#if UNITY_EDITOR
    [Button]
    private void FindMaterials()
    {
        graphics.Clear();
        var rendered = GetComponentsInChildren<MaskableGraphic>();
        graphics.AddRange(rendered);
        UnityEditor.EditorUtility.SetDirty(gameObject);
    }
#endif

    [Button]
    public void ReorderToFront()
    {
        foreach (var item in graphics)
        {
            item.material = custom_mat;
        }
    }

    [Button]
    public void ResetOrder()
    {
        foreach (var item in graphics)
        {
            item.material = null;
        }
    }

}
