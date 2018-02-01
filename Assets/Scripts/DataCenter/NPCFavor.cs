using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace smallone
{
    public class NPCFavor : ICSVDeserializable
    {
        protected string _strID;
        protected GameObject _objPrefab;

        public string ID
        {
            get { return _strID; }
        }

        public GameObject Prefab
        {
            get { return _objPrefab; }
        }


        public virtual void CSVDeserialize(Dictionary<string, string[]> data, int index)
        {
            _strID = data["ID"][index];
            string prefabPath = data["Prefab"][index];
            if (!string.IsNullOrEmpty(prefabPath))
            {
                _objPrefab = Resources.Load<GameObject>(prefabPath);
            }
        }

    }
}

