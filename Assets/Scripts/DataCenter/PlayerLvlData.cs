using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace smallone
{
    public class PlayerLvlData: ICSVDeserializable
    {
        protected string _strID;
        protected int _nRequireExp;
        protected List<string> _lstTableUnlock;
        protected List<string> _lstTaskUnlock;


        public int Lv
        {
            get { return int.Parse(_strID); }
        }

        public int RequireExp
        {
            get { return _nRequireExp; }
        }

        public List<string> TableUnlock
        {
            get { return _lstTableUnlock; }
        }

        public List<string> TaskUnlock
        {
            get { return _lstTaskUnlock; }
        }



        public virtual void CSVDeserialize(Dictionary<string, string[]> data, int index)
        {
            _strID = data["PlayerLv"][index];
            _nRequireExp = int.Parse(data["RequireExp"][index]);

            _lstTableUnlock = new List<string>();
            string table = data["TableUnlock"][index];
            if (!string.IsNullOrEmpty(table) && table != "-1")
            {
                string[] multi = table.Split('|');
                for (int i = 0; i < multi.Length; i++)
                {
                    _lstTableUnlock.Add( multi[i] );
                }
            }

            _lstTaskUnlock = new List<string>();
            string task = data["TaskUnlock"][index];
            if (!string.IsNullOrEmpty(task) && task != "-1")
            {
                string[] multi = task.Split('|');
                for (int i = 0; i < multi.Length; i++)
                {
                    _lstTaskUnlock.Add( multi[i] );
                }
            }

        }

    }
}

