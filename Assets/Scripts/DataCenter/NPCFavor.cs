﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace smallone
{
    public class NPCFavor : ICSVDeserializable
    {
        protected string _strID;
        protected int _nLv;
        protected int _nExpNeed;
        protected List<ItemPair> _lstReward;
        protected List<int> _lstTableUnlock;
        protected List<int> _lstEquipUnlock;
        protected List<int> _lstCardUnlock;


        public string ID
        {
            get { return _strID; }
        }

        public int Lv
        {
            get { return _nLv; }
        }

        public int ExpNeed
        {
            get { return _nExpNeed; }
        }

        public List<ItemPair> Reward
        {
            get { return _lstReward; }
        }

        public List<int> TableUnlock
        {
            get { return _lstTableUnlock; }
        }

        public List<int> EquipUnlock
        {
            get { return _lstEquipUnlock; }
        }

        public List<int> CardUnlock
        {
            get { return _lstCardUnlock; }
        }


        public virtual void CSVDeserialize(Dictionary<string, string[]> data, int index)
        {
            _strID = data["NpcId"][index];
            _nLv = int.Parse(data["Lv"][index]);
            _nExpNeed = int.Parse(data["Exp"][index]);

            string reward = data["Reward"][index];
            if (!string.IsNullOrEmpty(reward) && reward != "-1")
            {
                string[] multi = reward.Split('|');
                for (int i = 0; i < multi.Length; i++)
                {
                    string[] item = multi[i].Split('=');
                    _lstReward.Add( new ItemPair(int.Parse(item[0]), int.Parse(item[1])) );
                }
            }

            string tableunlock = data["TableUnlock"][index];
            if (!string.IsNullOrEmpty(tableunlock) && tableunlock != "-1")
            {
                string[] multi = tableunlock.Split('|');
                for (int i = 0; i < multi.Length; i++)
                {
                    _lstTableUnlock.Add(int.Parse(multi[i]));
                }
            }

            string equipunlock = data["EquipUnlock"][index];
            if (!string.IsNullOrEmpty(equipunlock) && equipunlock != "-1")
            {
                string[] multi = equipunlock.Split('|');
                for (int i = 0; i < multi.Length; i++)
                {
                    _lstEquipUnlock.Add(int.Parse(multi[i]));
                }
            }

            string cardunlock = data["CardUnlock"][index];
            if (!string.IsNullOrEmpty(cardunlock) && cardunlock != "-1")
            {
                string[] multi = cardunlock.Split('|');
                for (int i = 0; i < multi.Length; i++)
                {
                    _lstCardUnlock.Add(int.Parse(multi[i]));
                }
            }

        }

    }
}

