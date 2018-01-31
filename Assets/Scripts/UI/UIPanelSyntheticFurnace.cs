using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using smallone;

public class UIPanelSyntheticFurnace : UIPanel
{
    public Transform trsGroup;
    public GameObject objEquipmentBag;
    public GameObject objBubble;

    GameObject _objBubble;

    private bool _bBagShow;


    void OnEnable()
    {
        EventCenter.Instance.RegisterGameEvent("ClosePanel", OnCloseSelf);
        EventCenter.Instance.RegisterGameEvent("OpenBag", OnOpenEquipmentBag);
        EventCenter.Instance.RegisterGameEvent("CloseBag", OnCloseEquipmentBag);
        // EventCenter.Instance.RegisterGameEvent("OpenInventory", OnBagClicked);

        for (int i = 0; i < trsGroup.childCount; i++)
        {
            trsGroup.GetChild(i).gameObject.AddMissingComponent<UISelectableItem>().cbSelect = OnSlotSelect;
        }

//         BubbleItemInfo();
// 
//         _bBagShow = false;
//         objEquipmentBag.SetActive(_bBagShow);

    }

    void OnDisable()
    {
        if (EventCenter.Instance != null)
        {
            EventCenter.Instance.UnregisterGameEvent("ClosePanel", OnCloseSelf);
            EventCenter.Instance.UnregisterGameEvent("OpenBag", OnOpenEquipmentBag);
            EventCenter.Instance.UnregisterGameEvent("CloseBag", OnCloseEquipmentBag);
            // EventCenter.Instance.UnregisterButtonEvent("OpenInventory", OnBagClicked);
        }
        GameObject.Destroy(_objBubble);
        

//         _bBagShow = false;
//         objEquipmentBag.SetActive(_bBagShow);
    }


    void OnOpenEquipmentBag()
    {

//         _bBagShow = true;
//         objEquipmentBag.SetActive(_bBagShow);
    }

    void OnCloseEquipmentBag()
    {

//         _bBagShow = false;
//         objEquipmentBag.SetActive(_bBagShow);
    }


    void OnCloseSelf()
    {
        UIPanelManager.Instance.HidePanel("UIPanelSyntheticFurnace").DoOnHideCompleted((panel) =>
        {
            Debug.Log("closed!");
        });
    }


    protected override void OnPanelShowBegin()
    {
        base.OnPanelShowBegin();
        _objBubble.SetActive(false);
    }


    void OnSlotSelect(bool isSelect, GameObject obj)
    {
        //         if (objBubble)
        //         {
        //             _objBubble.GetComponent<RectTransform>().anchoredPosition = trsGroup.localPosition + obj.transform.localPosition + new Vector3(-316, 131);
        //         }
        // 
        // 
        //         if (_objBubble)
        //         {
        //             _objBubble.SetActive(isSelect);
        //         }
    }

    void BubbleItemInfo()
    {
        _objBubble = GameObject.Instantiate(objBubble) as GameObject;
        _objBubble.SetActive(false);
        _objBubble.transform.SetParent(transform);
        _objBubble.transform.SetAsLastSibling();
        _objBubble.transform.localScale = Vector3.one;

    }
}
