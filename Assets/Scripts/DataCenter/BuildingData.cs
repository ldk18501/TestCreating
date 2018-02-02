using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace smallone
{
    public class BuildingData : ICSVDeserializable
    {
        protected string _strID;
        protected int _nType;
        protected int _nLvl;
        protected string _strName;
        protected string _strIcon;
        protected Sprite _spIcon;
        protected int _nSpanX;
        protected int _nSpanZ;
        protected int _nNodeX;
        protected int _nNodeZ;
        protected float _fSortX;
        protected float _fSortZ;

        protected GameObject _objPrefab;

        public string ID {
            get { return _strID; }
        }

        public int Type {
            get { return _nType; }
        }

        public int Lv {
            get { return _nLvl; }
        }

        public string Name {
            get { return _strName; }
        }
        public string IconPath {
            get { return _strIcon; }
        }

        public Sprite IconSprite
        {
            get { return string.IsNullOrEmpty(_strIcon) ? null : AtlasManager.Instance.GetSprite(_strIcon); }
        }

        public int SpanX
        {
            get { return _nSpanX; }
        }

        public int SpanZ
        {
            get { return _nSpanZ; }
        }

        public int NodeX
        {
            get { return _nNodeX; }
        }

        public int NodeZ
        {
            get { return _nNodeZ; }
        }

        public float SortX
        {
            get { return _fSortX; }
        }

        public float SortZ
        {
            get { return _fSortZ; }
        }

        public GameObject Prefab
        {
            get
            {
                string prefabPath = "Prefabs/Temp/";
                switch (_nType)
                {
                    case 1:
                        {
                            prefabPath += "ForestObject";
                            break;
                        }
                    default:
                        {
                            prefabPath += "HostObject";
                            break;
                        }
                }

                if (!string.IsNullOrEmpty(prefabPath))
                {
                    return Resources.Load<GameObject>(prefabPath);
                }
                else
                {
                    return null;
                }
            }
        }

        public virtual void CSVDeserialize(Dictionary<string, string[]> data, int index)
        {
            _strID = data["Id"][index];
            _nType = int.Parse(data["Type"][index]);
            _nLvl = int.Parse(data["Lv"][index]);
            _strName = data["Name"][index];
            _strIcon = data["Src"][index];
            _nSpanX = int.Parse(data["spanX"][index]);
            _nSpanZ = int.Parse(data["spanZ"][index]);
            _nNodeX = int.Parse(data["nodeX"][index]);
            _nNodeZ = int.Parse(data["nodeZ"][index]);
            _fSortX = float.Parse(data["SortX"][index]);
            _fSortZ = float.Parse(data["SortZ"][index]);

            //_spIcon = string.IsNullOrEmpty(_strIcon) ? null : AtlasManager.Instance.GetSprite(_strIcon);
            
        }

    }
}

