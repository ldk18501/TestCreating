using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace smallone
{
    public class TaskData : ICSVDeserializable
    {
        protected string _strID;
        protected string _strTableID;
        protected string _strName;
        protected string _strDesc;
        protected int _nRareLvl;
        protected int _nTaskLvl;
        protected int _nType;
        protected List<int> _lstCritCount;
        protected List<int> _lstCritQuality;
        protected int _nTime;
        protected List<ItemPair> _lstItemReq;
        protected List<ItemPair> _lstProd;
        protected int _nExp;

        public string ID
        {
            get { return _strID; }
        }
        public string Name
        {
            get { return _strName; }
        }
        public string Desc
        {
            get { return _strDesc; }
        }
        public int RareLvl
        {
            get { return _nRareLvl; }
        }
        public int TaskLvl
        {
            get { return _nTaskLvl; }
        }
        public int Type
        {
            get { return _nType; }
        }
        public int Time
        {
            get { return _nTime; }
        }
        public int Exp
        {
            get { return _nExp; }
        }

        public List<int> CritCount {
            get {
                return null;
            }
        }

        public virtual void CSVDeserialize(Dictionary<string, string[]> data, int index)
        {
            _strID = data["ID"][index];
            _strName = data["Name"][index];
            _strDesc = data["Desc"][index];
            _nRareLvl = int.Parse(data["RareLvl"][index]);
            _nTaskLvl = int.Parse(data["TaskLvl"][index]);
            _nType = int.Parse(data["Type"][index]);
            _nTime = int.Parse(data["Time"][index]);
            _nExp = int.Parse(data["Exp"][index]);
            string critCounts = data["CritCount"][index];
            string critQualities = data["CritQuality"][index];
            string itemReqs = data["ItemReq"][index];
            string productions = data["Production"][index];
        
            //数量暴击率
            string[] counts = critCounts.Split('|');
            _lstCritCount = new List<int>();
            for (int i = 0; i < counts.Length; i++)
            {
                _lstCritCount.Add(int.Parse(counts[i]));
            }

            //品质暴击率
            string[] quas = critQualities.Split('|');
            _lstCritQuality = new List<int>();
            for (int i = 0; i < quas.Length; i++)
            {
                _lstCritQuality.Add(int.Parse(quas[i]));
            }

            //需求列表：类型、数量、id
            _lstItemReq = new List<ItemPair>();
            string[] multi = itemReqs.Split('+');
            for (int i = 0; i < multi.Length; i++)
            {
                string[] vals = multi[i].Split('|');
                _lstItemReq.Add(new ItemPair(vals[0], int.Parse(vals[2]), int.Parse(vals[1])));
            }

            //产出列表:类型、id，数量为1
            _lstProd = new List<ItemPair>();
            multi = productions.Split('+');
            for (int i = 0; i < multi.Length; i++)
            {
                string[] vals = multi[i].Split('|');
                _lstProd.Add(new ItemPair(vals[0], 1, int.Parse(vals[1])));
            }
        }
    }
}
