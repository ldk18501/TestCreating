using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoozyUI;
using DG.Tweening;

namespace smallone
{
    public class LevelMain : LevelBase
    {
        private StateMachine<LevelMain> _fsmLevel;

        private Vector3 _v3Fix = new Vector3(-500, 500, 0);

        public LevelMain()
        {
        }

        protected override void CreateLevelFsm()
        {
            _fsmLevel = new StateMachine<LevelMain>(this);
        }

        public override void LoadLevel()
        {
            base.LoadLevel();

            //new Vector3(-448, 572, 120), new Vector3(20, 195, 0));

            if (!LevelManager.Instance.IsInGame)
                LevelManager.Instance.StartCoroutine(EnterGame());
            else
                LevelManager.Instance.StartCoroutine(ReturnFromGame());
        }

        public override void CleanLevel()
        {
            if (_fsmLevel.CurState != null)
                _fsmLevel.CurState.Exit();
            _fsmLevel.CleanState();

            base.CleanLevel();
            Debug.Log("clean");
        }

        public override void TickStateExecuting(float deltaTime)
        {
            if (_bLevelEnded)
                return;
            string status = _fsmLevel.TickState(deltaTime);
        }

        #region MainUI
        IEnumerator ReturnFromGame()
        {
            //one-frame delay to avoid jitter
            yield return null;

            UIPanelManager.Instance.HidePanel("UILoading");
            UIPanelManager.Instance.ShowPanel("UIGameHUD");
        }
        IEnumerator EnterGame()
        {
            yield return null;
            UIPanelManager.Instance.ShowPanel("UIStartPanel");
            // UIPanelManager.Instance.GetPanel("UICover").GetComponentInChildren<SpriteFrameAnim>().Play();
        }
        #endregion
    }
}
