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

        private UI_Gameplay UIGame;
        private LeanSpawnWithFinger spawner;

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            UIGame = UI_manager.I.PanelGameplay;
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
            UI_manager.I.ShowGamePause(true);
        }

        public void ResumeGame()
        {
            StateHandler.ResumeGame();
            UI_manager.I.ShowGamePause(false);
        }

        public void ExitGame()
        {
            StateHandler.EndGame();
            UI_manager.I.ShowGamePause(false);
            UI_manager.I.Show(UI_manager.States.Home);
        }

        private void OnScoreUpdate(int score, int points)
        {
            UIGame.SetScoreUI(score, points);
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
