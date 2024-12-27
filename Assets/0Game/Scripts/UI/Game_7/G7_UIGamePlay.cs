using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class G7_UIGamePlay : MonoBehaviour
{
    public static G7_UIGamePlay Instance;
    
    [Header("DEV_INIT")] 
    [SerializeField] private ButtonEffectLogic btn_hint;
    [SerializeField] private ButtonEffectLogic btn_check;
    [SerializeField] private ButtonEffectLogic btn_hint_info;
    [SerializeField] private ButtonEffectLogic btn_check_info;
    [SerializeField] private GameObject ban_touch;
    [Space] 
    [SerializeField] private Canvas canvas_btn_hint;
    [SerializeField] private Canvas canvas_btn_check;
    [SerializeField] private Text txt_coin_hint;
    [SerializeField] private Text txt_coin_check;
    [Space] 
    [SerializeField] private CanvasGroup canvas_group_info;
    [SerializeField] private Text txt_info_content;
    [SerializeField] private ButtonEffectLogic btn_close_info;
    [Space] 
    [SerializeField] private Transform broad_holder;
    [SerializeField] private GameObject broad_10x10;

    [Header("ATTRIBUTE")] 
    [SerializeField] private int current_level_index;
    [SerializeField] private G7_SO_DataInfoLevel current_level_data;
    [SerializeField] private G7_Broad current_broad;
    private const string info_hint = "Use this hint to reveal an entire row, helping you solve the puzzle faster and uncovering more clues to complete the picture!";
    private const string info_check = "This function scans the game board and highlights all incorrectly marked cells, showing which cells are wrong so that users can correct their mistakes.";

    private const int coin_hint = 50;
    private const int coin_check = 50;

    private const float info_fade_duration = 0.35f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        btn_hint.onClick.AddListener(Button_Hint_Click);
        btn_check.onClick.AddListener(Button_Check_Click);
        btn_hint_info.onClick.AddListener(Button_InfoHint_Click);
        btn_check_info.onClick.AddListener(Button_InfoCheck_Click);
        btn_close_info.onClick.AddListener(Button_CloseInfo_Click);
        txt_coin_hint.text = coin_hint.ToString();
        txt_coin_check.text = coin_check.ToString();
    }

    public void LoadCurrentLevel(int levelIndex)
    {
        current_level_index = levelIndex % DataController.instance.G7_SO_DataList.dataLevelList.Count;
        current_level_data = DataController.instance.G7_SO_DataList.dataLevelList[current_level_index];
        if (current_level_data.row_amount == 10)
        {
            var broad = Instantiate(broad_10x10).transform;
            broad.SetParent(broad_holder);
            broad.localPosition = Vector3.zero;
            broad.localScale = Vector3.one;
        }

        current_broad = G7_Broad.Instance;
        G7_Broad.Instance.LoadBroadData(current_level_data);
    }

    public void LoadNextLevel()
    {
        current_level_index++;
        LoadCurrentLevel(current_level_index);
    }

    public void ShowEndPanel()
    {
        SetBanTouch(false);
        UIController.Instance.uiWinPopup.Show();
    }

    public void SetBanTouch(bool value)
    {
        ban_touch.SetActive(value);
    }
    //Button Click
    #region ButtonClick
    private void Button_Hint_Click()
    {
        //ToDoz
        //Check Coin Here
        
        G7_Broad.Instance.FindClueHint();
    }

    private void Button_Check_Click()
    {
        //ToDoz
        //Check Coin Here
        
        G7_Broad.Instance.DoCheck();
        
    }

    private void Button_InfoHint_Click()
    {
        canvas_btn_hint.overrideSorting = true;
        
        txt_info_content.text = info_hint;
        canvas_group_info.interactable = false;
        canvas_group_info.alpha = 0;
        canvas_group_info.gameObject.SetActive(true);
        canvas_group_info.DOFade(1, info_fade_duration).OnComplete(() =>
        {
            canvas_group_info.interactable = true;
        });
    }

    private void Button_InfoCheck_Click()
    {
        canvas_btn_check.overrideSorting = true;
        
        txt_info_content.text = info_check;
        canvas_group_info.interactable = false;
        canvas_group_info.alpha = 0;
        canvas_group_info.gameObject.SetActive(true);
        canvas_group_info.DOFade(1, info_fade_duration).OnComplete(() =>
        {
            canvas_group_info.interactable = true;
        });
    }

    private void Button_CloseInfo_Click()
    {
        canvas_group_info.interactable = false;
        canvas_group_info.DOFade(0, info_fade_duration).OnComplete(() =>
        {
            canvas_group_info.gameObject.SetActive(false);
            canvas_btn_hint.overrideSorting = false;
            canvas_btn_check.overrideSorting = false;
        });
    }
    #endregion
}
