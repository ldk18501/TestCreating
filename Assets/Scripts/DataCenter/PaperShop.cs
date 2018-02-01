using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace smallone
{
    public class PaperShop : ICSVDeserializable
    {
        protected string _strID;
        protected string _strName;
        protected int _nPlayerLv;
        protected List<ItemPair> _lstPrice;
        protected int _nTskId;
        protected string _strIcon;
        protected Sprite _spIcon;
        protected string _strInfo;


        public string ID
        {
            get { return _strID; }
        }

        public string Name
        {
            get { return _strName; }
        }

        public int PlayerLv
        {
            get { return _nPlayerLv; }
        }

        public List<ItemPair> Price
        {
            get { return _lstPrice; }
        }

        public int TskId
        {
            get { return _nTskId; }
        }

        public string IconPath
        {
            get { return _strIcon; }
        }

        public Sprite IconSprite
        {
            get { return _spIcon; }
        }

        public string Info
        {
            get { return _strInfo; }
        }
        
        public virtual void CSVDeserialize(Dictionary<string, string[]> data, int index)
        {
            _strID = data["ID"][index];
            _strName = data["Name"][index];
            _nPlayerLv = int.Parse(data["PlayerLv"][index]);
            string price = data["Price"][index];
            if (!string.IsNullOrEmpty(price) && price != "-1")
            {
                string[] multi = price.Split('|');
                for (int i = 0; i < multi.Length; i++)
                {
                    string[] item = multi[i].Split('=');
                    _lstPrice.Add(new ItemPair(int.Parse(item[0]), int.Parse(item[1])));
                }
            }

            _nTskId = int.Parse(data["TaskId"][index]);
            _strIcon = data["Src"][index];
            _strInfo = data["Info"][index];

            _spIcon = string.IsNullOrEmpty(_strIcon) ? null : AtlasManager.Instance.GetSprite(_strIcon);

        }
    }
}

