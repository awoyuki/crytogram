using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class G1_DEVSUPPORT : MonoBehaviour
{
    [SerializeField] ButtonEffectLogic btn_select_level;
    [SerializeField] GameObject SeleclLevelPanel;
    [SerializeField] ButtonEffectLogic buttonPrefab;
    [SerializeField] Transform content;

    private void Start()
    {
        btn_select_level.onClick.AddListener(OnClickBtnSelectLevel);
        var list_data = DataController.instance.G1_SO_DataList.dataLevelList;
        for (int i = 0; i < list_data.Count; i++)
        {
            var btn = Instantiate(buttonPrefab, content);
            var txt = btn.GetComponentInChildren<Text>();
            txt.text = (i + 1).ToString();
            btn.onClick.AddListener(() =>
            {
                /*G1_LevelManager.Instance.uiGameplay.Show();
                G1_LevelManager.Instance.uiGameplay.StartGameByID(int.Parse(txt.text) - 1);*/
            });
        }
        DestroyImmediate(buttonPrefab.gameObject);
    }

    private void OnClickBtnSelectLevel()
    {
        SeleclLevelPanel.SetActive(!SeleclLevelPanel.activeInHierarchy);
    }
}
