using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using smallone;

public class UIPanelEquipment : UIPanel
{
    public Transform trsSlotItemRoot;

    public GameObject objEquipmentDesc;
    public GameObject objEquipmentBag;
    public Button btClose; 
    public Button btUnEquip;

    [Header("PlayerInfo")]
    public Text txtNpcName;
    public Text txtScore;
    
	[Header("Skills")]
	public Transform trsNpcSkillRoot;
	public GameObject objSkillSlot;

	[Header("Prefers")]
	public Transform trsNpcPreferRoot;
	public GameObject objPreferSlot;

	[Header("Traits")]
	public Transform trsNpcTraitRoot;
	public GameObject objTraitSlot;


    private bool _IsEquipmentInfoShow;
    private NPCData _npcdata;


    void OnEnable()
    {
        EventCenter.Instance.RegisterGameEvent("ClosePanel", OnCloseSelf);
        // EventCenter.Instance.RegisterGameEvent("OpenInventory", OnBagClicked);
        //EventCenter.Instance.RegisterGameEvent("SwitchEquipment", OnSwitchEquipment);
        EventCenter.Instance.RegisterGameEvent("CloseEquipmentInfo", OnCloseBag);
    }

    void OnDisable()
    {
        if (EventCenter.Instance != null)
        {
            EventCenter.Instance.UnregisterGameEvent("ClosePanel", OnCloseSelf);
            //EventCenter.Instance.UnregisterGameEvent("SwitchEquipment", OnSwitchEquipment);
            EventCenter.Instance.UnregisterGameEvent("CloseEquipmentInfo", OnCloseBag);
            // EventCenter.Instance.UnregisterButtonEvent("OpenInventory", OnBagClicked);
        }

        OnCloseBag();
    }

    void OnCloseSelf()
    {
        UIPanelManager.Instance.HidePanel("UIPanelEquipment").DoOnHideCompleted((panel) =>
        {
            Debug.Log("closed!");
        });

        OnCloseBag();
    }

    protected override void OnPanelShowBegin()
    {
        base.OnPanelShowBegin();


		// 得到该界面的npc信息
		for (int i = 0; i < GameData.lstNpcs.Count ; i++) {
			if( GameData.strCurNpcId ==  GameData.lstNpcs[i].ID ){
				_npcdata = GameData.lstUnlockNpcs [i];
			}
		}

		// 临时方案，以防万一
		if(_npcdata == null){
			_npcdata = GameData.lstUnlockNpcs[0];
        }
        OnCloseBag();



        // 生成NPC信息
		GenerateNpcInfo();


    }

    protected override void OnPanelHideCompleted()
    {
        base.OnPanelHideCompleted();

        GameData.strCurNpcId = null;
    }


    void OnOpenBag(GameObject objSelectedSlot)
    {
        //         // 如果点击了同一个slot，则关闭
        //         if()
        //         {
        // 
        //         }
        //         // 如果点击了不同的slot，打开换装信息
        //         else
        //         {
        //         }


        UIMemberEquipSlot slotinfo = objSelectedSlot.GetComponent<UIMemberEquipSlot>();

        if (slotinfo.item == null)
        {
            objEquipmentDesc.SetActive(false);
            btClose.gameObject.SetActive(false);
            btUnEquip.gameObject.SetActive(false);
        }
        else
        {
            objEquipmentDesc.SetActive(true);
            btClose.gameObject.SetActive(true);
            btUnEquip.gameObject.SetActive(true);
            objEquipmentDesc.GetComponent<UISlotItemInfo>().UpdateItemInfo(slotinfo.item);
        }

        objEquipmentBag.GetComponent<BubbleBag>().GenerateItemsInBag(slotinfo.itemtype);


        
    }


    void OnCloseBag()
    {
        objEquipmentBag.SetActive(false);
        objEquipmentDesc.SetActive(false);
        btClose.gameObject.SetActive(false);
        btUnEquip.gameObject.SetActive(false);

        objEquipmentBag.GetComponent<BubbleBag>().ItemsClear();
    }



    void OnSwitchItem()
    {
        OnCloseBag();
    }
    
