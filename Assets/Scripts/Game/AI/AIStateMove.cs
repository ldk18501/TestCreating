using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace smallone
{
    public class AIStateMove : State<AINpc>
    {
        public AIStateMove(int stateEnum) : base(stateEnum)
        {

        }

        public override void Enter(object param)
        {
            base.Enter(param);

            if (_owner.DesirePoint == null || _owner.AIMaster.nodePos == _owner.DesirePoint)
                StrStateStatus = "MoveOver";
            else
            {
                _owner.GameWorld.gridData.CalculateLinks();
                if (_owner.GameWorld.astar.FindPath(_owner.AIMaster.nodeX, _owner.AIMaster.nodeZ, (int)_owner.DesirePoint.x, (int)_owner.DesirePoint.y))
                {
                    //! 之后把Ball拆成Move组件
                    EntityRole ball = _owner.AIMaster.GetComponent<EntityRole>();
                    ball.MoveByRoads(_owner.GameWorld.astar.path);
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
