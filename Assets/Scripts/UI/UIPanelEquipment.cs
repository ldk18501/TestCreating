using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using smallone;

public class UIPanelEquipment : UIPanel
{
    public int nSlotItemCount = 20;
    public Transform trsSlotItemRoot;
    public GameObject objSlotItem;


    public GameObject objEquipmentDesc;
    public GameObject objEquipmentBag;

    private bool _IsEquipmentInfoShow;
	private NPCData _npcdata;

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


    void OnEnable()
    {
        EventCenter.Instance.RegisterGameEvent("ClosePanel", OnCloseSelf);
        // EventCenter.Instance.RegisterGameEvent("OpenInventory", OnBagClicked);
        EventCenter.Instance.RegisterGameEvent("SwitchEquipment", OnSwitchEquipment);
        EventCenter.Instance.RegisterGameEvent("CloseEquipmentInfo", OnCloseEquipmentInfo);

    }

    void OnDisable()
    {
        if (EventCenter.Instance != null)
        {
            EventCenter.Instance.UnregisterGameEvent("ClosePanel", OnCloseSelf);
            EventCenter.Instance.UnregisterGameEvent("SwitchEquipment", OnSwitchEquipment);
            EventCenter.Instance.UnregisterGameEvent("CloseEquipmentInfo", OnCloseEquipmentInfo);
            // EventCenter.Instance.UnregisterButtonEvent("OpenInventory", OnBagClicked);
        }

        OnCloseEquipmentInfo();
    }

    void OnCloseSelf()
    {
        UIPanelManager.Instance.HidePanel("UIPanelEquipment").DoOnHideCompleted((panel) =>
        {
            Debug.Log("closed!");
        });

        OnCloseEquipmentInfo();
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

		GenerateSlotNature ( trsNpcSkillRoot , objSkillSlot , _npcdata.SkillId );
		GenerateSlotNature ( trsNpcPreferRoot , objPreferSlot , _npcdata.Favor );
		GenerateSlotNature ( trsNpcTraitRoot , objTraitSlot , _npcdata.Character );

		OnCloseEquipmentInfo();
		GenerateSlotList();
    }

    protected override void OnPanelHideCompleted()
    {
        base.OnPanelHideCompleted();

        GameData.strCurNpcId = null;
    }


    void OnSwitchEquipment()
    {
//         // 如果点击了同一个slot，则关闭
//         if()
//         {
// 
//         }
//         // 打开换装信息
//         else
//         {
//         }

        if(_IsEquipmentInfoShow)
        {
            _IsEquipmentInfoShow = false;
            ShowInfo(_IsEquipmentInfoShow);
        }
        else
        {
            _IsEquipmentInfoShow = true;
            ShowInfo(_IsEquipmentInfoShow);
        }
    }


    void OnCloseEquipmentInfo()
    {
        _IsEquipmentInfoShow = false;
        ShowInfo(_IsEquipmentInfoShow);
    }


    void OnSwitch()
    {
        _IsEquipmentInfoShow = false;
        ShowInfo(_IsEquipmentInfoShow);
    }

    void ShowInfo(bool IsShow)
    {
        objEquipmentBag.SetActive(IsShow);
        objEquipmentDesc.SetActive(IsShow);
    }


    void GenerateSlotList()
    {

		// 装备信息
		for (int i = 0; i < _npcdata.lstEquipments.Count; i++)
		{
			var item = GameObject.Instantiate(objSlotItem) as GameObject;
			item.name = objSlotItem.name + "_" + i;
			item.transform.SetParent(trsSlotItemRoot);
			item.transform.localScale = Vector3.one;
			item.gameObject.AddMissingComponent<Button>().onClick.AddListener(() => { OnSwitch(); });
		}

		// 卡片信息
		for (int i = 0; i < _npcdata.lstEquipments.Count; i++)
		{
			var item = GameObject.Instantiate(objSlotItem) as GameObject;
			item.name = objSlotItem.name + "_" + i;
			item.transform.SetParent(trsSlotItemRoot);
			item.transform.localScale = Vector3.one;
			item.gameObject.AddMissingComponent<Button>().onClick.AddListener(() => { OnSwitch(); });
		}
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
