﻿using UnityEngine;
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

    [Header("CurrentProduct")]
    public Image imgCurrentProduct;
    public Text txtTimeRemain;
    public Button btSpeedUp;
    public Text txtSpeedUpPrice;

    [Header("WaitingProduct")]
    public Image imgWaitingProduct1_bg;
    public Image imgWaitingProduct1_icon;
    public Image imgWaitingProduct2_bg;
    public Image imgWaitingProduct2_icon;
    public Image imgWaitingProduct3_bg;
    public Image imgWaitingProduct3_icon;

    [Header("CurrentProduct")]
    public Button btAddProductList;
    public Text txtAddProductListPrice;


    [Header("others")]


    public int nSlotListCount = 10;

    private BuildingTask _dataTask;

    GameObject _objBubble;

    private bool _bCanProduct;

    UITimerCtrl timer;
    
    void OnEnable()
    {
        EventCenter.Instance.RegisterGameEvent("ClosePanel", OnCloseSelf);
        // EventCenter.Instance.RegisterGameEvent("OpenInventory", OnBagClicked);


        timer = new UITimerCtrl();

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
        UpdateProductList();
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
            // GameData.AddItemToBag (DataCenter.Instance.dictItem [_dataTask.Product.strId], _dataTask.Product.nCount);

            // 计时器
            // 查找该建筑
            for (int i = 0; i < GameData.lstConstructionObj.Count; i++)
            {
                if(GameData.lstConstructionObj[i].GetComponent<EntityBuilding>().dataBuilding.ID == GameData.strCurConstructionId)
                {
                    Debug.Log(GameData.lstConstructionObj[i].name);
                    GameData.lstConstructionObj[i].GetComponent<EntityBuilding>().AddProduct( _dataTask ); ;
                    
                    break;
                }
            }
            
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

    private void Update()
    {
        UpdateProductList();
    }

    // 生成生产队列
    void UpdateProductList()
    {
        Dictionary<string, BuildingTask> task = DataCenter.Instance.dictBuildingTask;

        EntityBuilding entitybuilding = null;

        for (int i = 0; i < GameData.lstConstructionObj.Count; i++)
        {
            if (GameData.lstConstructionObj[i].GetComponent<EntityBuilding>().dataBuilding.ID == GameData.strCurConstructionId)
            {
                entitybuilding = GameData.lstConstructionObj[i].GetComponent<EntityBuilding>();
                break;
            }
        }

        if (entitybuilding.lstProductItem.Count > 0)
        {
            imgCurrentProduct.sprite = entitybuilding.lstProductItem[0].item.IconSprite;
            txtAddProductListPrice.text = "10";
            txtTimeRemain.text = entitybuilding.timer.Remain.ToString();

            imgCurrentProduct.gameObject.SetActive(true);
            txtAddProductListPrice.gameObject.SetActive(true);
            txtTimeRemain.gameObject.SetActive(true);
            btSpeedUp.gameObject.SetActive(true);
        }
        else
        {
            imgCurrentProduct.gameObject.SetActive(false);
            txtAddProductListPrice.gameObject.SetActive(false);
            txtTimeRemain.gameObject.SetActive(false);
            btSpeedUp.gameObject.SetActive(false);
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
        

        BubbleConstructNeed bubble = _objBubble.GetComponent<BubbleConstructNeed>();
        
		// 任务名字
		bubble.name = _dataTask.Name;

        // 检查目标产品库存数量
        bubble.txtHave.text = GameData.GetItemHave(it).ToString();

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
