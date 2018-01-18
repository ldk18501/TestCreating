using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace smallone
{
    public enum ItemType
    {
        Food,
        Equipment,
        Paper
    }

    // TODO::临时放这里
    public class ItemCost
    {
        Dictionary<string, int> _costMap = new Dictionary<string, int>();
        public void AddCost(string type, int cost)
        {
            if (_costMap.ContainsKey(type))
                _costMap[type] += cost;
            else _costMap.Add(type, cost);
        }
    }

    public class Item : ICSVDeserializable
    {
        protected string _strID;
        protected string _strName;
        protected string _strDesc;
        protected string _strIcon;
        protected int _nQuality;
        protected int _nLvl;
        protected int _nType;
        protected int _nOrder;
        protected int _nPower;
        protected string _strEffect;
        protected string _strColor;
        protected string _strSkill;
        protected int _nPreTask;
        protected string _strPrice;
        protected int _nStoreCount;

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
        public string IconPath
        {
            get { return _strIcon; }
        }

        public Sprite IconSprite
        {
            get { return string.IsNullOrEmpty(_strIcon) ? null : AtlasManager.Instance.GetSprite(_strIcon); }
        }

        public int Quality
        {
            get { return _nQuality; }
        }

        public int Lvl
        {
            get { return _nLvl; }
        }
        public int Type
        {
            get { return _nType; }
        }

        public ItemType Category
        {
            get
            {
                if (_nType < 100)
                    return ItemType.Food;
                else if (_nType > 100 && _nType < 1000)
                    return ItemType.Equipment;
                else
                    return ItemType.Paper;
            }
        }

        public int Order
        {
            get { return _nOrder; }
        }
        public int Power
        {
            get { return _nPower; }
        }

        public Vector3 ItemRGB
        {
            get
            {
                if (string.IsNullOrEmpty(_strColor))
                    return Vector3.zero;
                else
                {
                    var rgbArray = _strColor.Split('|');
                    return new Vector3(float.Parse(rgbArray[0]), float.Parse(rgbArray[1]), float.Parse(rgbArray[2]));
                }
            }
        }

        public ItemCost Price
        {
            get
            {
                string[] multi = _strPrice.Split('+');
                ItemCost cost = new ItemCost();
                for (int i = 0; i < multi.Length; i++)
                {
                    string[] price = multi[i].Split('|');
                    cost.AddCost(price[0], int.Parse(price[1]));
                }
                return cost;
            }
        }

        public int StoreCount
        {
            get { return _nStoreCount; }
        }

        public virtual void CSVDeserialize(Dictionary<string, string[]> data, int index)
        {
            _strID = data["ID"][index];
            _strName = data["Name"][index];
            _strDesc = data["Desc"][index];
            _nQuality = int.Parse(data["Quality"][index]);
            _nLvl = int.Parse(data["Lvl"][index]);
            _nType = int.Parse(data["Type"][index]);
            _nOrder = int.Parse(data["Order"][index]);
            _strIcon = data["Icon"][index];
            _strEffect = data["Effect"][index];
            _strColor = data["Color"][index];
            _strSkill = data["Skill"][index];
            _nPreTask= int.Parse(data["Task"][index]);
            _strPrice = data["Price"][index];
            _nStoreCount = int.Parse(data["Store"][index]);

            //string prefabPath = data["Prefab"][index];
            //if (!string.IsNullOrEmpty(prefabPath))
            //{
            //    _mPrefab = Resources.Load<GameObject>(prefabPath);
            //}
        }
    }
}