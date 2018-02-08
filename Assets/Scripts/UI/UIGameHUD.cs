using UnityEngine;
using UnityEngine.UI;
using DoozyUI;
using smallone;
using TMPro;


public class UIGameHUD : UIPanel
{
    [Header("TestInfo")]
    public int nTestPlyaerLv = 1;

	// 玩家等级信息
    public int nPlayerLvlInfoCount = 3;
    public Transform trsPlayerLvlInfoRoot;
    public GameObject objSlotItem;
	public TextMeshProUGUI tmPlayerLvInfoText;

	// 成员列表
    public int nHeroCount = 6;
    public Transform trsHeroListRoot;
    public GameObject objRoleSlot; 

    public UIElement elePlayerLvlInfo;

	// 游戏币
    public Text mCoinNum;
	public Text mDiamondNum;

    private LevelMain _mainLevel;

	// 玩家信息
	public Image imgExpFill;
	public TextMeshProUGUI tmPlayerLv;
	public TextMeshProUGUI tmPlayerName;


    void OnEnable()
    { 
        EventCenter.Instance.RegisterGameEvent("OpenInventory", OnBagClicked);

        EventCenter.Instance.RegisterGameEvent("OpenPlayerLvlInfo", OnPlayerLvlInfoShow);
        EventCenter.Instance.RegisterGameEvent("ClosePlayerLvlInfo", OnPlayerLvlInfoHide);

        GameData.nPlayerLv = nTestPlyaerLv;

        StartGame();
    }

    void OnDisable()
    {
        if (EventCenter.Instance != null)
        {
            EventCenter.Instance.UnregisterGameEvent("OpenInventory", OnBagClicked);

            EventCenter.Instance.UnregisterGameEvent("OpenPlayerLvlInfo", OnPlayerLvlInfoShow);
            EventCenter.Instance.UnregisterGameEvent("ClosePlayerLvlInfo", OnPlayerLvlInfoHide);
        }
    }

    void StartGame()
    {
        _mainLevel = LevelManager.Instance.MainLevel as LevelMain;
        _mainLevel.StartGameLogic();
        
        GenerateHeroList();
        GeneratePlayerLvlInfoList();
		UpdatePlayerLv ();
    }

    protected override void OnPanelRepaint()
    {
        base.OnPanelRepaint();

        mCoinNum.text = GameData.Coins.ToString();
		mDiamondNum.text = GameData.Gems.ToString();

        // 玩家等级经验条
        imgExpFill.fillAmount = (float)GameData.nPlayerLvExp / DataCenter.Instance.dictPlyerLvlData[GameData.nPlayerLv.ToString()].RequireExp;

        // 玩家等级txt
        tmPlayerLv.text = GameData.nPlayerLv.ToString();

        // 玩家头像列表
        GenerateHeroList();
    }

    protected override void OnPanelShowBegin()
    {
        base.OnPanelShowBegin();
		mCoinNum.text = GameData.Coins.ToString();
		mDiamondNum.text = GameData.Gems.ToString ();
        // UIManager.SoundCheck();
    }

    protected override void OnPanelHideBegin()
    {
        base.OnPanelHideBegin();
    }

    protected override void OnPanelShowCompleted()
    {
        base.OnPanelShowCompleted();
    }

    protected override void OnPanelHideCompleted()
    {
        base.OnPanelHideCompleted();
    }

    void GenerateHeroList()
    {
        for (int i = 0; i < trsHeroListRoot.childCount; i++)
        {
            GameObject.Destroy( trsHeroListRoot.GetChild(i).gameObject );
        }
        

        for (int i = 0; i < GameData.lstNpcs.Count; i++)
		{
            if (GameData.lstNpcs[i].UnlockLv <= GameData.nPlayerLv)
            {
                var item = GameObject.Instantiate(objRoleSlot) as GameObject;


                item.name = GameData.lstNpcs[i].Name;
                item.transform.SetParent(trsHeroListRoot);
				item.transform.localScale = Vector3.one;
				Debug.Log ("NpcTag" + i);
                UIRoleInfo role = item.GetComponent<UIRoleInfo>();

                role.nRoleTag = i;
                role.btRole.onClick.AddListener(() => { OnHeroClicked(item); });
                role.btMission.onClick.AddListener(() => { OnMissionClicked(item); });

                if(GameData.lstNpcs[i].CurNpcTask == null)
                {
                    GameData.NewNpcTask(GameData.lstNpcs[i]);

                }


                // NPC任务道具需求显示
                string itemid = null;
                bool IsAlready = true;
                int count = GameData.lstNpcs[i].CurNpcTask.Require.Count;

                for (int j = 0; j < count; j++)
                {
                    itemid = GameData.lstNpcs[i].CurNpcTask.Require[j].strId;
                    
                    if (GameData.GetItemHave( DataCenter.Instance.dictItem[itemid] ) < GameData.lstNpcs[i].CurNpcTask.Require[j].nCount)
                    {
                        role.imgMissionIcom.sprite = DataCenter.Instance.dictItem[itemid].IconSprite;
                        IsAlready = false;
                        break;
                    }
                }

                if (IsAlready)
                {
                    role.imgMissionIcom.sprite = DataCenter.Instance.dictItem[GameData.lstNpcs[i].CurNpcTask.Require[count-1].strId].IconSprite;
                }



                Debug.Log("GameData.lstNpcs.add = " + item.GetComponent<UIRoleInfo>().nRoleTag);
            }
        }
    }

