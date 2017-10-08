using UnityEngine.UI;
using DoozyUI;
using UncleBear;

public class UIGameHUD : UIPanel {

    public Text mCoinNum;

    void OnEnable()
    {
        EventCenter.Instance.RegisterButtonEvent("HomeButton", OnHomeBtnClicked);
    }

    void OnDisable()
    {
        if (EventCenter.Instance != null)
        {
            EventCenter.Instance.UnregisterButtonEvent("HomeButton", OnHomeBtnClicked);
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

    void OnHomeBtnClicked()
    {
        (UIPanelManager.Instance.HidePanel("UIGameHUD") as UIPanel).HideSubElements("UIHomeButton");

        UIPanelManager.Instance.ShowPanel("UICover").DoOnShowCompleted((panel) =>
        {
            UIPanelManager.Instance.ShowPanel("UIStartPanel");
            UIManager.isSoundOn = false;
        });
    }

	void OnApplicationPause(bool pauseStatus) 
	{		
		if (!pauseStatus) 
		{
		}
	}
}
