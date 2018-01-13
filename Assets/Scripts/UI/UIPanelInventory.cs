using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using smallone;

public class UIPanelInventory : UIPanel
{
    void OnEnable()
    {
        EventCenter.Instance.RegisterGameEvent("CloseInventory", OnCloseBag);
        // EventCenter.Instance.RegisterGameEvent("OpenInventory", OnBagClicked);

        //在这里初始化背包
    }

    void OnDisable()
    {
        if (EventCenter.Instance != null)
        {
            EventCenter.Instance.UnregisterGameEvent("CloseInventory", OnCloseBag);
            // EventCenter.Instance.UnregisterButtonEvent("OpenInventory", OnBagClicked);
        }
    }

    void OnCloseBag()
    {
        UIPanelManager.Instance.HidePanel("UIPanelInventory").DoOnHideCompleted((panel) =>
        {
            Debug.Log("bag closed!");
        });
    }
}
