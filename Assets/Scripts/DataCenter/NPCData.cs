using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace smallone
{
    public class NPCData : ICSVDeserializable
    {
        protected string _strID;
        protected string _strName;
        protected Sprite _spPortrait;
        protected GameObject _objPrefab;
        protected int _nUnlockLv;
        protected List<int> _lstSkillUnlocklv;
        protected int _nPower;
        protected List<int> _lstSkillId;
        protected List<int> _lstFavor;
        protected List<int> _lstCharacter;
        protected List<int> _lstEquipType;
        protected List<int> _lstCardUnlockLv;
        protected int _nEmotionLimit;
        protected List<int> _lstTalkInterval;
        protected List<int> _lstGiftInterval;

        public string ID
        {
            get { return _strID; }
        }

        public string Name
        {
            get { return _strName; }
        }

        public int UnlockLv
        {
            get { return _nUnlockLv; }
        }

        public List<int> SkillUnlocklv
        {
            get { return _lstSkillUnlocklv; }
        }

        public int Power
        {
            get { return _nPower; }
        }

        public List<int> SkillId
        {
            get { return _lstSkillId; }
        }

        public List<int> Favor
        {
            get { return _lstFavor; }
        }

        public List<int> Character
        {
            get { return _lstCharacter; }
        }

        public List<int> EquipType
        {
            get { return _lstEquipType; }
        }

        public List<int> CardUnlockLv
        {
            get { return _lstCardUnlockLv; }
        }

        public int EmotionLimit
        {
            get { return _nEmotionLimit; }
        }

        public List<int> TalkInterval
        {
            get { return _lstTalkInterval; }
        }

        public List<int> GiftInterval
        {
            get { return _lstGiftInterval; }
        }

        public GameObject Prefab
        {
            get { return _objPrefab; }
        }

        public virtual void CSVDeserialize(Dictionary<string, string[]> data, int index)
        {
            _strID = data["Id"][index];
            _strName = data["Name"][index];

            _spPortrait = string.IsNullOrEmpty(data["Portrait"][index]) ? null : AtlasManager.Instance.GetSprite(data["Portrait"][index]);

            string prefab = data["Prefab"][index];
            if (!string.IsNullOrEmpty(prefab))
            {
                _objPrefab = Resources.Load<GameObject>(prefab);
            }

            _nUnlockLv = int.Parse(data["UnlockLv"][index]);

            _lstSkillUnlocklv = new List<int>();
            string skillunlocklv = data["SkillUnlock"][index];
            if (!string.IsNullOrEmpty(skillunlocklv) && skillunlocklv != "-1")
            {
                string[] multi = skillunlocklv.Split('|');
                for (int i = 0; i < multi.Length; i++)
                {
                    _lstSkillUnlocklv.Add(int.Parse(multi[i]));
                }
            }

            _nPower = int.Parse(data["Power"][index]);

            _lstSkillId = new List<int>();
            string skillid = data["Skill"][index];
            if (!string.IsNullOrEmpty(skillid) && skillid != "-1")
            {
                string[] multi = skillid.Split('|');
                for (int i = 0; i < multi.Length; i++)
                {
                    _lstSkillId.Add(int.Parse(multi[i]));
                }
            }

            _lstFavor = new List<int>();
            string favor = data["Favor"][index];
            if (!string.IsNullOrEmpty(favor) && favor != "-1")
            {
                string[] multi = favor.Split('|');
                for (int i = 0; i < multi.Length; i++)
                {
                    _lstFavor.Add(int.Parse(multi[i]));
                }
            }

            _lstCharacter = new List<int>();
            string character = data["Character"][index];
            if (!string.IsNullOrEmpty(character) && character != "-1")
            {
                string[] multi = character.Split('|');
                for (int i = 0; i < multi.Length; i++)
                {
                    _lstCharacter.Add(int.Parse(multi[i]));
                }
            }

            _lstEquipType = new List<int>();
            string equiptype = data["Equip"][index];
            if (!string.IsNullOrEmpty(equiptype) && equiptype != "-1")
            {
                string[] multi = equiptype.Split('|');
                for (int i = 0; i < multi.Length; i++)
                {
                    _lstEquipType.Add(int.Parse(multi[i]));
                }
            }

            _lstCardUnlockLv = new List<int>();
            string cardunlocklv = data["Card"][index];
            if (!string.IsNullOrEmpty(cardunlocklv) && cardunlocklv != "-1")
            {
                string[] multi = cardunlocklv.Split('|');
                for (int i = 0; i < multi.Length; i++)
                {
                    _lstCardUnlockLv.Add(int.Parse(multi[i]));
                }
            }

            _nEmotionLimit = int.Parse(data["Emotion"][index]);

            _lstTalkInterval = new List<int>();
            string talk = data["TalkInterval"][index];
            if (!string.IsNullOrEmpty(talk) && talk != "-1")
            {
                string[] multi = talk.Split('|');
                for (int i = 0; i < multi.Length; i++)
                {
                    _lstTalkInterval.Add(int.Parse(multi[i]));
                }
            }

            _lstGiftInterval = new List<int>();
            string gift = data["GiftInterval"][index];
            if (!string.IsNullOrEmpty(gift) && gift != "-1")
            {
                string[] multi = gift.Split('|');
                for (int i = 0; i < multi.Length; i++)
                {
                    _lstGiftInterval.Add(int.Parse(multi[i]));
                }
            }
        }

    }
}

