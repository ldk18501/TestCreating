using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using smallone;
using TMPro;


public class UIPanelPlayerLevelUp : UIPanel
{

    public Transform trsPrizeRoot;
    public GameObject objPrize;
    public TextMeshProUGUI txtTitle;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    void OnEnable()
    {
        EventCenter.Instance.RegisterGameEvent("CloseLvUp", OnClosePanel);
        // EventCenter.Instance.RegisterGameEvent("OpenInventory", OnBagClicked);
        
    }

    void OnDisable()
    {
        if (EventCenter.Instance != null)
        {
            EventCenter.Instance.UnregisterGameEvent("CloseInventory", OnClosePanel);
            // EventCenter.Instance.UnregisterButtonEvent("OpenInventory", OnBagClicked);
        }

    }

    protected override void OnPanelShowBegin()
    {
        base.OnPanelShowBegin();

        PanelInit();
    }

    protected override void OnPanelHideCompleted()
    {
        base.OnPanelHideCompleted();
    }


    void OnClosePanel()
    {
        UIPanelManager.Instance.HidePanel("UIPanelPlayerLevelUp").DoOnHideCompleted((panel) =>
        {
            for(int i = 0; i< trsPrizeRoot.childCount; i++ )
            {
                GameObject.Destroy(trsPrizeRoot.GetChild(i).gameObject);
            }

            Debug.Log("bag closed!");
        });
        
    }


    void PanelInit()
    {

        // TODO::玩家等级需要判断最高玩家等级
        string playerlvl = GameData.nPlayerLv.ToString();

        PlayerLvlData playerlvldata = DataCenter.Instance.dictPlyerLvlData[playerlvl];

        // 玩家升级信息：解锁建筑
        for (int i = 0; i < playerlvldata.TableUnlock.Count; i++)
        {
            var item = GameObject.Instantiate(objPrize) as GameObject;
            item.name = DataCenter.Instance.dictBuilding[playerlvldata.TableUnlock[i]].Name;
            item.transform.SetParent(trsPrizeRoot);
            item.transform.localScale = Vector3.one;
            item.GetComponent<UISlotItem>().imgIcon.sprite = DataCenter.Instance.dictBuilding[playerlvldata.TableUnlock[i]].IconSprite;

            // TODO::品质
            item.GetComponent<UISlotItem>().ShowQuality = false;
            item.GetComponent<UISlotItem>().ShowScore = false;
            item.GetComponent<UISlotItem>().ShowCount = false;
            item.GetComponent<UISlotItem>().ShowScore = false;
        }

        // 玩家升级信息：解锁任务
        for (int i = 0; i < playerlvldata.TaskUnlock.Count; i++)
        {
            var item = GameObject.Instantiate(objPrize) as GameObject;
            item.name = DataCenter.Instance.dictBuildingTask[playerlvldata.TaskUnlock[i]].Name;
            item.transform.SetParent(trsPrizeRoot);
            item.transform.localScale = Vector3.one;

            string productid = DataCenter.Instance.dictBuildingTask[playerlvldata.TaskUnlock[i]].Product.strId;
            item.GetComponent<UISlotItem>().imgIcon.sprite = DataCenter.Instance.dictItem[productid].IconSprite;

            // TODO::品质
            // 疑问：为啥界面中还在显示
            item.GetComponent<UISlotItem>().ShowQuality = false;
            item.GetComponent<UISlotItem>().ShowScore = false;
            item.GetComponent<UISlotItem>().ShowCount = false;
            item.GetComponent<UISlotItem>().ShowScore = false;
        }

        // 标题信息
        txtTitle.text = "Congratulations!!! Lv : " + playerlvldata.Lv.ToString();
    }

}
