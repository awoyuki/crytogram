using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class G1_UIHint : UIPanel
{
    [SerializeField] ReOrderUI[] hint_reorders;
    public Text txt_hint, txt_hint_iap;


    public void UpdateHintUI()
    {
        txt_hint.text = G1_UIGameplay.Instance.DataContainer.CurrentHint.ToString();
    }

    public void HintHandle()
    {
        if (G1_UIGameplay.Instance.DataContainer.CurrentHint > 0)
            G1_UIGameplay.Instance.EnableHint(!IsShowing);
        else
        {
            //show ads
        }
    }

    public void HintIAPHandle()
    {
        G1_UIGameplay.Instance.DataContainer.CurrentHint += 20;
        UpdateHintUI();
    }

    public override void Show(object data = null)
    {
        base.Show(data);
        var uiGameplay = G1_UIGameplay.Instance;
        foreach (var item in uiGameplay.uiSentence.list_letter_uncompleted)
        {
            item.ActiveHint();
        }
        
    }

    public override void Hide()
    {
        var uiGameplay = G1_UIGameplay.Instance;
        foreach (var item in uiGameplay.uiSentence.list_letter_uncompleted)
        {
            item.ResetHint();
        }
        UpdateHintUI();
        base.Hide();
    }
}
