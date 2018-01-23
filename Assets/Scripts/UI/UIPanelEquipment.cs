using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using smallone;

public class UIPanelEquipment : UIPanel
{
    public GameObject objEquipmentDesc;
    public GameObject objEquipmentBag;

    private bool _IsEquipmentInfoShow;

    void OnEnable()
    {
        EventCenter.Instance.RegisterGameEvent("ClosePanel", OnCloseSelf);
        // EventCenter.Instance.RegisterGameEvent("OpenInventory", OnBagClicked);
        EventCenter.Instance.RegisterGameEvent("SwitchEquipment", OnSwitchEquipment);
        EventCenter.Instance.RegisterGameEvent("CloseEquipmentInfo", OnCloseEquipmentInfo);
        OnCloseEquipmentInfo();
    }

    void OnDisable()
    {
        if (EventCenter.Instance != null)
        {
            EventCenter.Instance.UnregisterGameEvent("ClosePanel", OnCloseSelf);
            EventCenter.Instance.UnregisterGameEvent("SwitchEquipment", OnSwitchEquipment);
            EventCenter.Instance.UnregisterGameEvent("CloseEquipmentInfo", OnCloseEquipmentInfo);
            // EventCenter.Instance.UnregisterButtonEvent("OpenInventory", OnBagClicked);
        }

        OnCloseEquipmentInfo();
    }

    void OnCloseSelf()
    {
        UIPanelManager.Instance.HidePanel("UIPanelEquipment").DoOnHideCompleted((panel) =>
        {
            Debug.Log("closed!");
        });

        OnCloseEquipmentInfo();
    }


    void OnSwitchEquipment()
    {
//         // 如果点击了同一个slot，则关闭
//         if()
//         {
// 
//         }
//         // 打开换装信息
//         else
//         {
//         }

        if(_IsEquipmentInfoShow)
        {
            _IsEquipmentInfoShow = false;
            ShowInfo(_IsEquipmentInfoShow);
        }
        else
        {
            _IsEquipmentInfoShow = true;
            ShowInfo(_IsEquipmentInfoShow);
        }
    }


    void OnCloseEquipmentInfo()
    {
        _IsEquipmentInfoShow = false;
        ShowInfo(_IsEquipmentInfoShow);
    }


    void ShowInfo(bool IsShow)
    {
        objEquipmentBag.SetActive(IsShow);
        objEquipmentDesc.SetActive(IsShow);
    }

  
}
