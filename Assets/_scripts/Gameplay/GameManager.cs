using DG.Tweening;
using Lean.Touch;
using System;
using System.Collections.Generic;
using UnityEngine;
using vgwb.framework;

namespace vgwb.lanoria
{
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        public GameFSM GameFSM;
        public PreviewManager Preview;

        private UI_Gameplay UIGame;
        private LeanSpawnWithFinger spawner;

        void Start()
        {
            UIGame = UI_manager.I.PanelGameplay;
        }

        public void StartGame()
        {
            BoardManager.I.EmptyProjectsContainer();
            GameFSM.StartGame();
        }

        public void EndGame()
        {
            GameFSM.EndGame();
        }

        public void PauseGame()
        {
            GameFSM.PauseGame();
            UI_manager.I.ShowGamePause(true);
        }

        public void ResumeGame()
        {
            GameFSM.ResumeGame();
            UI_manager.I.ShowGamePause(false);
        }

        public void ExitGame()
        {
            GameFSM.EndGame();
            UI_manager.I.ShowGamePause(false);
            UI_manager.I.Show(UI_manager.States.Home);
        }

        #region Debug and Editor Methods

        public void DebugPlayCard(int whichCard)
        {
            if (GameFSM.state == GameplayState.Play) {
                Debug.Log("Simulate Playing Card " + whichCard);
                var tileToPlace = GameFSM.GetTileByCardIndex(whichCard - 1);
                var hexposition = new Hex(1, 2).ToWorld();
                tileToPlace.ManualSetPostion(hexposition, HexDirection.E);
            }
        }

        #endregion
    }
}
