using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using smallone;

public class UIPanelTrade : UIPanel
{
    public GameObject objBubble;
    public GameObject[] objSlots;

    void OnEnable()
    {
        EventCenter.Instance.RegisterGameEvent("ClosePanel", OnCloseSelf);
        // EventCenter.Instance.RegisterGameEvent("OpenInventory", OnBagClicked);
        for (int i = 0; i < 3; i++)
        {
            objSlots[i].AddMissingComponent<UISelectableItem>().cbSelect = OnSlotSelect;
        }
    }

    void OnDisable()
    {
        if (EventCenter.Instance != null)
        {
            EventCenter.Instance.UnregisterGameEvent("ClosePanel", OnCloseSelf);
            // EventCenter.Instance.UnregisterButtonEvent("OpenInventory", OnBagClicked);
        }
    }

    protected override void OnPanelShowBegin()
    {
        base.OnPanelShowBegin();
        objBubble.SetActive(false);
    }

    void OnCloseSelf()
    {
        UIPanelManager.Instance.HidePanel("UIPanelTrade").DoOnHideCompleted((panel) =>
        {
            Debug.Log("closed!");
        });
    }

    void OnSlotSelect(bool isSelect,GameObject obj) {
        if (objBubble)
            objBubble.transform.localPosition = obj.transform.localPosition + new Vector3(30, 50);
        objBubble.SetActive(isSelect);
    }
}
