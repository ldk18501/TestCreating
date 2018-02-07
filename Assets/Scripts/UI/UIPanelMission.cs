using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using smallone;

public class UIPanelMission : UIPanel
{

    public Transform trsNeedListRoot;
    public GameObject objNeedSlot;
    private NPCData _npcData;

    public Transform trsRewardRoot;
    public GameObject objRewardItem;

    private bool _bIsTaskOk;

    void OnEnable()
    {
        EventCenter.Instance.RegisterGameEvent("ClosePanel", OnCloseSelf);
        EventCenter.Instance.RegisterGameEvent("RefuseTask", OnRefuseTask);
        EventCenter.Instance.RegisterGameEvent("MissionClear", OnMissionClear);

    }

    void OnDisable()
    {
        if (EventCenter.Instance != null)
        {
            EventCenter.Instance.UnregisterGameEvent("ClosePanel", OnCloseSelf);
            EventCenter.Instance.UnregisterGameEvent("RefuseTask", OnRefuseTask);
            EventCenter.Instance.UnregisterGameEvent("MissionClear", OnMissionClear);
        }
    }

    void OnCloseSelf()
    {
        UIPanelManager.Instance.HidePanel("UIPanelMission").DoOnHideCompleted((panel) =>
        {
            Debug.Log("closed!");
            
        });
    }
    

    void OnRefuseTask()
    {
        UpdateNpcTask();

        OnCloseSelf();
    }

    void OnMissionClear()
    {
        // 粗暴做法：检查道具数量是否够

        if (_bIsTaskOk)
        {
			// 扣除道具
			for (int i = 0; i < _npcData.CurNpcTask.Require.Count; i++)
			{
				Item it = DataCenter.Instance.dictItem[ _npcData.CurNpcTask.Require [i].strId ];

				GameData.DelItemFromBag ( it , _npcData.CurNpcTask.Require[i].nCount  );
			}


            // TODO::临时，任务奖励
            for (int i = 0; i < _npcData.CurNpcTask.Reward.Count; i++)
            {
                // 钱
                if (DataCenter.Instance.dictItem[_npcData.CurNpcTask.Reward[i].strId].ID == "1")
                {
                    GameData.nCoin += _npcData.CurNpcTask.Reward[i].nCount;
                }

                // 水晶
                if (DataCenter.Instance.dictItem[_npcData.CurNpcTask.Reward[i].strId].ID == "2")
                {
                    GameData.nGems += _npcData.CurNpcTask.Reward[i].nCount;
                }

                // 好感度
                if (DataCenter.Instance.dictItem[_npcData.CurNpcTask.Reward[i].strId].Type == 2 && DataCenter.Instance.dictItem[_npcData.CurNpcTask.Reward[i].strId].ID != "5")
                {
                    // 检测是否最高级
                    int expneed = 0;    
                    bool isLvMax = true;

                    int nextlv = _npcData.CurfavorabilityLv + 1;

                    Dictionary<string, NPCFavor> dictfavor = DataCenter.Instance.dictNPCFavor;

                    foreach (string id in dictfavor.Keys)
                    {
                        if(dictfavor[id].NpcID == _npcData.ID && dictfavor[id].Lv == _npcData.CurfavorabilityLv )
                        {
                            expneed = dictfavor[id].ExpNeed;
                        }

                        if (dictfavor[id].NpcID == _npcData.ID && dictfavor[id].Lv == nextlv )
                        {
                            isLvMax = false;
                        }
                    }

                    // 增加好感
                    // 如果最大等级
                    if( isLvMax )
                    {
                        GameData.lstNpcs[GameData.nCurNpcTag].CurfavorabilityExp = Mathf.Min(
                            GameData.lstNpcs[GameData.nCurNpcTag].CurfavorabilityExp + _npcData.CurNpcTask.Reward[i].nCount
                            , expneed);
                    }
                    else
                    {
                        GameData.lstNpcs[GameData.nCurNpcTag].CurfavorabilityExp += _npcData.CurNpcTask.Reward[i].nCount;

                        if( GameData.lstNpcs[GameData.nCurNpcTag].CurfavorabilityExp >= expneed )
                        {
                            GameData.lstNpcs[GameData.nCurNpcTag].CurfavorabilityExp -= expneed;
                            GameData.lstNpcs[GameData.nCurNpcTag].CurfavorabilityLv ++ ;
                        }

                    }
                    
                }


                // 经验值
                if (DataCenter.Instance.dictItem[_npcData.CurNpcTask.Reward[i].strId].ID == "5")
                {
                    // 检测是否最高级
                    int expneed = 0;
                    bool isLvMax = true;

                    int nextlv = GameData.nPlayerLv + 1;

                    Dictionary<string, PlayerLvlData> dictPlayerData = DataCenter.Instance.dictPlyerLvlData;

                    foreach (string id in dictPlayerData.Keys)
                    {
                        if (dictPlayerData[id].Lv == GameData.nPlayerLv)
                        {
                            expneed = dictPlayerData[id].RequireExp;
                        }

                        if (dictPlayerData[id].Lv == nextlv)
                        {
                            isLvMax = false;
                        }
                    }

                    if (isLvMax)
                    {
                        GameData.nPlayerLvExp = Mathf.Min(
                            GameData.nPlayerLvExp + _npcData.CurNpcTask.Reward[i].nCount
                            , expneed);
                    }
                    else
                    {
                        GameData.nPlayerLvExp += _npcData.CurNpcTask.Reward[i].nCount;

                        if (GameData.nPlayerLvExp >= expneed)
                        {
                            GameData.nPlayerLvExp -= expneed;
                            GameData.nPlayerLv ++ ;
                        }
                    }

                }

            }

            Debug.Log("----------------------------------------------------------------------");
            Debug.Log("NpcID = " + _npcData.ID + ", NpcFavorLv = " + _npcData.CurfavorabilityLv + ", NpcFavorExp = " + _npcData.CurfavorabilityExp);
            Debug.Log("PlayerLv = " + GameData.nPlayerLv + ", PlayerExp = " + GameData.nPlayerLvExp);

            UpdateNpcTask();
            OnCloseSelf();
        }
    }
    

