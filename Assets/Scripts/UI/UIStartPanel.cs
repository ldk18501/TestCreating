using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class UIStartPanel : UIPanel
{

	void Start()
	{

	}

    void OnEnable()
    {
        EventCenter.Instance.RegisterGameEvent("EnterGame", OnEnterGame);
    }

    void OnDisable()
    {
        if (EventCenter.Instance != null)
        {
            EventCenter.Instance.UnregisterGameEvent("EnterGame", OnEnterGame);
        }
    }

    void OnEnterGame()
    {
        UIPanelManager.Instance.HidePanel(this).DoOnHideCompleted((panel) =>
            {
                UIPanelManager.Instance.ShowPanel("UIGameHUD");
                UIPanelManager.Instance.HidePanel("UICover");
            });
    }
}
