using UnityEngine.UI;
using DoozyUI;
using smallone;
using UnityEngine;

public class UIGameHUD : UIPanel
{
    public int nHeroCount = 6;
    public Transform trsHeroListRoot;
    public GameObject objRoleSlot; 
    public UIElement elePlayerLvlInfo;
    public Text mCoinNum;

    void OnEnable()
    {
        EventCenter.Instance.RegisterGameEvent("OpenInventory", OnBagClicked);

        EventCenter.Instance.RegisterGameEvent("OpenPlayerLvlInfo", OnPlayerLvlInfoShow);
        EventCenter.Instance.RegisterGameEvent("ClosePlayerLvlInfo", OnPlayerLvlInfoHide);
        GenerateHeroList();
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

    protected override void OnPanelRepaint()
    {
        base.OnPanelRepaint();
        mCoinNum.text = GameData.Coins.ToString();
    }

    protected override void OnPanelShowBegin()
    {
        base.OnPanelShowBegin();
        mCoinNum.text = GameData.Coins.ToString();
        UIManager.SoundCheck();
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
        for (int i = 0; i < nHeroCount; i++)
        {
            var item = GameObject.Instantiate(objRoleSlot) as GameObject;
            item.name = objRoleSlot.name + "_" + i;
            item.transform.SetParent(trsHeroListRoot);
            item.transform.localScale = Vector3.one;
            item.GetComponent<UIRoleInfo>().btRole.onClick.AddListener(() => { OnHeroClicked(item); });
            item.GetComponent<UIRoleInfo>().btMission.onClick.AddListener(() => { OnMissionClicked(item); });
        }
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
        UIPanelManager.Instance.ShowPanel("UIPanelEquipment").DoOnShowCompleted((panel) =>
        {
            Debug.Log(obj.name);
        });
    }

    void OnMissionClicked(GameObject obj)
    {
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