    protected override void OnPanelShowBegin()
    {
        base.OnPanelShowBegin();

        // 获取当前NPC信息
        _npcData = GameData.lstNpcs[GameData.nCurNpcTag];

        _bIsTaskOk = true;

        // 如果没有任务，刷新一个任务
        if(_npcData.CurNpcTask == null)
        {
            UpdateNpcTask();
        }

        InitNeedList();
    }

    protected override void OnPanelHideCompleted()
    {
        base.OnPanelHideCompleted();

        for (int i = 0; i < trsNeedListRoot.childCount; i++)
        {
            GameObject.Destroy(trsNeedListRoot.GetChild(i).gameObject);
        }

        for (int i = 0; i < trsRewardRoot.childCount; i++)
        {
            GameObject.Destroy(trsRewardRoot.GetChild(i).gameObject);
        }
    }

    void InitNeedList()
    {
        
        NPCTask task = _npcData.CurNpcTask;
        // 生成需求道具
        for (int i = 0; i < task.Require.Count; i++)
        {
            var need = GameObject.Instantiate(objNeedSlot) as GameObject;
            need.name = objNeedSlot.name;
            need.transform.SetParent(trsNeedListRoot);
            need.transform.localScale = Vector3.one;

            need.AddMissingComponent<Button>().onClick.AddListener(() => { OnItemClicked(objNeedSlot); });

            Debug.Log("NpcId = " + GameData.nCurNpcTag + "taskId = " + task.ID + " :  task.requireid : " + task.Require[i].strId);

            Item item = DataCenter.Instance.dictItem[task.Require[i].strId];
            need.GetComponent<SlotMissionNeed>().UpdataItemInfo(item, task.Require[i].nCount);

            // TODO::太粗暴，要改，检查需求数量是否足够
            int count = 0;
            for (int j = 0; j < GameData.lstBagItems.Count; j++)
            {
                if( GameData.lstBagItems[j].ID == task.Require[i].strId )
                {
                    count++;
                }
            }
            if(count < task.Require[i].nCount)
            {
                _bIsTaskOk = false;
            }

        }


        // 生成奖励
        for (int i = 0; i < task.Reward.Count; i++)
        {
            var need = GameObject.Instantiate(objRewardItem) as GameObject;
            need.name = objRewardItem.name;
            need.transform.SetParent(trsRewardRoot);
            need.transform.localScale = Vector3.one;

            Item item = DataCenter.Instance.dictItem[task.Reward[i].strId];

            Debug.Log("NpcId = " + GameData.nCurNpcTag + "taskId = " + task.ID + " :  task.requireid : " + task.Reward[i].strId);

            need.GetComponent<SlotNpcTaskReward>().imgIcon.sprite = item.IconSprite;
            need.GetComponent<SlotNpcTaskReward>().txtNum.text = task.Reward[i].nCount.ToString();
        }


    }


    void UpdateNpcTask()
    {
        // 选择任务

        // 满足条件的数量
        int count = 0;

        Dictionary<string, NPCTask> dicttask = DataCenter.Instance.dictNPCTask;

        foreach (string id in dicttask.Keys)
        {
            if (_npcData.ID == dicttask[id].NpcId)
            {
                if (GameData.nPlayerLv >= dicttask[id].Lv[0] && GameData.nPlayerLv <= dicttask[id].Lv[1])
                {
                    if (_npcData.CurfavorabilityLv >= dicttask[id].Favor[0] && _npcData.CurfavorabilityLv <= dicttask[id].Favor[1])
                    {
                        if (_npcData.CurEmotion >= dicttask[id].Emtion[0] && _npcData.CurEmotion <= dicttask[id].Emtion[1])
                        {
                            count++;
                        }
                    }
                }
            }
        }

        int rand = Random.Range(0, count);
        count = 0;
        
        foreach (string id in dicttask.Keys)
        {
            if (_npcData.ID == dicttask[id].NpcId)
            {
                if (GameData.nPlayerLv >= dicttask[id].Lv[0] && GameData.nPlayerLv <= dicttask[id].Lv[1])
                {
                    if (_npcData.CurfavorabilityLv >= dicttask[id].Favor[0] && _npcData.CurfavorabilityLv <= dicttask[id].Favor[1])
                    {
                        if (_npcData.CurEmotion >= dicttask[id].Emtion[0] && _npcData.CurEmotion <= dicttask[id].Emtion[1])
                        {
                            if (count == rand)
                            {
                                GameData.lstNpcs[GameData.nCurNpcTag].CurNpcTask = dicttask[id];
                                
                                break;
                            }
                            else
                            {
                                count++;
                            }
                        }
                    }
                }
            }
        }

        Debug.Log(" NpcId = " + _npcData.ID +  ": NpcTaskCount = " + count );

    }


    void OnItemClicked(GameObject obj)
    {

    }
}
