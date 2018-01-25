using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace smallone
{
    public class AIStateMove : State<AINpc>
    {
        //! Hack
        MyIsoWorld _isoWorld;
        public AIStateMove(int stateEnum) : base(stateEnum)
        {

        }

        public override void Enter(object param)
        {
            base.Enter(param);

            //! 之后把ISOWorld编程全局生成
            if (_isoWorld == null)
                _isoWorld = GameObject.Find("ISOWorld").GetComponent<MyIsoWorld>();

            if (_owner.DesirePoint == null || _owner.AIMaster.nodePos == _owner.DesirePoint)
                StrStateStatus = "MoveOver";
            else
            {
                _isoWorld.gridData.CalculateLinks();
                if (_isoWorld.astar.FindPath(_owner.AIMaster.nodeX, _owner.AIMaster.nodeZ, (int)_owner.DesirePoint.x, (int)_owner.DesirePoint.y))
                {
                    //! 之后把Ball拆成Move组件
                    Ball ball = _owner.AIMaster.GetComponent<Ball>();
                    ball.MoveByRoads(_isoWorld.astar.path);
                    ball.cbArrived = OnArrive;
                }
                else StrStateStatus = "MoveOver";
            }
        }

        public override string Execute(float deltaTime)
        {
            return base.Execute(deltaTime);
        }

        public override void Exit()
        {
            base.Exit();
        }

        void OnArrive()
        {
            StrStateStatus = "MoveOver";
        }
    }
}
