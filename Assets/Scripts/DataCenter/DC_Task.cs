using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace smallone
{
    public class DC_Task : ICSVDeserializable
    {
        protected string _strID;
        protected string _strTableID;
        protected string _strName;
        protected string _strDesc;
        protected int _nRareLvl;
        protected int _nTaskLvl;
        protected int _nType;
        protected string _strCritCount;
        protected string _strCritQuality;
        protected int _nTime;
        protected string _strItemReq;
        protected string _strProduction;
        protected int _nExp;


        public virtual void CSVDeserialize(Dictionary<string, string[]> data, int index)
        {
            _strID = data["ID"][index];
            _strName = data["Name"][index];
            _strDesc = data["Desc"][index];
            _nRareLvl = int.Parse(data["RareLvl"][index]);
            _nTaskLvl = int.Parse(data["TaskLvl"][index]);
            _nType = int.Parse(data["Type"][index]);
            _nTime = int.Parse(data["Time"][index]);
            _strCritCount = data["CritCount"][index];
            _strCritQuality = data["CritQuality"][index];
            _strItemReq = data["ItemReq"][index];
            _strProduction = data["Production"][index];
            _nExp = int.Parse(data["Exp"][index]);
        }
    }
}
