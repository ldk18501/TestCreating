using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using smallone;

public class UIPanelMission : UIPanel
{

    public int nNeedList = 3;
    public Transform trsNeedListRoot;
    public GameObject objNeedSlot;

    void OnEnable()
    {
        EventCenter.Instance.RegisterGameEvent("ClosePanel", OnCloseSelf);
        // EventCenter.Instance.RegisterGameEvent("OpenInventory", OnBagClicked);

        InitNeedList();
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
        UIPanelManager.Instance.HidePanel("UIPanelMission").DoOnHideCompleted((panel) =>
        {
            Debug.Log("closed!");
        });
    }

    void InitNeedList()
    {
        for (int i = 0; i < nNeedList; i++)
        {
            var item = GameObject.Instantiate(objNeedSlot) as GameObject;
            item.name = objNeedSlot.name + "_" + i;
            item.transform.SetParent(trsNeedListRoot);
            item.transform.localScale = Vector3.one;
        }
    }

}