    void GeneratePlayerLvlInfoList()
    {
		// TODO::玩家等级需要判断最高玩家等级
		string playerlvl = ( GameData.nPlayerLv + 1 ).ToString();

		PlayerLvlData playerlvldata = DataCenter.Instance.dictPlyerLvlData [playerlvl];

		// 玩家升级信息：解锁建筑
		for (int i = 0; i < playerlvldata.TableUnlock.Count; i++)
        {
            var item = GameObject.Instantiate(objSlotItem) as GameObject;
			item.name = DataCenter.Instance.dictBuilding [playerlvldata.TableUnlock [i]].Name;
            item.transform.SetParent(trsPlayerLvlInfoRoot);
			item.transform.localScale = Vector3.one;
			item.GetComponent<UISlotItem> ().imgIcon.sprite = DataCenter.Instance.dictBuilding [playerlvldata.TableUnlock [i]].IconSprite;

			// TODO::品质
			item.GetComponent<UISlotItem> ().ShowQuality = false;
			item.GetComponent<UISlotItem> ().ShowScore = false;
            item.GetComponent<UISlotItem>().ShowCount = false;
            item.GetComponent<UISlotItem>().ShowScore = false;
        }

        // 玩家升级信息：解锁任务
        for (int i = 0; i < playerlvldata.TaskUnlock.Count; i++)
		{
			var item = GameObject.Instantiate(objSlotItem) as GameObject;
			item.name = DataCenter.Instance.dictBuildingTask[ playerlvldata.TaskUnlock [i] ].Name;
			item.transform.SetParent(trsPlayerLvlInfoRoot);
			item.transform.localScale = Vector3.one;

			string productid = DataCenter.Instance.dictBuildingTask [playerlvldata.TaskUnlock [i]].Product.strId;
			item.GetComponent<UISlotItem> ().imgIcon.sprite = DataCenter.Instance.dictItem [productid].IconSprite;

			// TODO::品质
			// 疑问：为啥界面中还在显示
			item.GetComponent<UISlotItem> ().ShowQuality = false;
			item.GetComponent<UISlotItem> ().ShowScore = false;
            item.GetComponent<UISlotItem>().ShowCount = false;
            item.GetComponent<UISlotItem>().ShowScore = false;
        }

        // 标题信息
		tmPlayerLvInfoText.text = "Next Lv : " + playerlvldata.Lv.ToString() ; 

    }


	void UpdatePlayerLv()
	{
		// 玩家等级经验条
		imgExpFill.fillAmount = (float) GameData.nPlayerLvExp / DataCenter.Instance.dictPlyerLvlData [GameData.nPlayerLv.ToString()].RequireExp;

		// 玩家等级txt
		tmPlayerLv.text = GameData.nPlayerLv.ToString();

		// 玩家名字
		tmPlayerName.text = GameData.strPlayerName;
	}



    /// <summary>
    /// 背包
    /// </summary>
    void OnBagClicked()
    {
        UIPanelManager.Instance.ShowPanel("UIPanelInventory").DoOnShowCompleted((panel) =>
        {
            Debug.Log("tada!");
        });
    }

    void OnHeroClicked(GameObject obj)
    {

		GameData.nCurNpcTag = obj.GetComponent<UIRoleInfo>().nRoleTag;

		Debug.Log("GameData.nCurNpcTag = " + GameData.nCurNpcTag);

        UIPanelManager.Instance.ShowPanel("UIPanelEquipment").DoOnShowCompleted((panel) =>
        {


        });
    }

    void OnMissionClicked(GameObject obj)
    {
        
        GameData.nCurNpcTag = obj.GetComponent<UIRoleInfo>().nRoleTag;

        Debug.Log("GameData.nCurNpcTag = " + GameData.nCurNpcTag);

        UIPanelManager.Instance.ShowPanel("UIPanelMission").DoOnShowCompleted((panel) =>
        {
            Debug.Log(obj.name);
        });
    }

    void OnPlayerLvlInfoShow()
    {
        elePlayerLvlInfo.Show(false);
    }

    void OnPlayerLvlInfoHide()
    {
        elePlayerLvlInfo.Hide(false);
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
        }
    }
}
