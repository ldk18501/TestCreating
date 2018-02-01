using System;
using System.Collections.Generic;
using UnityEngine;

namespace smallone
{
    public class GameData
    {
        static public Dictionary<string, GameConfigEntry> dictGameConfs;

        static public List<Item> lstBagItems;

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
        }
    }
}