using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace smallone
{
    public class AIBase : MonoBehaviour
    {
        protected bool _bAIPausing;
        protected MyIsoObject _aiMaster;
        public MyIsoObject AIMaster
        {
            get { return _aiMaster; }
        }

        public void RegisterMaster(MyIsoObject obj)
        {
            _aiMaster = obj;
            _bAIPausing = true;
        }

        public bool isAIOff
        {
            set { _bAIPausing = value; }
            get { return _bAIPausing; }
        }

        // Use this for initialization
        void Awake()
        {

        }

        void Update()
        {
            TickStateExecuting(Time.deltaTime);
        }

        protected virtual void TickStateExecuting(float deltaTime) { }
    }
}