    void GenerateNpcInfo()
    {
        Debug.Log("GenerateSlotList");

        // 装备
        for (int i = 0; i < _npcdata.lstEquipments.Count; i++)
        {
            var obj = trsSlotItemRoot.GetChild(i).gameObject as GameObject;
            UIMemberEquipSlot slot = obj.GetComponent<UIMemberEquipSlot>();

            slot.item = _npcdata.lstEquipments[i];
            slot.itemtype = _npcdata.lstEquipments[i].Category;

            if (_npcdata.lstEquipments[i] == null)
            {
                slot.imgIconIfNull.gameObject.SetActive(true);
                slot.imgIcon.gameObject.SetActive(false);
            }
            else
            {
                slot.imgIconIfNull.gameObject.SetActive(false);
                slot.imgIcon.gameObject.SetActive(true);
            }


            obj.GetComponent<Button>().onClick.AddListener(() => { OnOpenBag(obj); });
            
        }

        // 卡片
        for (int i = _npcdata.lstEquipments.Count; i < _npcdata.lstEquipments.Count + _npcdata.lstCards.Count; i++)
        {
            var obj = trsSlotItemRoot.GetChild(i).gameObject as GameObject;
            UIMemberEquipSlot slot = obj.GetComponent<UIMemberEquipSlot>();

            slot.item = _npcdata.lstCards[i - _npcdata.lstEquipments.Count];
            slot.itemtype = _npcdata.lstCards[i - _npcdata.lstEquipments.Count].Category;

            if (_npcdata.lstCards[i - _npcdata.lstEquipments.Count] == null)
            {
                slot.imgIconIfNull.gameObject.SetActive(true);
                slot.imgIcon.gameObject.SetActive(false);
            }
            else
            {
                slot.imgIconIfNull.gameObject.SetActive(false);
                slot.imgIcon.gameObject.SetActive(true);
            }


            obj.GetComponent<Button>().onClick.AddListener(() => { OnOpenBag(obj); });
        }


        // 技能、喜好、性格
        GenerateSlotNature(trsNpcSkillRoot, objSkillSlot, _npcdata.SkillId);
        GenerateSlotNature(trsNpcPreferRoot, objPreferSlot, _npcdata.Favor);
        GenerateSlotNature(trsNpcTraitRoot, objTraitSlot, _npcdata.Character);
    }

	void GenerateSlotNature ( Transform trsroot , GameObject objslot , List<string> lstslotid)
	{

		// 生成格子
		for (int i = trsroot.childCount ; i < lstslotid.Count; i++) 
		{
			var skill = GameObject.Instantiate (objslot) as GameObject;
			skill.name = objslot.name + "_" +i;
			skill.transform.SetParent(trsroot);
			skill.transform.localScale = Vector3.one;

		}

		// 生成内容
		for (int i = 0; i < trsroot.childCount; i++) 
		{
			MemberNatureSlot item = trsroot.GetChild (i).GetComponent<MemberNatureSlot>();
			UISelectableItem sItem = item.gameObject.AddMissingComponent<UISelectableItem>();

			if (i < trsroot.childCount)
			{
				// TODO::技能\喜好\性格 的贴图
				// item.imgIcon.sprite =  DataCenter.Instance.dictItem[lstslotid[i]].IconSprite;
				item.gameObject.SetActive (true);
				sItem.bSelectable = true;
				sItem.cbSelect = OnNatureClicked;

				item.imgBg.gameObject.SetActive (true);


				// 技能是否已经解锁
				if (trsroot == trsNpcSkillRoot) {
					if (_npcdata.SkillUnlocklv [i] >= GameData.nPlayerLv) {
						item.imgLock.gameObject.SetActive (false);
					} else {
						item.imgLock.gameObject.SetActive (true);
					}
				} else {
					item.imgLock.gameObject.SetActive (false);
				}
			}
			else
			{
				item.imgIcon.sprite = null;
				item.gameObject.SetActive (false);
				sItem.bSelectable = false;
				sItem.cbSelect = null;

				item.imgBg.gameObject.SetActive (false);
				item.imgLock.gameObject.SetActive (false);
			}

		}
			
	}


	void OnNatureClicked(bool isSelect, GameObject obj)
	{
		
	}

}
