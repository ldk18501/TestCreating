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

    GameObject _objBubble;

    void OnEnable()
    {
        EventCenter.Instance.RegisterGameEvent("ClosePanel", OnCloseSelf);
        // EventCenter.Instance.RegisterGameEvent("OpenInventory", OnBagClicked);
        for (int i = 0; i < 3; i++)
        {
            objSlots[i].AddMissingComponent<UISelectableItem>().cbSelect = OnSlotSelect;
        }

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
        //if (objBubble)
        //{
        //    Debug.Log(obj.transform.position + " " + obj.transform.localPosition);
        //    objBubble.transform.localPosition = obj.transform.localPosition + new Vector3(30, 50);
        //}

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
}
