using System;
using System.Collections.Generic;
using UnityEngine;

namespace smallone
{
    public class GameData
    {
        static public Dictionary<string, string> GameConfigs;

        static public List<Item> BagList;

        static public int Coins
        {
            get
            {
                return PlayerPrefs.GetInt(Consts.SAVEKEY_COINS, int.Parse(GameConfigs["InitCoins"]));
            }
            set
            {
                PlayerPrefs.SetInt(Consts.SAVEKEY_COINS, value);
            }
        }

        static public void Init()
        {
            GameConfigs = new Dictionary<string, string>();
            List<GameConfigEntry> entryList = SerializationManager.LoadFromCSV<GameConfigEntry>("Configs/GameConfigs");
            for (int i = 0; i < entryList.Count; ++i)
            {
                GameConfigs.Add(entryList[i].Key, entryList[i].Value);
            }

            BagList = new List<Item>();
            foreach (string id in DataCenter.Instance.dictItem.Keys)
            {
                BagList.Add(DataCenter.Instance.dictItem[id]);
            }
            // 
            //         FoodItemList = SerializationManager.LoadFromCSV<FoodItem>("Data/FoodItems");
        }
    }
}