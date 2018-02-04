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

    private BuildingTask _dataTask;
    
    void OnEnable()
    {
        EventCenter.Instance.RegisterGameEvent("ClosePanel", OnCloseSelf);
        // EventCenter.Instance.RegisterGameEvent("OpenInventory", OnBagClicked);

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

        GenerateSlotList();
    }

    protected override void OnPanelHideCompleted()
    {
        base.OnPanelHideCompleted();

        
        for(int i = 0; i < trsGroup.childCount ; i++)
        {
            GameObject.Destroy( trsGroup.GetChild(i).gameObject );
        }



    }


    void OnSlotSelect(bool isSelect, GameObject obj)
    {
        if (objBubble)
        {
            _objBubble.GetComponent<RectTransform>().anchoredPosition = trsGroup.localPosition + obj.transform.localPosition + new Vector3(-316, 131);
            _objBubble.SetActive(isSelect);

//             Debug.Log(obj.name + " == " );

            _dataTask = DataCenter.Instance.dictBuildingTask[obj.name];

        }


    }

    void BubbleItemInfo()
    {
        _objBubble = GameObject.Instantiate(objBubble) as GameObject;
        _objBubble.transform.SetParent(transform);
        _objBubble.transform.SetAsLastSibling();
        _objBubble.transform.localScale = Vector3.one;
        _objBubble.SetActive(false);
        
    }


    void GenerateSlotList()
    {
        Dictionary<string, BuildingTask> task = DataCenter.Instance.dictBuildingTask;



        foreach (string id in task.Keys)
        {
			if( task[id].TableId == GameData.strCurConstructionId )
            {
                var obj = GameObject.Instantiate(objSlotItem) as GameObject;
                obj.transform.SetParent(trsGroup);
                obj.name = task[id].ID;
                obj.AddMissingComponent<UISelectableItem>().cbSelect = OnSlotSelect;
                obj.transform.localScale = Vector3.one;


                // 问题：同一个道具不同品质，怎么写比较好(或者怎么配置)

                string itemID = task[id].Product.strId.ToString();

//                 Debug.Log(itemID);
                
                obj.GetComponent<UISlotItem>().imgIcon.sprite = DataCenter.Instance.dictItem[itemID].IconSprite;
                obj.GetComponent<UISlotItem>().imgQuality.gameObject.SetActive(false);
                obj.GetComponent<UISlotItem>().txtScore.gameObject.SetActive(false);
            }
        }        
    }


    void GenerateContructNeed()
    {
        // 先清空列表
        for (int i = 0; i < _objBubble.GetComponent<BubbleConstructNeed>().trsNeedRoot.childCount; i++)
        {
            GameObject.Destroy(_objBubble.GetComponent<BubbleConstructNeed>().trsNeedRoot.GetChild(i).gameObject);
        }


        int store = 0;
        // TODO 背包存量查找
        for(int i = 0; i< GameData.lstBagItems.Count; i++)
        {
            if( _dataTask.Product.strId == GameData.lstBagItems[i].ID )
            {
                store++;
            }
        }

        Debug.Log(store);

        BubbleConstructNeed bubble = _objBubble.GetComponent<BubbleConstructNeed>();
        

		bubble.name = store.ToString();


		Debug.Log(bubble.name );

        // 道具名字
        bubble.txtName.text = _dataTask.Name;



        // 道具属性查找
        bubble.txtScore.text = DataCenter.Instance.dictItem[_dataTask.Product.strId].Power.ToString();
        
        // 生产所需时间
        bubble.txtTime.text = _dataTask.Time.ToString();

        // 生成所需道具列表
        for (int i = 0 ; i < _dataTask.ItemRequire.Count ;i++ )
        {
            var obj = GameObject.Instantiate( _objBubble.GetComponent<BubbleConstructNeed>().objSlotNeed ) as GameObject;
            obj.transform.SetParent(_objBubble.GetComponent<BubbleConstructNeed>().trsNeedRoot);
            obj.name = _dataTask.Name;
            obj.transform.localScale = Vector3.one;
            obj.GetComponent<SlotConstructNeed>().imgIcon.sprite = DataCenter.Instance.dictItem[_dataTask.ItemRequire[i].strId].IconSprite;

            string needcount = store + " / " +_dataTask.ItemRequire[i].nCount.ToString();
            obj.GetComponent<SlotConstructNeed>().txtNeed.text = needcount;
        }
    }


	// 生成生产中商品
	void GenerateProductItem()
	{





	}

	// 新增生产商品
	void AddNewProduct(BuildingTask taskId)
	{



	}


}
