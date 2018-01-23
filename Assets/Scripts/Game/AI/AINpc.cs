using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace smallone
{
    public enum PhaseEnum
    {
        Idle,
        Move,
        Think,
        Work,
        Judge,
    }

    public class AINpc : AIBase
    {

        // Use this for initialization
        void Start()
        {
            _fsmIsoObject.AddState(new AIStateIdle((int)PhaseEnum.Idle));
            _fsmIsoObject.AddState(new AIStateMove((int)PhaseEnum.Move));
            _fsmIsoObject.AddState(new AIStateThink((int)PhaseEnum.Think));
            _fsmIsoObject.AddState(new AIStateWork((int)PhaseEnum.Work));
            _fsmIsoObject.AddState(new AIStateJudge((int)PhaseEnum.Judge));

            _fsmIsoObject.ChangeState((int)PhaseEnum.Idle);
        }

        protected override void TickStateExecuting(float deltaTime)
        {
            if (_bAIPausing) return;

            string status = _fsmIsoObject.TickState(deltaTime);
            if (status != null)
            {
                switch (status)
                {
                    case "IdleOver":
                        _fsmIsoObject.ChangeState((int)PhaseEnum.Think);
                        break;
                    case "MoveToPoint":
                        _fsmIsoObject.ChangeState((int)PhaseEnum.Move);
                        break;
                    default:
                        _fsmIsoObject.ChangeState((int)PhaseEnum.Idle);
                        break;
                }
            }
        }
    }
}
