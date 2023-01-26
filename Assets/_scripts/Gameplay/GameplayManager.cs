using DG.Tweening;
using Lean.Touch;
using System;
using System.Collections.Generic;
using UnityEngine;
using vgwb.framework;

namespace vgwb.lanoria
{
    public class GameplayManager : SingletonMonoBehaviour<GameplayManager>
    {
        #region Var
        public bool StartGameOnPlay = false;
        [Header("Cards")]
        public CardDealer Dealer;
        [Header("Score")]
        public ScoreManager Scorer;
        [Header("State")]
        public GameplayBehaviour StateHandler;

        private UI_GameHUD UIGame;
        private LeanSpawnWithFinger spawner;
        #endregion

        #region MonoB
        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            UIGame = UI_manager.I.PanelGameHUD;
            EventsSubscribe();

            if (StartGameOnPlay) {
                StartGame();
            }
        }

        private void OnDestroy()
        {
            EventsUnsubscribe();
        }
        #endregion

        #region Functions
        public void StartGame()
        {
            BoardManager.I.EmptyProjectsContainer();
            StateHandler.StartGame(this);
        }

        public void EndGame()
        {
            StateHandler.EndGame();
        }

        public void PauseGame()
        {
            StateHandler.PauseGame();
        }

        private void OnScoreUpdate(int score)
        {
            UIGame.SetScoreUI(score);
        }

        private void EventsSubscribe()
        {
            Scorer.OnScoreUpdate += OnScoreUpdate;
        }

        private void EventsUnsubscribe()
        {
            Scorer.OnScoreUpdate -= OnScoreUpdate;
        }
        #endregion
    }
}
