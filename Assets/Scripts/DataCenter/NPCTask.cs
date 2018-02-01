using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace smallone
{
    public class NPCTask : ICSVDeserializable
    {
        protected string _strID;
        protected int _nTaskId;
        protected List<int> _lstLv;
        protected string _strName;
        protected string _strInfo;
        protected List<int> _lstEmtionMin;
        protected List<int> _lstEmtionMax;
        protected List<ItemPair> _lstRequire;
        protected List<ItemPair> _lstReward;
        
        public string ID
        {
            get { return _strID; }
        }

        protected int TaskId
        {
            get { return _nTaskId; }
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

        protected List<int> EmtionMin
        {
            get { return _lstEmtionMin; }
        }

        protected List<int> EmtionMax
        {
            get { return _lstEmtionMax; }
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
            _strID = data["NpcId"][index];
            _nTaskId = int.Parse(data["TaskId"][index]);
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
            string emtionmin = data["EmtionMin"][index];
            if (!string.IsNullOrEmpty(emtionmin) && emtionmin != "-1")
            {
                string[] multi = emtionmin.Split('|');
                for (int i = 0; i < multi.Length; i++)
                {
                    _lstEmtionMin.Add(int.Parse(multi[i]));
                }
            }

            string emtionmax = data["EmtionMax"][index];
            if (!string.IsNullOrEmpty(emtionmax) && emtionmax != "-1")
            {
                string[] multi = emtionmax.Split('|');
                for (int i = 0; i < multi.Length; i++)
                {
                    _lstEmtionMax.Add(int.Parse(multi[i]));
                }
            }

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

