using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using smallone;

public class UIPanelInventory : UIPanel
{
    public GameObject objSlotItem;
    public Transform trsSlotItemRoot;
    public GameObject objSlotBubble;

    void OnEnable()
    {

        EventCenter.Instance.RegisterGameEvent("CloseInventory", OnCloseBag);
        // EventCenter.Instance.RegisterGameEvent("OpenInventory", OnBagClicked);

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

        //! 因为背包是动态的,每次打开界面都需要检查格子数目
        GenerateBagList();
        GenerateItemsInBag();
    }

    void OnCloseBag()
    {
        UIPanelManager.Instance.HidePanel("UIPanelInventory").DoOnHideCompleted((panel) =>
        {
            Debug.Log("bag closed!");
        });
        objSlotBubble.SetActive(false);
    }

    //! 生成空格子
    void GenerateBagList()
    {
        for (int i = trsSlotItemRoot.childCount; i < GameData.BagCapacity; i++)
        {
            var item = GameObject.Instantiate(objSlotItem) as GameObject;
            item.name = objSlotItem.name + "_" + i;
            item.transform.SetParent(trsSlotItemRoot);
            item.transform.localScale = Vector3.one;
        }
    }
    //! 生成内部道具
    void GenerateItemsInBag()
    {
		// 疑问：这个是排序吗？
		GameData.lstBagItems.Sort((Item x, Item y) => x.Order.CompareTo(y.Order) );
		
        for (int i = 0; i < GameData.BagCapacity; i++)
        {
            UISlotItem item = trsSlotItemRoot.GetChild(i).GetComponent<UISlotItem>();
            UISelectableItem sItem = item.gameObject.AddMissingComponent<UISelectableItem>();
            if (i < GameData.lstBagItems.Count)
            {
                item.imgIcon.sprite = GameData.lstBagItems[i].IconSprite;
                item.ShowIcon = sItem.bSelectable = true;
                sItem.cbSelect = OnSlotSelected;
            }
            else
            {
                item.imgIcon.sprite = null;
                item.ShowIcon = sItem.bSelectable = false;
                sItem.cbSelect = null;
            }
        }
    }

    void OnSlotSelected(bool isSelect, GameObject obj)
    {
        objSlotBubble.SetActive(isSelect);
        objSlotBubble.GetComponent<UIBubbleSlotItemInfo>().txtItemName.text = obj.name;
    }

}
