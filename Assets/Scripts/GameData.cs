using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace smallone
{
    public class GameData
    {
        static public Dictionary<string, GameConfigEntry> dictGameConfs;

        static public List<NPCData> lstUnlockNpcs;

		// 记录所有npc信息
		static public List<NPCData> lstNpcs;
        
        static public List<Item> lstBagItems;
        
		// 所有建筑的obj列表
		static public List<GameObject> lstConstructionObj;

		// 当前选中的建筑id
		static public string strCurConstructionId;

        // 当前选中的建筑Data

        // 玩家当前选中的Npc列表Tag
        static public int nCurNpcTag;

        // 玩家当前等级
        static public int nPlayerLv;

		// 玩家当前经验值
		static public int nPlayerLvExp;

		// 玩家名字
		static public string strPlayerName;



        static public int BagCapacity
        {
            get
            {
                return PlayerPrefs.GetInt(Consts.SAVEKEY_BAGCAPACITY, int.Parse(dictGameConfs[Consts.G_BAG_MIN].Value));
            }
            set
            {
                PlayerPrefs.SetInt(Consts.SAVEKEY_BAGCAPACITY, value);
            }
        }

        static public int Coins
        {
            get
            {
                return PlayerPrefs.GetInt(Consts.SAVEKEY_COINS, int.Parse(dictGameConfs[Consts.INITCOINS].Value));
            }
            set
            {
                PlayerPrefs.SetInt(Consts.SAVEKEY_COINS, value);
            }
        }

        static public int Gems
        {
            get
            {
                return PlayerPrefs.GetInt(Consts.SAVEKEY_GEMS, int.Parse(dictGameConfs[Consts.INITGEMS].Value));
            }
            set
            {
                PlayerPrefs.SetInt(Consts.SAVEKEY_GEMS, value);
            }
        }

		// 获取背包中道具数量
		static public int GetItemHave(Item item)
		{
			int have = 0;

			for (int i = 0; i < GameData.lstBagItems.Count; i++) 
			{
				if (GameData.lstBagItems [i] == item)
				{
					have += item.Have;
				}
			}

			return have;
		}

		// 从背包中删除道具 + 数量
		// TODO:: 单个道具格子堆叠数量上限
		static public void DelItemFromBag(Item item , int count )
		{
			for (int i = 0; i < lstBagItems.Count; i++)
			{
				if ( lstBagItems[i] == item ) 
				{
					lstBagItems[i].Have -= count;
					if(lstBagItems[i].Have == 0)
					{
						lstBagItems.Remove ( lstBagItems[i] );
					}

					return;
				}
			}
		}

		// 添加道具 + 数量
		static public void AddItemToBag(Item item , int count )
		{
			if ( GetItemHave(item) == 0 ) 
			{
				lstBagItems.Add(item);
			} 

			for (int i = 0; i < lstBagItems.Count; i++)
			{
				if ( lstBagItems[i] == item ) 
				{
					lstBagItems[i].Have += count;
					return;
				}
			}
		}


        // 增加NPC任务
        static public void NewNpcTask(NPCData npc)
        {
            // 选择任务

            // 满足条件的数量
            int count = 0;

            Dictionary<string, NPCTask> dicttask = DataCenter.Instance.dictNPCTask;
            

            foreach (string id in dicttask.Keys)
            {
                if (npc.ID == dicttask[id].NpcId)
                {
                    if (GameData.nPlayerLv >= dicttask[id].Lv[0] && GameData.nPlayerLv <= dicttask[id].Lv[1])
                    {
                        if (npc.CurfavorabilityLv >= dicttask[id].Favor[0] && npc.CurfavorabilityLv <= dicttask[id].Favor[1])
                        {
                            if (npc.CurEmotion >= dicttask[id].Emtion[0] && npc.CurEmotion <= dicttask[id].Emtion[1])
                            {
                                count++;
                            }
                        }
                    }
                }
            }

            int rand = UnityEngine.Random.Range(0, count);

			if (count == 0) {
				npc.CurNpcTask = null;
				return;
			}

            count = 0;

            foreach (string id in dicttask.Keys)
            {
                if (npc.ID == dicttask[id].NpcId)
                {
                    if (GameData.nPlayerLv >= dicttask[id].Lv[0] && GameData.nPlayerLv <= dicttask[id].Lv[1])
                    {
                        if (npc.CurfavorabilityLv >= dicttask[id].Favor[0] && npc.CurfavorabilityLv <= dicttask[id].Favor[1])
                        {
                            if (npc.CurEmotion >= dicttask[id].Emtion[0] && npc.CurEmotion <= dicttask[id].Emtion[1])
                            {
                                if (count == rand)
                                {
                                    npc.CurNpcTask = dicttask[id];


									Debug.Log(" NpcId = " + npc.ID + ": NpcTaskCount = " + count);
                                    return;
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


        }



        static public void Init()
        {
            dictGameConfs = SerializationManager.LoadDictFromCSV<GameConfigEntry>("Key", "Configs/GameConfigs");

            //TODO::背包物品应该存档，现在只是道具列表每个来一样
            lstBagItems = new List<Item>();
//            foreach (string id in DataCenter.Instance.dictItem.Keys)
//            {
//                lstBagItems.Add(DataCenter.Instance.dictItem[id]);
//            }

            //临时解锁一位NPC
            lstUnlockNpcs = new List<NPCData>();
            lstUnlockNpcs.Add(DataCenter.Instance.dictNPCData["1"]);



			lstNpcs = new List<NPCData>() ;

			foreach(string id in DataCenter.Instance.dictNPCData.Keys)
			{
				lstNpcs.Add (DataCenter.Instance.dictNPCData[id]) ;
			}

            // 疑问：成员的信息，之后绑定存档
            for(int i = 0 ; i < lstNpcs.Count ; i++ )
			{
				//lstNpcs.Add (DataCenter.Instance.dictNPCData[i.ToString()]) ;
                lstNpcs[i].CurfavorabilityLv = 0;
				lstNpcs[i].CurfavorabilityExp = 0;
				lstNpcs[i].CurPower = lstNpcs[i].Power;
                lstNpcs[i].CurEmotion = 50;
				lstNpcs[i].CurNpcTask = null;
				lstNpcs[i].IsUnlocked = false;

                // 装备信息
                for (int j = 0; j < lstNpcs[i].EquipType.Count; j++)
                {
					lstNpcs[i].lstEquipments.Add(null);

                    // 战力
					if(lstNpcs[i].lstEquipments[j] != null)
                    {
						lstNpcs[i].CurPower += lstNpcs[i].lstEquipments[j].Power;
                    }
                }

                // 卡片信息
				for (int j = 0; j < lstNpcs[i].CardUnlockLv.Count; j++)
                {
                    lstNpcs[i].lstCards.Add(null);
                }
            }


            // 场景中建筑的obj列表
            lstConstructionObj = new List<GameObject>();
            
            // 当前玩家选择的建筑ID
            strCurConstructionId = null;

            // 当前选中的NPCid
            nCurNpcTag = 0;


            // TODO:: 临时
            nPlayerLv = 10;
            
            nPlayerLvExp = 0;
            
            strPlayerName = "SmallOne";

            Gems = 0;

            Coins = 1000;
            
        }
    }
}