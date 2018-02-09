using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using smallone;

public class UIPanelMonster : UIPanel
{
    
    public Transform trsLootListRoot;
    public GameObject objLootSlot;

    public Text txtMonsterName;
    public Text txtMemberNumber;
    public Text txtTime;
    public Text txtScore;
    public Text txtSuccRate;

    private MonsterData _dataMonster;


    void OnEnable()
    {
        EventCenter.Instance.RegisterGameEvent("ClosePanel", OnCloseSelf);
        // EventCenter.Instance.RegisterGameEvent("OpenInventory", OnBagClicked);

        InitMonsterInfo();
    }

    void OnDisable()
    {
        if (EventCenter.Instance != null)
        {
            EventCenter.Instance.UnregisterGameEvent("ClosePanel", OnCloseSelf);
            // EventCenter.Instance.UnregisterButtonEvent("OpenInventory", OnBagClicked);
        }
    }

    void OnCloseSelf()
    {
        UIPanelManager.Instance.HidePanel("UIPanelMonster").DoOnHideCompleted((panel) =>
        {
            Debug.Log("closed!");
        });
    }

    void InitMonsterInfo()
    {
        _dataMonster = GameData.dataCurSelMonster;

        // 怪物名字
        txtMonsterName.text = _dataMonster.Name;

        // TODO::成员数量
        txtMemberNumber.text = "1 - 3";

        // 战斗用时
        txtTime.gameObject.SetActive(false);

        // TODO::战斗力
        txtScore.text = " 100 / " + _dataMonster.Power.ToString();

        // TODO::成功率
        txtSuccRate.text = "100%";


        //刷新掉落
        for (int i = 0; i < _dataMonster.LootList.Count ; i++)
        {
            var item = GameObject.Instantiate(objLootSlot) as GameObject;
            item.name = objLootSlot.name + "_" + i;
            item.transform.SetParent(trsLootListRoot);
            item.transform.localScale = Vector3.one;
        }
    }
}
