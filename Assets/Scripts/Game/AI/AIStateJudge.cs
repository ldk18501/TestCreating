using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace smallone
{
    public class AIStateJudge : State<MyIsoObject>
    {
        public AIStateJudge(int stateEnum) : base(stateEnum)
        {

        }
        public override void Enter(object param)
        {
            base.Enter(param);
        }

        public override string Execute(float deltaTime)
        {
            return base.Execute(deltaTime);
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}