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
        protected Vector2 _v2DesirePoint;
        protected StateMachine<AINpc> _fsmNPC;

        public Vector2 DesirePoint
        {
            set { _v2DesirePoint = value; }
            get { return _v2DesirePoint; }
        }

        // Use this for initialization
        void Start()
        {
            _fsmNPC = new StateMachine<AINpc>(this);

            _fsmNPC.AddState(new AIStateIdle((int)PhaseEnum.Idle));
            _fsmNPC.AddState(new AIStateMove((int)PhaseEnum.Move));
            _fsmNPC.AddState(new AIStateThink((int)PhaseEnum.Think));
            _fsmNPC.AddState(new AIStateWork((int)PhaseEnum.Work));
            _fsmNPC.AddState(new AIStateJudge((int)PhaseEnum.Judge));

            _fsmNPC.ChangeState((int)PhaseEnum.Think);
        }

        protected override void TickStateExecuting(float deltaTime)
        {
            if (_bAIPausing) return;

            string status = _fsmNPC.TickState(deltaTime);
            if (status != null)
            {
                switch (status)
                {
                    case "IdleOver":
                    case "MoveOver":
                        _fsmNPC.ChangeState((int)PhaseEnum.Think);
                        break;
                    case "MoveToPoint":
                        _fsmNPC.ChangeState((int)PhaseEnum.Move);
                        break;
                    default:
                        _fsmNPC.ChangeState((int)PhaseEnum.Idle, Random.Range(2, 4));
                        break;
                }
            }
        }

    }
}
