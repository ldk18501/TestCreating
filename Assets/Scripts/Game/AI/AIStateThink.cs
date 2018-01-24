﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace smallone
{
    public class AIStateThink : State<AINpc>
    {
        //! Hack
        MyIsoWorld _isoWorld;

        public AIStateThink(int stateEnum) : base(stateEnum)
        {
        }

        public override void Enter(object param)
        {
            base.Enter(param);
            //! 之后把ISOWorld编程全局生成
            if (_isoWorld == null)
                _isoWorld = GameObject.Find("ISOWorld").GetComponent<MyIsoWorld>();

            int ranNum = Random.Range(0, 5);
            if (ranNum == 0)
                StrStateStatus = "Stay";
            else 
            {
                List<MyIsoObject> buildings = _isoWorld.GetBuildings();
                if (buildings.Count > 0)
                {
                    int ranPick = Random.Range(0, buildings.Count);
                    _owner.DesirePoint = new Vector2(buildings[ranPick].nodeX, buildings[ranPick].nodeZ + buildings[ranPick].spanZ);
                    Debug.Log(_owner.DesirePoint);
                    StrStateStatus = "MoveToPoint";
                }
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
    }
}
