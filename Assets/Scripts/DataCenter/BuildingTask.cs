using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace smallone
{

    public class BuildingTask : ICSVDeserializable
    {
        protected string _strID;
        protected string _nTableId;
        protected string _strName;
        protected int _nType;
        protected int _nLv;
        protected List<int> _lstCountCritWeight;
        protected List<int> _lstQualityCritWeight;
        protected int _nTime;
        protected List<ItemPair> _lstItemRequire;
        protected ItemPair _itempairProduct;
        protected int _nExp;
        protected string _strInfo;

        public string ID
        {
            get { return _strID; }
        }

        public string TableId
        {
            get { return _nTableId; }
        }

        public string Name
        {
            get { return _strName; }
        }

        public int Type
        {
            get { return _nType; }
        }

        public int Lv
        {
            get { return _nLv; }
        }

        public List<int> CountCritWeight
        {
            get { return _lstCountCritWeight; }
        }

        public List<int> QualityCritWeight
        {
            get { return _lstQualityCritWeight; }
        }

        public int Time
        {
            get { return _nTime; }
        }

        public List<ItemPair> ItemRequire
        {
            get { return _lstItemRequire; }
        }

        public ItemPair Product
        {
            get { return _itempairProduct; }
        }

        public int Exp
        {
            get { return _nExp; }
        }

        public string Info
        {
            get { return _strInfo; }
        }

        public virtual void CSVDeserialize(Dictionary<string, string[]> data, int index)
        {
            _strID = data["Id"][index];
            _nTableId = data["TableId"][index];
            _strName = data["Name"][index];
            _nType = int.Parse(data["Type"][index]);
            _nLv = int.Parse(data["Lv"][index]);

            _lstCountCritWeight = new List<int>();
            string countcrit = data["CountCrit"][index];
            if(!string.IsNullOrEmpty(countcrit) && countcrit != "-1" )
            {
                string[] multi = countcrit.Split('|');
                for (int i = 0; i < multi.Length; i++)
                {
                    _lstCountCritWeight.Add(int.Parse(multi[i]));
                }
            }

            _lstQualityCritWeight = new List<int>();
            string qualitycrit = data["QualityCrit"][index];
            if (!string.IsNullOrEmpty(qualitycrit) && qualitycrit != "-1")
            {
                string[] multi = qualitycrit.Split('|');
                for (int i = 0; i < multi.Length; i++)
                {
                    _lstQualityCritWeight.Add(int.Parse(multi[i]));
                }
            }

            _nTime = int.Parse(data["Time"][index]);


            _lstItemRequire = new List<ItemPair>();
            string itemrequire = data["ItemRequire"][index];
            if (!string.IsNullOrEmpty(itemrequire) && itemrequire != "-1")
            {
                string[] multi = itemrequire.Split('|');
                for (int i = 0; i < multi.Length; i++)
                {
                    string[] item = multi[i].Split('=');
                    _lstItemRequire.Add(new ItemPair( item[0], int.Parse(item[1])));
                }
            }
            
            string product = data["product"][index];
            if (!string.IsNullOrEmpty(product) && product != "-1")
            {
                string[] multi = product.Split('=');
                _itempairProduct = new ItemPair( multi[0] , int.Parse(multi[1]));               
            }

            _nExp = int.Parse(data["Exp"][index]);
            _strInfo = data["Info"][index];
            

        }

    }
}

