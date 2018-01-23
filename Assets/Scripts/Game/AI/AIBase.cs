using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace smallone
{
    public class AIBase : MonoBehaviour
    {
        protected bool _bAIPausing;
        protected MyIsoObject _owner;
        protected StateMachine<MyIsoObject> _fsmIsoObject;

        public void RegisterOwner(MyIsoObject obj)
        {
            _owner = obj;
            _bAIPausing = true;
        }

        public bool isAIOff {
            set { _bAIPausing = value; }
            get { return _bAIPausing; }
        }

        // Use this for initialization
        void Awake()
        {
            _fsmIsoObject = new StateMachine<MyIsoObject>(_owner);
        }

        void Update()
        {
        
        }

        protected virtual void TickStateExecuting(float deltaTime) { }
    }
}
