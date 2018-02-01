using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace smallone
{
    public class PlayerLvlData: ICSVDeserializable
    {
        protected string _strID;
        protected int _nRequireExp;
        protected List<int> _lstTableUnlock;
        protected List<int> _lstTaskUnlock;


        public string Lv
        {
            get { return _strID; }
        }

        public int RequireExp
        {
            get { return _nRequireExp; }
        }

        public List<int> TableUnlock
        {
            get { return _lstTableUnlock; }
        }

        public List<int> TaskUnlock
        {
            get { return _lstTaskUnlock; }
        }



        public virtual void CSVDeserialize(Dictionary<string, string[]> data, int index)
        {
            _strID = data["PlayerLv"][index];
            _nRequireExp = int.Parse(data["RequireExp"][index]);

            string table = data["TableUnlock"][index];
            if (!string.IsNullOrEmpty(table) && table != "-1")
            {
                string[] multi = table.Split('|');
                for (int i = 0; i < multi.Length; i++)
                {
                    _lstTableUnlock.Add(int.Parse(multi[i]));
                }
            }

            string task = data["TaskUnlock"][index];
            if (!string.IsNullOrEmpty(task) && task != "-1")
            {
                string[] multi = task.Split('|');
                for (int i = 0; i < multi.Length; i++)
                {
                    _lstTaskUnlock.Add(int.Parse(multi[i]));
                }
            }

        }

    }
}

