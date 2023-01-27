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
        public CardDealer Dealer;
        public ScoreManager Scorer;
        public GameplayBehaviour StateHandler;
        public PreviewManager Preview;

        private UI_GameHUD UIGame;
        private LeanSpawnWithFinger spawner;

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            UIGame = UI_manager.I.PanelGameHUD;
            EventsSubscribe();
        }

        private void OnDestroy()
        {
            EventsUnsubscribe();
        }

        public void StartGame()
        {
            BoardManager.I.EmptyProjectsContainer();
            StateHandler.StartGame();
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
    }
}
