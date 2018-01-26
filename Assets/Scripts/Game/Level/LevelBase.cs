using DoozyUI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace smallone
{
    public class LevelBase
    {
        protected bool _bPausedAtUI;
        protected bool _bLevelEnded;
        protected Dictionary<string, GameObject> _dictLevelEntities = new Dictionary<string, GameObject>();
        public Dictionary<string, GameObject> LevelObjs
        { get { return _dictLevelEntities; } }

        public LevelBase()
        {
            CreateLevelFsm();
        }

        //关卡加载
        public virtual void LoadLevel()
        {
            _bPausedAtUI = _bLevelEnded = false;
        }

        public virtual void ResetLevel()
        {

        }

        protected virtual void GenLevelEntities(string path)
        {
            UIPanelManager.Instance.HidePanel("UILoading");
        }

        //关卡退出清理
        public virtual void CleanLevel()
        {
            _dictLevelEntities.Clear();
            _bLevelEnded = true;
        }

        //构造时创建关卡所有状态
        protected virtual void CreateLevelFsm()
        { }

        //用于检查关卡内状态切换
        public virtual void TickStateExecuting(float deltaTime)
        { }

        public virtual void OnContinueLevelPhase()
        { }
    }

    public class LevelEntityData : ICSVDeserializable
    {
        protected string _strId;
        protected string _strName;
        protected Vector2 _v2PointInit;

        public string Name
        {
            get { return _strName; }
        }
        public string ID
        {
            get
            {
                return _strId;
            }
        }

        public Vector2 InitPoint
        {
            get { return _v2PointInit; }
        }

        public void CSVDeserialize(Dictionary<string, string[]> data, int index)
        {
            _strName = data["Name"][index];
            _strId = data["ID"][index];
        }
    }
}
