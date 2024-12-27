using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SRF.UI.Layout;
using UnityEngine.UI;

public class G2_UIHint : UIPanel
{
    [SerializeField] FlowLayoutGroup flowLayout;
    [SerializeField] Transform BottomLayout;
    public Text txt_hint, txt_hint_iap;

    private List<G2_UILetter> uILetters = new List<G2_UILetter>();


    private void OnEnable()
    {
        UpdateHintUI();
    }

    public void UpdateHintUI()
    {
        txt_hint.text = G2_UIGameplay.Instance.DataContainer.CurrentHint.ToString();
    }

    public void HintHandle()
    {
        if (G2_UIGameplay.Instance.DataContainer.CurrentHint > 0)
            G2_UIGameplay.Instance.EnableHint();
        else
        {
            //show ads
        }
    }

    public void HintIAPHandle()
    {
        G2_UIGameplay.Instance.DataContainer.CurrentHint += 20;
        UpdateHintUI();
    }

    public override void Show(object data = null)
    {
        base.Show(data);
        var uiGameplay = G2_UIGameplay.Instance;
        uiGameplay.btn_hint.transform.SetParent(transform);
        flowLayout.enabled = false;
        foreach (var item in uiGameplay.uiSentence.list_word)
        {
            item.EnableLayoutGroup(false);
        }
        
        foreach (var item in uiGameplay.uiSentence.list_letter_uncompleted)
        {
            if(item.letter_status == G2_LetterStatus.Process)
            {
                uILetters.Add(item);
                item.ActiveHint(transform);
            }
        }
        
    }

    public override void Hide()
    {
        flowLayout.enabled = true;
        var uiGameplay = G2_UIGameplay.Instance;
        uiGameplay.btn_hint.transform.SetParent(transform);
        foreach (var item in uiGameplay.uiSentence.list_word)
        {
            item.EnableLayoutGroup(true);
        }

        foreach (var item in uILetters)
        {
            item.ResetPosition();
        }
        uILetters.Clear();
        uiGameplay.btn_hint.transform.SetParent(BottomLayout);
        UpdateHintUI();
        base.Hide();
    }
}
