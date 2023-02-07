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
        public PointsPreviewManager Preview;

        private UI_Gameplay UIGame;
        private LeanSpawnWithFinger spawner;

        void Start()
        {
            UIGame = UI_manager.I.PanelGameplay;
        }

        public void StartGame()
        {
            GameFSM.StartGame();
        }

        public void ForceEndGame()
        {
            GameFSM.ForceEndGame();
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
            GameFSM.ExitGame();            
        }

        #region Debug and Editor Methods

        public void AutomaticPlayCard(int whichCard)
        {
            if (GameFSM.state == GameplayState.Play) {
                //                Debug.Log("Simulate Playing Card " + whichCard);
                var card = GameFSM.GetCard(whichCard - 1); // get card
                                                           //                Debug.Log(card.Project);
                var foundLocation = new TileLocation();
                var shape = card.TilePrefab.GetComponent<Tile>().ShapePath;
                if (GridManager.I.GetGoodTileLocation(shape, out foundLocation)) {
                    var tileInstance = Instantiate(card.TilePrefab); // instantiate project
                    var tileToPlace = tileInstance.GetComponent<Tile>();
                    tileToPlace.ManualSetPosition(foundLocation.Position.ToWorld(), foundLocation.Direction);
                    tileToPlace.SetupCellsColor(card.Project);
                    GameFSM.PlayCardDebug(tileToPlace);
                } else {
                    Debug.Log("NOT FOUND ANY POSITION");
                }
            }
        }

        public void AutomaticEndGame()
        {
            GameFSM.DebugEndGame();
        }

        #endregion
    }
}
