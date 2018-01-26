using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace smallone
{
    public class AIBase : MonoBehaviour
    {
        protected bool _bAIPausing;
        protected MyIsoWorld _world;
        protected MyIsoObject _aiMaster;

        public MyIsoWorld GameWorld
        {
            get { return _world; }
        }
        public MyIsoObject AIMaster
        {
            get { return _aiMaster; }
        }

        public void RegisterMaster(MyIsoObject obj)
        {
            _world = (LevelManager.Instance.MainLevel as LevelMain).MainWorld;
            _aiMaster = obj;
            _bAIPausing = true;
        }

        public bool isAIOff
        {
            set { _bAIPausing = value; }
            get { return _bAIPausing; }
        }

        void Update()
        {
            TickStateExecuting(Time.deltaTime);
        }

        protected virtual void TickStateExecuting(float deltaTime) { }
    }
}
