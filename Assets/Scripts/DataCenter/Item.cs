using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace smallone
{
    public enum ItemType
    {
        Token,
        Food,
        Equipment,
        Material,
        Card,
        Special
    }
    

    public class ItemPair {
        public string strId;
        public int nCount;
        public ItemPair(string id , int count = 0) {
            strId = id;
            nCount = count;
        }
    }

    public class Item : ICSVDeserializable
    {
        protected string _strID;
        protected string _strName;
        protected int _nLvl;
        protected int _nType;
        protected int _nQuality;
        protected int _nPower;
        protected string _strSkill;
        protected int _nStoreCount;
        protected int _nOrder;
        protected List<ItemPair> _lstPrice;
        protected string _strIcon;
        protected Sprite _spIcon;
        protected string _strEffect;
        protected string _strInfo;

		// 道具堆叠数量
		public int Have = 0;

        public string ID
        {
            get { return _strID; }
        }
        public string Name
        {
            get { return _strName; }
        }
        public string Info
        {
            get { return _strInfo; }
        }
        public string IconPath
        {
            get { return _strIcon; }
        }

        public Sprite IconSprite
        {
            get { return _spIcon; }
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
                    return ItemType.Token;
                else if (_nType >= 100 && _nType < 200)
                    return ItemType.Equipment;
                else if (_nType >= 200 && _nType < 300)
                    return ItemType.Food;
                else if (_nType >= 300 && _nType < 400)
                    return ItemType.Material;
                else if (_nType >= 400 && _nType < 500)
                    return ItemType.Card;
                else
                    return ItemType.Special;
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

        // 装备品质为贴图，代码留下做参考
//         public Vector3 ItemRGB
//         {
//             get
//             {
//                 if (string.IsNullOrEmpty(_strColor))
//                     return Vector3.zero;
//                 else
//                 {
//                     var rgbArray = _strColor.Split('|');
//                     return new Vector3(float.Parse(rgbArray[0]), float.Parse(rgbArray[1]), float.Parse(rgbArray[2]));
//                 }
//             }
//         }

        public List<ItemPair> Price
        {
            get
            {
                return _lstPrice;
            }
        }

        public int StoreCount
        {
            get { return _nStoreCount; }
        }

        public virtual void CSVDeserialize(Dictionary<string, string[]> data, int index)
        {
            _strID = data["Id"][index];
            _strName = data["Name"][index];
            _strInfo = data["Info"][index];
            _nQuality = int.Parse(data["Quality"][index]);
            _nLvl = int.Parse(data["Lvl"][index]);
            _nType = int.Parse(data["Type"][index]);
            _nOrder = int.Parse(data["Order"][index]);
            _strIcon = data["Src"][index];
            _strEffect = data["Effect"][index];

            _strSkill = data["Skill"][index];

            _lstPrice = new List<ItemPair>();
            string price = data["Price"][index];
            if (!string.IsNullOrEmpty(price) && price != "-1" && price != "0")
            {
                string[] multi = price.Split('|');
                for (int i = 0; i < multi.Length; i++)
                {
                    string[] prices = multi[i].Split('=');
                    _lstPrice.Add(new ItemPair( prices[0] , int.Parse(prices[1])));
                }
            }

            _nStoreCount = int.Parse(data["Store"][index]);
            _spIcon = string.IsNullOrEmpty(_strIcon) ? null : AtlasManager.Instance.GetSprite(_strIcon);

            //string prefabPath = data["Prefab"][index];
            //if (!string.IsNullOrEmpty(prefabPath))
            //{
            //    _mPrefab = Resources.Load<GameObject>(prefabPath);
            //}
        }
    }
}