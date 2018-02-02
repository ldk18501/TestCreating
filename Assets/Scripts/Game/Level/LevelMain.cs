using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoozyUI;
using DG.Tweening;

namespace smallone
{
    public class LevelMain : LevelBase
    {
        public enum PhaseEnum {
            Load,
            World,
        }

        private StateMachine<LevelMain> _fsmLevel;
        private Vector3 _v3Fix = new Vector3(-500, 500, 0);
        private bool _bMainGameStarted;
        private MyIsoWorld _isoWorld;
        public MyIsoWorld MainWorld
        {
            get
            {
                return _isoWorld;
            }
        }

        public bool MainGameStarted
        {
            get
            {
                return _bMainGameStarted;
            }
        }

        public LevelMain()
        {
            _bMainGameStarted = false;
            if (_isoWorld == null)
                _isoWorld = GameObject.Find("ISOWorld").GetComponent<MyIsoWorld>();
        }

        protected override void CreateLevelFsm()
        {
            _fsmLevel = new StateMachine<LevelMain>(this);
            _fsmLevel.AddState(new MainStateLoad((int)PhaseEnum.Load));

            _fsmLevel.ChangeState((int)PhaseEnum.Load);
        }

        protected override void GenLevelEntities(string path)
        {
            var entities = SerializationManager.LoadFromCSV<LevelEntityData>(path);
            if (entities != null)
            {
                entities.ForEach(e =>
                {
                    var inited = GameObject.Instantiate(DataCenter.Instance.dictBuilding[e.ID].Prefab, Vector3.one * 5000, Quaternion.identity);
                    inited.name = e.Name;
                    //TODO::通过MyIsoObject类和DataCenter的配置设定size等信息
                    inited.AddMissingComponent<MyIsoObject>();
                    //TODO::把建筑都存在dict中，根据关卡表生成设定建筑位置
                    _dictLevelEntities.Add(e.ID, inited);
                });
            }
            base.GenLevelEntities(path);
        }

        public override void LoadLevel()
        {
            base.LoadLevel();
            //! 现在建筑都是静态的情况下就不调用了
            // GenLevelEntities();

            //! 如果以后有事件需要切换场景
            if (!LevelManager.Instance.IsInEvent)
                LevelManager.Instance.StartCoroutine(EnterGame());
            else
                LevelManager.Instance.StartCoroutine(ReturnFromEvent());
        }


        public override void CleanLevel()
        {
            if (_fsmLevel.CurState != null)
                _fsmLevel.CurState.Exit();
            _fsmLevel.CleanState();

            base.CleanLevel();
            Debug.Log("main game cleaned");
        }

        public override void TickStateExecuting(float deltaTime)
        {
            if (_bLevelEnded)
                return;
            string status = _fsmLevel.TickState(deltaTime);
        }

        #region MainUI
        //TODO::预留，可能没用，再议
        IEnumerator ReturnFromEvent()
        {
            //one-frame delay to avoid jitter
            yield return null;

            UIPanelManager.Instance.HidePanel("UILoading");
            UIPanelManager.Instance.ShowPanel("UIGameHUD");
        }

        // 第一次进游戏初始化用
        IEnumerator EnterGame()
        {
            yield return null;
            UIPanelManager.Instance.ShowPanel("UIStartPanel");
        }
        #endregion

        public void StartGameLogic()
        {
            _bMainGameStarted = true;
            //_isoWorld.ball.gameObject.AddMissingComponent<smallone.AINpc>().RegisterMaster(_isoWorld.ball);
            //_isoWorld.ball.GetComponent<smallone.AINpc>().isAIOff = false;
        }

    }
}
