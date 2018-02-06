using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using smallone;
using TMPro;

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

    [Header("favorability")]
    public TextMeshProUGUI txtFavorLv;
    public Image imgFavorExp;

    private bool _IsEquipmentInfoShow;
    private NPCData _npcdata;
	private int _nCurSlotSelTag;


    void OnEnable()
    {
        EventCenter.Instance.RegisterGameEvent("ClosePanel", OnCloseSelf);
        // EventCenter.Instance.RegisterGameEvent("OpenInventory", OnBagClicked);
        //EventCenter.Instance.RegisterGameEvent("SwitchEquipment", OnSwitchEquipment);
        EventCenter.Instance.RegisterGameEvent("CloseBag", OnCloseBag);

    }

    void OnDisable()
    {
        if (EventCenter.Instance != null)
        {
            EventCenter.Instance.UnregisterGameEvent("ClosePanel", OnCloseSelf);
            //EventCenter.Instance.UnregisterGameEvent("SwitchEquipment", OnSwitchEquipment);
			EventCenter.Instance.UnregisterGameEvent("CloseBag", OnCloseBag);
            // EventCenter.Instance.UnregisterButtonEvent("OpenInventory", OnBagClicked);
        }

        OnCloseBag();
    }

    void OnCloseSelf()
    {

		OnCloseBag();

        UIPanelManager.Instance.HidePanel("UIPanelEquipment").DoOnHideCompleted((panel) =>
        {
            Debug.Log("closed!");

        });

    }

    protected override void OnPanelShowBegin()
    {


		_nCurSlotSelTag = -1;

		// 得到该界面的npc信息
		_npcdata = GameData.lstNpcs [GameData.nCurNpcTag];

		// 临时方案，以防万一
		if(_npcdata == null){
			_npcdata = GameData.lstNpcs[0];
        }
        OnCloseBag();
        
        // 生成NPC信息
		GenerateNpcInfo();
        
        // 好感度
        txtFavorLv.text = _npcdata.CurfavorabilityLv.ToString(); 

        foreach( string id in DataCenter.Instance.dictNPCFavor.Keys)
        {
            if (DataCenter.Instance.dictNPCFavor[id].ID == _npcdata.ID)
                if (DataCenter.Instance.dictNPCFavor[id].Lv == _npcdata.CurfavorabilityLv)
                    imgFavorExp.fillAmount = (float)_npcdata.CurfavorabilityExp / DataCenter.Instance.dictNPCFavor[id].ExpNeed;
        }

        base.OnPanelShowBegin();

    }

    protected override void OnPanelHideCompleted()
    {
        base.OnPanelHideCompleted();

		for (int i = 0; i < trsSlotItemRoot.childCount; i++) {
			trsSlotItemRoot.GetChild (i).GetComponent<Button> ().onClick.RemoveAllListeners ();
		}


        btUnEquip.onClick.RemoveAllListeners();

        GameData.nCurNpcTag = 0;
    }


    void OnOpenBag(GameObject objSelectedSlot)
    {
		if(objSelectedSlot.GetComponent<UIMemberEquipSlot>().nSlotTag == _nCurSlotSelTag)
		{
			OnCloseBag ();
			return;
		}
		else
		{
			_nCurSlotSelTag = objSelectedSlot.GetComponent<UIMemberEquipSlot> ().nSlotTag;
		}
		
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
			Debug.Log ("?");
            objEquipmentDesc.SetActive(false);
            btClose.gameObject.SetActive(false);
            btUnEquip.gameObject.SetActive(false);
        }
        else
		{
			Debug.Log ("!");
            objEquipmentDesc.SetActive(true);
            btClose.gameObject.SetActive(true);
            btUnEquip.gameObject.SetActive(true);
            objEquipmentDesc.GetComponent<UISlotItemInfo>().UpdateItemInfo(slotinfo.item);
        }

		Debug.Log ("itemtype = " + slotinfo.nslottype);


		objEquipmentBag.SetActive (true);
        objEquipmentBag.GetComponent<BubbleBag>().GenerateItemsInBag(slotinfo.nslottype);

        // 可装备格子监听
		Transform Itemroot = objEquipmentBag.GetComponent<BubbleBag> ().trsSlotItemRoot;
        
		for (int i = 0; i < Itemroot.childCount ; i++) {

			var obj = Itemroot.GetChild (i).gameObject as GameObject;

			obj.AddMissingComponent<Button> ().onClick.AddListener (()=> { OnSwitchItem( objSelectedSlot , obj.name ) ;} );
		}

        // 卸装备监听
        btUnEquip.onClick.AddListener(() => { OnUnEquip(objSelectedSlot); });

    }


    void OnCloseBag()
    {
        objEquipmentBag.SetActive(false);
        objEquipmentDesc.SetActive(false);
        btClose.gameObject.SetActive(false);
        btUnEquip.gameObject.SetActive(false);

		_nCurSlotSelTag = -1;

        objEquipmentBag.GetComponent<BubbleBag>().ItemsClear();
    }


    void OnUnEquip(GameObject Slot)
    {
        int powerchange = int.Parse(txtScore.text);

        // 装备放入背包
        UIMemberEquipSlot slot = Slot.GetComponent<UIMemberEquipSlot>();
        if (slot.item != null)
        {
            GameData.AddItemToBag(slot.item , 1);
            powerchange -= slot.item.Power;
        }

        if ( _nCurSlotSelTag < _npcdata.lstEquipments.Count )
        {
            GameData.lstNpcs[GameData.nCurNpcTag].lstEquipments[_nCurSlotSelTag] = null;
        }
        else
        {
            int j = _nCurSlotSelTag - GameData.lstNpcs[GameData.nCurNpcTag].lstEquipments.Count;
            GameData.lstNpcs[GameData.nCurNpcTag].lstCards[j] = null;
        }

        // npc装备改为null
        GameData.lstNpcs[GameData.nCurNpcTag].lstEquipments[_nCurSlotSelTag] = null;

        txtScore.text = powerchange.ToString();

        Slot.GetComponent<UIMemberEquipSlot>().imgIcon.sprite = null;
        Slot.GetComponent<UIMemberEquipSlot>().imgIcon.gameObject.SetActive(false);
        Slot.GetComponent<UIMemberEquipSlot>().imgIconIfNull.gameObject.SetActive(true);

        OnCloseBag();



    }

    void OnSwitchItem(GameObject Slot , string ItemId)
    {
		int powerchange = int.Parse(txtScore.text) ;

		// 装备放入背包
		UIMemberEquipSlot slot = Slot.GetComponent<UIMemberEquipSlot>();
		if(slot.item != null)
		{
			GameData.AddItemToBag(slot.item , 1);
			powerchange -= slot.item.Power;

		}

		//  穿装备
		for (int i = 0; i < GameData.lstBagItems.Count; i++) 
		{
			if (GameData.lstBagItems[i].ID == ItemId) 
			{
				Debug.Log (ItemId);
				slot.item = GameData.lstBagItems [i];

				if (GameData.lstBagItems [i].Category == ItemType.Equipment) 
				{
					GameData.lstNpcs [GameData.nCurNpcTag].lstEquipments[_nCurSlotSelTag] = GameData.lstBagItems [i];
				} 
				else
				{
					int j = _nCurSlotSelTag - GameData.lstNpcs [GameData.nCurNpcTag].lstEquipments.Count;
					GameData.lstNpcs [GameData.nCurNpcTag].lstCards [j]  = GameData.lstBagItems [i];
				}


				Slot.GetComponent<UIMemberEquipSlot> ().imgIcon.sprite = slot.item.IconSprite;
				Slot.GetComponent<UIMemberEquipSlot> ().imgIcon.gameObject.SetActive (true);
				Slot.GetComponent<UIMemberEquipSlot> ().imgIconIfNull.gameObject.SetActive (false);

				powerchange += slot.item.Power; 

				GameData.DelItemFromBag( slot.item , 1 );

				txtScore.text = powerchange.ToString ();


				Debug.Log ("Npc "+GameData.nCurNpcTag+" Slot = "+ _nCurSlotSelTag + " : " + GameData.lstNpcs [GameData.nCurNpcTag].lstEquipments [_nCurSlotSelTag].ID);


				OnCloseBag();
				return;
			}

		}



    }
    
	// 生成界面
    void GenerateNpcInfo()
    {
        Debug.Log("GenerateSlotList");

		txtNpcName.text = GameData.lstNpcs [GameData.nCurNpcTag].Name;
		int power = DataCenter.Instance.dictNPCData[ GameData.lstNpcs [GameData.nCurNpcTag].ID ].Power;

		Debug.Log ("NpcId : " + _npcdata.ID);

        // 装备
        for (int i = 0; i < _npcdata.lstEquipments.Count; i++)
        {
            var obj = trsSlotItemRoot.GetChild(i).gameObject as GameObject;
            UIMemberEquipSlot slot = obj.GetComponent<UIMemberEquipSlot>();

			slot.item = _npcdata.lstEquipments[i];
			slot.nSlotTag = i;

			Debug.Log (i);

			slot.nslottype = DataCenter.Instance.dictNPCData[ _npcdata.ID ].EquipType[i];

            if (_npcdata.lstEquipments[i] == null)
			{
				Debug.Log ("?");
                slot.imgIconIfNull.gameObject.SetActive(true);
                slot.imgIcon.gameObject.SetActive(false);
            }
            else
			{
				Debug.Log ("!");
                slot.imgIconIfNull.gameObject.SetActive(false);
                slot.imgIcon.gameObject.SetActive(true);

				Debug.Log ("Npc "+ i +" Equip : " + slot.item.ID);

				power += slot.item.Power;
            }


            obj.GetComponent<Button>().onClick.AddListener(() => { OnOpenBag(obj); });
            
        }

        // 卡片
        for (int i = _npcdata.lstEquipments.Count; i < _npcdata.lstEquipments.Count + _npcdata.lstCards.Count; i++)
        {
            var obj = trsSlotItemRoot.GetChild(i).gameObject as GameObject;
            UIMemberEquipSlot slot = obj.GetComponent<UIMemberEquipSlot>();

            slot.item = _npcdata.lstCards[i - _npcdata.lstEquipments.Count];
			slot.nSlotTag = i;

			// TODO::非常临时的写法
			slot.nslottype = 400;

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

		// 战斗力
		txtScore.text = power.ToString();
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
