﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using smallone;

public class UIStartPanel : UIPanel
{
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
        (LevelManager.Instance.MainLevel as LevelMain).StartGameLogic();
        UIPanelManager.Instance.HidePanel(this).DoOnHideCompleted((panel) =>
            {
                UIPanelManager.Instance.ShowPanel("UIGameHUD");
                UIPanelManager.Instance.HidePanel("UICover");
            });
    }
}
