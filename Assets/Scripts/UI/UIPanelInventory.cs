using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using smallone;

public class UIPanelInventory : UIPanel
{
    public int nBagListCount = 15;
    public int nBagSlotMin = 6;
    public int nBagSlotMax = 60;
    public int nBagSlotAdd = 6;

    public GameObject objSlotItem;
    public Transform trsSlotItemRoot;
    public GameObject objSlotBubble;

    void OnEnable()
    {
        
        EventCenter.Instance.RegisterGameEvent("CloseInventory", OnCloseBag);
        // EventCenter.Instance.RegisterGameEvent("OpenInventory", OnBagClicked);

        //在这里初始化背包
        GenerateBagList();
        objSlotBubble.SetActive(false);
    }

    void OnDisable()
    {
        if (EventCenter.Instance != null)
        {
            EventCenter.Instance.UnregisterGameEvent("CloseInventory", OnCloseBag);
            // EventCenter.Instance.UnregisterButtonEvent("OpenInventory", OnBagClicked);
        }
        objSlotBubble.SetActive(false);
    }

    protected override void OnPanelShowBegin()
    {
        base.OnPanelShowBegin();
        Debug.Log(GameData.lstBagItems.Count);
    }

    void OnCloseBag()
    {
        UIPanelManager.Instance.HidePanel("UIPanelInventory").DoOnHideCompleted((panel) =>
        {
            Debug.Log("bag closed!");
        });
        objSlotBubble.SetActive(false);
    }


    void GenerateBagList()
    {
        int BagSlotCount = 0; ;

        // 背包格子数量
        if(nBagListCount < nBagSlotMin)
        {
            BagSlotCount = nBagSlotMin;
        }
        else
        {
            BagSlotCount = Mathf.CeilToInt( Mathf.Min(nBagListCount,nBagSlotMax)/(float)nBagSlotAdd) * nBagSlotAdd;

        }

        Debug.Log(BagSlotCount);



        for (int i = 0; i < BagSlotCount; i++)
        {
            var item = GameObject.Instantiate(objSlotItem) as GameObject;
            item.name = objSlotItem.name + "_" + i;
            item.transform.SetParent(trsSlotItemRoot);
            item.transform.localScale = Vector3.one;
            //             item.GetComponent<UIRoleInfo>().btRole.onClick.AddListener(() => { OnHeroClicked(item); });
            //             item.GetComponent<UIRoleInfo>().btMission.onClick.AddListener(() => { OnMissionClicked(item); });
                
            if (i < nBagListCount)
            {
                item.AddMissingComponent<UISelectableItem>().cbSelect = OnSlotSelected;
            }
            else
            {
                item.GetComponent<UISlotItem>().imgIcon.gameObject.SetActive(false);
                item.GetComponent<UISlotItem>().txtScore.gameObject.SetActive(false);
            }


            Debug.Log(item.name);

        }

    }

    void OnSlotSelected(bool isSelect,GameObject obj)
    {
        objSlotBubble.SetActive(isSelect);
        objSlotBubble.GetComponent<UIBubbleSlotItemInfo>().txtItemName.text = obj.name ;

    }

}
