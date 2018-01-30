using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using smallone;

public class UIPanelExpedition : UIPanel
{

    public int nLootList = 4;
    public Transform trsLootListRoot;
    public GameObject objLootSlot;

    void OnEnable()
    {
        EventCenter.Instance.RegisterGameEvent("ClosePanel", OnCloseSelf);
        // EventCenter.Instance.RegisterGameEvent("OpenInventory", OnBagClicked);

        InitLootList();
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
        UIPanelManager.Instance.HidePanel("UIPanelExpedition").DoOnHideCompleted((panel) =>
        {
            Debug.Log("closed!");
        });
    }

    void InitLootList()
    {
        for(int i = 0; i < nLootList ; i++)
        {
            var item = GameObject.Instantiate(objLootSlot) as GameObject;
            item.name = objLootSlot.name + "_" + i;
            item.transform.SetParent(trsLootListRoot);
            item.transform.localScale = Vector3.one;
        }
    }
}
