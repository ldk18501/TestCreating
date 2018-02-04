using System;
using System.Collections.Generic;
using UnityEngine;

namespace smallone
{
    public class GameData
    {
        static public Dictionary<string, GameConfigEntry> dictGameConfs;

        static public List<NPCData> lstUnlockNpcs;
        
        static public List<Item> lstBagItems;
        
		// 所有建筑的obj列表
		static public List<GameObject> lstConstructionObj;

		// 当前选中的建筑id
		static public string strCurConstructionId;

        // 玩家当前选中的NpcId
        static public string strCurNpcId;

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

        static public void Init()
        {
            dictGameConfs = SerializationManager.LoadDictFromCSV<GameConfigEntry>("Key", "Configs/GameConfigs");

            //TODO::背包物品应该存档，现在只是道具列表每个来一样
            lstBagItems = new List<Item>();
            foreach (string id in DataCenter.Instance.dictItem.Keys)
            {
                lstBagItems.Add(DataCenter.Instance.dictItem[id]);
            }

            //临时解锁一位NPC
            lstUnlockNpcs = new List<NPCData>();
            lstUnlockNpcs.Add(DataCenter.Instance.dictNPCData["1"]);


            // 疑问：成员的信息，之后绑定存档
            for(int i = 0 ; i < DataCenter.Instance.dictNPCData.Count; i++ )
            {
                lstUnlockNpcs[i].CurfavorabilityLv = 1;
                lstUnlockNpcs[i].CurfavorabilityExp = 0;
                lstUnlockNpcs[i].CurPower = lstUnlockNpcs[i].Power;

                // 装备信息
                for (int j = 0; j < lstUnlockNpcs[i].lstEquipments.Count; j++)
                {
                    lstUnlockNpcs[i].lstEquipments[j] = null;

                    // 战力
                    if(lstUnlockNpcs[i].lstEquipments[i] != null)
                    {
                        lstUnlockNpcs[i].CurPower += lstUnlockNpcs[i].lstEquipments[i].Power;
                    }
                }

                // 卡片信息
                for (int j = 0; j < lstUnlockNpcs[i].lstEquipments.Count; j++)
                {
                    lstUnlockNpcs[i].lstCards[j] = null;
                }
            }


            // 场景中建筑的obj列表
            lstConstructionObj = new List<GameObject>();
            
            // 当前玩家选择的建筑ID
            strCurConstructionId = null;

            // 当前选中的NPCid
            strCurNpcId = null;


            // TODO:: 临时
            nPlayerLv = 10;
            
            nPlayerLvExp = 0;
            
            strPlayerName = "SmallOne";
            
        }
    }
}