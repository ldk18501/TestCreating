using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using smallone;

public class UIPanelTrade : UIPanel
{
    public GameObject objBubble;
    public Transform trsGoodListRoot;
    public GameObject objGoodSlot;
    public Text txtTitle;

    GameObject _objBubble;

    void OnEnable()
    {
        EventCenter.Instance.RegisterGameEvent("ClosePanel", OnCloseSelf);
        // EventCenter.Instance.RegisterGameEvent("OpenInventory", OnBagClicked);
        
    }

    void OnDisable()
    {
        if (EventCenter.Instance != null)
        {
            EventCenter.Instance.UnregisterGameEvent("ClosePanel", OnCloseSelf);
            // EventCenter.Instance.UnregisterButtonEvent("OpenInventory", OnBagClicked);
        }
        GameObject.Destroy(_objBubble);
    }

    protected override void OnPanelShowBegin()
    {
        base.OnPanelShowBegin();

        txtTitle.text = "小兄弟，你买得起吗？";

        InitGoodList();
        BubbleGood();
        _objBubble.SetActive(false);
        
    }

    void OnCloseSelf()
    {
        UIPanelManager.Instance.HidePanel("UIPanelTrade").DoOnHideCompleted((panel) =>
        {
            Debug.Log("closed!");
        });

        for(int i = 0; i < trsGoodListRoot.childCount ; i++)
        {
            GameObject.Destroy( trsGoodListRoot.GetChild(i).gameObject );
        }


    }

    void OnSlotSelect(bool isSelect, GameObject obj)
    {
        BuildingTask task = DataCenter.Instance.dictBuildingTask[  obj.GetComponent<UISlotShopGood>().datapaper.TskId ];
        _objBubble.GetComponent<UIBubbleStoreGoodInfo>().UpdateBubbleInfo(task);
        _objBubble.SetActive(isSelect);
    }

    void BubbleGood()
    {
        _objBubble = GameObject.Instantiate(objBubble) as GameObject;
        _objBubble.SetActive(false);
        _objBubble.transform.SetParent(transform);
        _objBubble.transform.SetAsLastSibling();
        _objBubble.transform.localScale = Vector3.one;
        _objBubble.GetComponent<RectTransform>().anchoredPosition = new Vector3(346, -250);
    }

    void InitGoodList()
    {
        foreach(string id in DataCenter.Instance.dictPaperShop.Keys)
        {
            if (DataCenter.Instance.dictPaperShop[id].PlayerLv <= GameData.nPlayerLv)
            {
                bool IsHave = false;
                for(int i = 0; i < GameData.lstPaper.Count; i++)
                {
                    if (GameData.lstPaper[i] == DataCenter.Instance.dictPaperShop[id])
                    {
                        IsHave = true;
                        break;
                    }
                }

                if(!IsHave)
                {
                    var item = GameObject.Instantiate(objGoodSlot) as GameObject;
                    item.transform.SetParent(trsGoodListRoot);  
                    item.transform.localScale = Vector3.one;
                    item.GetComponent<UISlotShopGood>().UpdateSlotInfo(DataCenter.Instance.dictPaperShop[id]);

                    item.GetComponent<UISlotShopGood>().btBuy.onClick.AddListener(() => { OnBuy(item); });

                    // item.GetComponent<UISlotShopGood>().imgIcon.gameObject.AddMissingComponent<UISelectableItem>().cbSelect = OnSlotSelect;
                    item.AddMissingComponent<UISelectableItem>().cbSelect = OnSlotSelect;
                }
            }
        }

    }

    void OnBuy(GameObject obj)
    {
        PaperShop pp = obj.GetComponent<UISlotShopGood>().datapaper;

        GameData.lstPaper.Add( pp );

        Item it = DataCenter.Instance.dictItem[ pp.Price[0].strId ];

        // 钱
        if (it.ID == "1")
        {
            GameData.Coins -= pp.Price[0].nCount;
        }
        //水晶
        else if (it.ID == "2")
        {
            GameData.Gems -= pp.Price[0].nCount;
        }

        
        GameObject.Destroy(obj);

        // 刷新能不能买
        for (int i = 0;i<trsGoodListRoot.childCount; i++)
        {
            GameObject objaa = trsGoodListRoot.GetChild(i).gameObject;

            objaa.GetComponent<UISlotShopGood>().UpdateSlotInfo( objaa.GetComponent<UISlotShopGood>().datapaper );

        }

        UIPanelManager.Instance.GetPanel("UIGameHUD").Repaint();

    }



}
