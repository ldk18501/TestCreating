using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using smallone;

public class UIPanelConstruction : UIPanel
{
    public Transform trsGroup;
    public GameObject objBubble;
    public GameObject objSlotItem;

    GameObject _objBubble;

    public int nSlotListCount = 10;


    void OnEnable()
    {
        EventCenter.Instance.RegisterGameEvent("ClosePanel", OnCloseSelf);
        // EventCenter.Instance.RegisterGameEvent("OpenInventory", OnBagClicked);


        SlotListInit();


        BubbleItemInfo();

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

    void OnCloseSelf()
    {
        UIPanelManager.Instance.HidePanel("UIPanelConstruction").DoOnHideCompleted((panel) =>
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
        if (objBubble)
        {
            _objBubble.GetComponent<RectTransform>().anchoredPosition = trsGroup.localPosition + obj.transform.localPosition + new Vector3(-316, 131);
            _objBubble.SetActive(isSelect);
        }

        
    }

    void BubbleItemInfo()
    {
        _objBubble = GameObject.Instantiate(objBubble) as GameObject;
        _objBubble.SetActive(false);
        _objBubble.transform.SetParent(transform);
        _objBubble.transform.SetAsLastSibling();
        _objBubble.transform.localScale = Vector3.one;
    }


    void SlotListInit()
    {
        for (int i = 0; i < nSlotListCount; i++)
        {
            var obj = GameObject.Instantiate(objSlotItem) as GameObject;
            obj.transform.SetParent(trsGroup);
            obj.name = objSlotItem.name + "_" + i;
            obj.AddMissingComponent<UISelectableItem>().cbSelect = OnSlotSelect;
            obj.transform.localScale = Vector3.one;
            obj.GetComponent<UISlotItem>().txtScore.gameObject.SetActive(false);
            
        }

        
    }
}
