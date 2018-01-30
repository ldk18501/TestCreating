using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using smallone;

public class UIPanelTrade : UIPanel
{
    public int nGoodList = 5;
    public GameObject objBubble;
    public Transform trsGoodListRoot;
    public GameObject objGoodSlot;

    GameObject _objBubble;

    void OnEnable()
    {
        EventCenter.Instance.RegisterGameEvent("ClosePanel", OnCloseSelf);
        // EventCenter.Instance.RegisterGameEvent("OpenInventory", OnBagClicked);


        InitGoodList();
        BubbleGood();
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
        _objBubble.SetActive(false);
    }

    void OnCloseSelf()
    {
        UIPanelManager.Instance.HidePanel("UIPanelTrade").DoOnHideCompleted((panel) =>
        {
            Debug.Log("closed!");
        });
    }

    void OnSlotSelect(bool isSelect, GameObject obj)
    {
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
        for (int i = 0; i < nGoodList; i++)
        {
            var item = GameObject.Instantiate(objGoodSlot) as GameObject;
            item.name = objGoodSlot.name + "_" + i;
            item.transform.SetParent(trsGoodListRoot);
            item.transform.localScale = Vector3.one;
            item.AddMissingComponent<UISelectableItem>().cbSelect = OnSlotSelect;
        }
    }
}
