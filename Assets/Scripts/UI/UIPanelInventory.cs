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
		int bag_min = int.Parse( GameData.dictGameConfs [Consts.G_BAG_MIN].Value);
		int bag_add = int.Parse( GameData.dictGameConfs [Consts.G_BAG_ADD].Value);

		int bag_slotcount = (int)( Mathf.Max( bag_min , GameData.lstBagItems.Count )/ bag_add ) * bag_add;

		bag_slotcount = Mathf.Min( bag_slotcount,int.Parse( GameData.dictGameConfs [Consts.G_BAG_MAX].Value));

        for (int i = trsSlotItemRoot.childCount; i < bag_slotcount ; i++)
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

		for (int i = 0; i < trsSlotItemRoot.childCount ; i++)
        {
            UISlotItem item = trsSlotItemRoot.GetChild(i).GetComponent<UISlotItem>();
            UISelectableItem sItem = item.gameObject.AddMissingComponent<UISelectableItem>();
            if (i < GameData.lstBagItems.Count)
            {
                item.imgIcon.sprite = GameData.lstBagItems[i].IconSprite;
				item.item = GameData.lstBagItems [i];
                item.ShowIcon = sItem.bSelectable = true;
				item.ShowQuality = false;


                sItem.cbSelect = OnSlotSelected;

				if (GameData.lstBagItems [i].Category == ItemType.Equipment) 
				{
					item.txtScore.text = GameData.lstBagItems [i].Power.ToString();
					item.ShowScore = true;

				} else {
					item.ShowScore = false;
				}

            }
            else
            {
                item.imgIcon.sprite = null;
                item.ShowIcon = sItem.bSelectable = false;
				item.ShowQuality = false;
				item.ShowScore = false;
                sItem.cbSelect = null;
            }
        }
    }

    void OnSlotSelected(bool isSelect, GameObject obj)
    {
		UISlotItem item = obj.GetComponent<UISlotItem>();
		UISlotItemInfo bubble = objSlotBubble.GetComponent<UISlotItemInfo> ();

		bubble.txtItemName.text = item.item.Name;

		if (item.item.Category == ItemType.Equipment) 
		{
			bubble.txtScore.text = item.item.Power.ToString();
			bubble.imgItemType.sprite = null;

			bubble.txtScore.gameObject.SetActive (true);
			bubble.imgScore.gameObject.SetActive (true);
			bubble.imgItemType.gameObject.SetActive (true);

		} 
		else 
		{
			bubble.txtScore.gameObject.SetActive (false);
			bubble.imgScore.gameObject.SetActive (false);
			bubble.imgItemType.gameObject.SetActive (false);
		}

		bubble.txtItemInfo.text = item.item.Info ;

		Debug.Log ("ItemId = " + item.item.ID + " , ItemName = " + item.item.Name + "ItemCategory = " + item.item.Category );

		objSlotBubble.SetActive(isSelect);

    }

}
