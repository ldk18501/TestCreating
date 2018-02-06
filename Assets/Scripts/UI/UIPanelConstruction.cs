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

	private bool _bCanProduct;
    
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
		if (_objBubble) {
			_objBubble.GetComponent<RectTransform> ().anchoredPosition = trsGroup.localPosition + obj.transform.localPosition + new Vector3 (-316, 131);
			_objBubble.SetActive (isSelect);

//             Debug.Log(obj.name + " == " );
		} else {
			return;
		}

		if (isSelect) 
		{
			
			_dataTask = DataCenter.Instance.dictBuildingTask[ obj.name ];

			GenerateContructNeed();
		}
		// TODO::临时::快速生产
		else if(!isSelect && _bCanProduct)
		{
			// 扣除道具
			for (int i = 0; i < _dataTask.ItemRequire.Count ; i++)
			{
				GameData.DelItemFromBag (DataCenter.Instance.dictItem [_dataTask.ItemRequire [i].strId], _dataTask.ItemRequire [i].nCount);
			}


			// 生产物品放入背包

			GameData.AddItemToBag (DataCenter.Instance.dictItem [_dataTask.Product.strId], _dataTask.Product.nCount);

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


        // 创建可生产道具
        foreach (string id in task.Keys)
        {
			if( task[id].TableId == GameData.strCurConstructionId && task[id].Lv <= GameData.nPlayerLv )
            {
                var obj = GameObject.Instantiate(objSlotItem) as GameObject;
                obj.transform.SetParent(trsGroup);

                obj.AddMissingComponent<UISelectableItem>().cbSelect = OnSlotSelect;
                obj.transform.localScale = Vector3.one;
                
				string itemID = task[id].Product.strId;
				obj.name = id ;

				obj.GetComponent<UISlotItem> ().UpdateShowInfo (DataCenter.Instance.dictItem[itemID]);

				Debug.Log ("itemID = " + obj.GetComponent<UISlotItem> ().item.ID );

				if (task [id].Product.nCount <= 1) {
					obj.GetComponent<UISlotItem> ().ShowCount = false;
				} else {

					obj.GetComponent<UISlotItem> ().txtCount.text = "x " + task [id].Product.nCount;
					obj.GetComponent<UISlotItem> ().ShowCount = true;
				}
            }
        }        
    }


    void GenerateContructNeed()
    {
		_bCanProduct = true;

        // 先清空列表
        for (int i = 0; i < _objBubble.GetComponent<BubbleConstructNeed>().trsNeedRoot.childCount; i++)
        {
            GameObject.Destroy(_objBubble.GetComponent<BubbleConstructNeed>().trsNeedRoot.GetChild(i).gameObject);
        }


		// 目标生产物品
		Item it = DataCenter.Instance.dictItem[_dataTask.Product.strId];

		// 检查库存数量
		int store = GameData.GetItemHave( it );

        Debug.Log("store = " + store);

        BubbleConstructNeed bubble = _objBubble.GetComponent<BubbleConstructNeed>();
        
		// 任务名字
		bubble.name = _dataTask.Name;

		Debug.Log(bubble.name);

        // 道具名字
        bubble.txtName.text = it.Name;
        
        // 道具属性查找
        bubble.txtScore.text = DataCenter.Instance.dictItem[_dataTask.Product.strId].Power.ToString();
        
        // 生产所需时间
        bubble.txtTime.text = _dataTask.Time.ToString();

        // 生成所需道具列表
        for (int i = 0 ; i < _dataTask.ItemRequire.Count ;i++ )
        {
            Debug.Log("TableId = " + _dataTask.TableId + " , TaskId = " + _dataTask.ID + " , RequireItem.id = " + _dataTask.ItemRequire[i].strId);

            var obj = GameObject.Instantiate( _objBubble.GetComponent<BubbleConstructNeed>().objSlotNeed ) as GameObject;
            obj.transform.SetParent(_objBubble.GetComponent<BubbleConstructNeed>().trsNeedRoot);
			obj.name = _dataTask.ItemRequire[i].strId;
            obj.transform.localScale = Vector3.one;
            obj.GetComponent<SlotConstructNeed>().imgIcon.sprite = DataCenter.Instance.dictItem[_dataTask.ItemRequire[i].strId].IconSprite;

			// 所需材料的库存量
			int count = GameData.GetItemHave( DataCenter.Instance.dictItem[_dataTask.ItemRequire[i].strId] );

            string needcount = count + " / " +_dataTask.ItemRequire[i].nCount.ToString();

			// 如果数量不满足，就不能造
			if(count < _dataTask.ItemRequire[i].nCount)
			{
				_bCanProduct = false;
			}

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
