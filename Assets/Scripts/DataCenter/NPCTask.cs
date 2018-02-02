﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace smallone
{
    public class NPCTask : ICSVDeserializable
    {
        protected string _strID;
        protected int _nNpcId;
        protected List<int> _lstLv;
        protected string _strName;
        protected string _strInfo;
        protected List<int> _lstFavor;
        protected List<int> _lstEmtion;
        protected List<ItemPair> _lstRequire;
        protected List<ItemPair> _lstReward;
        
        public string ID
        {
            get { return _strID; }
        }

        protected int NpcId
        {
            get { return _nNpcId; }
        }

        protected List<int> Lv
        {
            get { return _lstLv; }
        }

        protected string Name
        {
            get { return _strName; }
        }

        protected string Info
        {
            get { return _strInfo; }
        }

        protected List<int> Favor
        { 
            get { return _lstFavor; }
        }

        protected List<int> Emtion
        {
            get { return _lstEmtion; }
        }

        protected List<ItemPair> Require
        {
            get { return _lstRequire; }
        }

        protected List<ItemPair> Reward
        {
            get { return _lstReward; }
        }

        public virtual void CSVDeserialize(Dictionary<string, string[]> data, int index)
        {
            _strID = data["Id"][index];
            _nNpcId = int.Parse(data["NpcId"][index]);

            _lstLv = new List<int>();
            string lv = data["Lv"][index];
            if (!string.IsNullOrEmpty(lv) && lv != "-1")
            {
                string[] multi = lv.Split('|');
                for (int i = 0; i < multi.Length; i++)
                {
                    _lstLv.Add(int.Parse(multi[i]));
                }
            }

            _strName = data["Name"][index];
            _strInfo = data["Info"][index];

            _lstFavor = new List<int>();
            string emtionmin = data["Favor"][index];
            if (!string.IsNullOrEmpty(emtionmin) && emtionmin != "-1")
            {
                string[] multi = emtionmin.Split('|');
                for (int i = 0; i < multi.Length; i++)
                {
                    _lstFavor.Add(int.Parse(multi[i]));
                }
            }

            _lstEmtion = new List<int>();
            string emtionmax = data["Emtion"][index];
            if (!string.IsNullOrEmpty(emtionmax) && emtionmax != "-1")
            {
                string[] multi = emtionmax.Split('|');
                for (int i = 0; i < multi.Length; i++)
                {
                    _lstEmtion.Add(int.Parse(multi[i]));
                }
            }

            _lstRequire = new List<ItemPair>();
            string require = data["Require"][index];
            if (!string.IsNullOrEmpty(require) && require != "-1")
            {
                string[] multi = require.Split('|');
                for (int i = 0; i < multi.Length; i++)
                {
                    string[] item = multi[i].Split('=');
                    _lstRequire.Add(new ItemPair(int.Parse(item[0]), int.Parse(item[1])));
                }
            }

            _lstReward = new List<ItemPair>();
            string reward = data["Reward"][index];
            if (!string.IsNullOrEmpty(reward) && reward != "-1")
            {
                string[] multi = reward.Split('|');
                for (int i = 0; i < multi.Length; i++)
                {
                    string[] item = multi[i].Split('=');
                    _lstReward.Add(new ItemPair(int.Parse(item[0]), int.Parse(item[1])));
                }
            }
        }

    }
}

