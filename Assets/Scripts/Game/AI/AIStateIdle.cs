using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace smallone
{
    public class AIStateIdle : State<AINpc>
    {
        float _fIdleTime;
        float _fIdleTimeCounter;

        public AIStateIdle(int stateEnum) : base(stateEnum)
        {

        }

        public override void Enter(object param)
        {
            base.Enter(param);
            _fIdleTime = _fIdleTimeCounter = 0;
            if(param != null)
                float.TryParse(param.ToString(), out _fIdleTime);
        }

        public override string Execute(float deltaTime)
        {
            if (string.IsNullOrEmpty(StrStateStatus))
            {
                if (_fIdleTimeCounter < _fIdleTime)
                {
                    _fIdleTimeCounter += deltaTime;
                }
                else StrStateStatus = "IdleOver";
            }

            return base.Execute(deltaTime);
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}

