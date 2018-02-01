using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace smallone
{
    public class Languages : ICSVDeserializable
    {
        protected string _strID;
        protected string _strCN;
        protected string _strEN;

        public string ID
        {
            get { return _strID; }
        }

        public string CN
        {
            get { return _strCN; }
        }

        public string EN
        {
            get { return _strEN; }
        }



        public virtual void CSVDeserialize(Dictionary<string, string[]> data, int index)
        {
            _strID = data["ID"][index];
            _strCN = data["Chinese"][index];
            _strEN = data["English"][index];

        }

    }
}

