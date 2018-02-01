using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace smallone
{
    public class MainStateLoad : State<LevelMain>
    {
        public MainStateLoad(int stateEnum) : base(stateEnum)
        {

        }


        public override void Enter(object param)
        {
            base.Enter(param);
            //! 加载NPC
            //! 关闭Loading，显示GameHUD
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
