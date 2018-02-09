using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace smallone
{
    public class MonsterData : ICSVDeserializable
    {
        protected string _strID;
        protected string _strName;
        protected int _nLvl;
        protected int _nPower;
        protected List<string> _lstLootList;
        protected List<string> _lstVictroyList;
        protected List<string> _lstLoseList;
        protected GameObject _objPrefab;
        protected string _strInfo;
        
        public string ID
        {
            get { return _strID; }
        }

        public string Name
        {
            get { return _strName; }
        }

        public int Lv
        {
            get { return _nLvl; }
        }

        public int Power
        {
            get { return _nPower; }
        }


        public List<string> LootList
        {
            get { return _lstLootList; }
        }

        public List<string> VictroyList
        {
            get { return _lstVictroyList; }
        }

        public List<string> LoseList
        {
            get { return _lstLoseList; }
        }


        public string Info
        {
            get { return _strInfo; }
        }

        public GameObject Prefab
        {
            get
            {
                return _objPrefab;
            }
        }

        public virtual void CSVDeserialize(Dictionary<string, string[]> data, int index)
        {
            _strID = data["Id"][index];
            _strName = data["Name"][index];
            _nLvl = int.Parse(data["Lv"][index]);
            _nPower = int.Parse(data["Power"][index]);

            _lstLootList = new List<string>();
            string lootlist = data["LootList"][index];
            if (!string.IsNullOrEmpty(lootlist) && lootlist != "-1" && lootlist != "0")
            {
                string[] multi = lootlist.Split('|');
                for (int i = 0; i < multi.Length; i++)
                {
                    _lstLootList.Add( multi[i] );
                }
            }

            _lstVictroyList = new List<string>();
            string VictroyList = data["VictoryLoot"][index];
            if (!string.IsNullOrEmpty(VictroyList) && VictroyList != "-1" && VictroyList != "0")
            {
                string[] multi = VictroyList.Split('|');
                for (int i = 0; i < multi.Length; i++)
                {
                    _lstVictroyList.Add(multi[i]);
                }
            }

            _lstLoseList = new List<string>();
            string LoseList = data["LoseLoot"][index];
            if (!string.IsNullOrEmpty(LoseList) && LoseList != "-1" && LoseList != "0")
            {
                string[] multi = LoseList.Split('|');
                for (int i = 0; i < multi.Length; i++)
                {
                    _lstLoseList.Add(multi[i]);
                }
            }
            

            _strInfo = data["Info"][index];

            string prefab = data["Prefab"][index];
            if (!string.IsNullOrEmpty(prefab))
            {
                _objPrefab = Resources.Load<GameObject>(prefab);
            }
            

        }

    }
}

