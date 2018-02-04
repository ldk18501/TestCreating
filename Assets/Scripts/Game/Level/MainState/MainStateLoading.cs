using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace smallone
{
    public class MainStateLoading : State<LevelMain>
    {
        public MainStateLoading(int stateEnum) : base(stateEnum)
        {

        }


        public override void Enter(object param)
        {
            base.Enter(param);

            //! 加载
            LoadBuildings();
            LoadNpcs();

            //! 关闭Loading，显示欢迎
            UIPanelManager.Instance.HidePanel("UILoading");
            UIPanelManager.Instance.ShowPanel("UIStartPanel");
        }

        public override string Execute(float deltaTime)
        {
            return base.Execute(deltaTime);
        }

        public override void Exit()
        {
            base.Exit();
        }

        void LoadNpcs() {
            _owner.MainWorld.InitNpc(GameData.lstUnlockNpcs);
        }

        void LoadBuildings() {
            _owner.MainWorld.InitBuildings(DataCenter.Instance.dictBuilding);
        }

    }

}
