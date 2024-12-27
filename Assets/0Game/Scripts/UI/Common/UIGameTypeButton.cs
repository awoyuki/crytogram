using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameTypeButton : MonoBehaviour
{
    [SerializeField] Text txt_name, txt_level;
    [SerializeField] Transform PanelPos;
    [SerializeField] GameObject BlockPanel;
    [SerializeField] ButtonEffectLogic btn;

    private int gameTypeIndex = -1;
    private GameMode gameModeSaved = GameMode.None;

    private void Awake()
    {
        btn.onClick.AddListener(OnChooseGameType);
    }

    private void OnChooseGameType()
    {
        GameManager.Instance.EnterGameMode(gameTypeIndex);
    }

    public void InitData(int id)
    {
        var datas = DataController.instance.so_ListGameType.data_game_type_list;
        if (id >= datas.Count)
        {
            CommingSoonStat();
            return;
        }
        gameTypeIndex = id;
        gameModeSaved = datas[id].gameMode;
        txt_name.text = string.IsNullOrEmpty(datas[id].so_name) ? datas[id].gameMode.ToString() : datas[id].so_name;
        ReloadInfo();
    }

    public void ReloadInfo()
    {
        if (gameModeSaved == GameMode.None)
            return;
        var cur_level = PrefsData.GetCurrentLevelCount(gameModeSaved);
        if (cur_level < DataController.instance.TotalLevelByGameMode(gameModeSaved))
            txt_level.text = ("Level {0}").Replace("{0}", (cur_level + 1).ToString());
        else
            txt_level.text = "Completed";
    }


    private void CommingSoonStat()
    {
        BlockPanel.SetActive(true);
        txt_name.text = Consts.txt_coming_soon;
        btn.interactable = false;
        btn.hasEffect = false;
    }

}
