using DG.Tweening;
using Lean.Touch;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb.lanoria
{
    public enum GameplayState
    {
        None, // no state defined
        Tutorial,
        Intro, // intro state, here efx like camera on regions are played
        Setup, // setup here we set things useful for gameplay
        Drawing, // draw step
        Play, // here we play!
        Score, // counting the score after a tile is placed
        ShowResult, // show the final score
        End, // end the game
        ForceEnd, // force the end game
        Pause // game is in pause
    }

    public enum TutorialStep
    {
        None,
        Drag, // how to drag a tile
        Rotate, // how to rotate
        Confirm, // how to confirm
        PointsSynergy, // how sinergy points are calculated
        PointsArea
    }

    public class GameFSM : MonoBehaviour
    {
        [Required]
        public GameObject CardPrefab;

        private int currentCardIndex;
        private bool displayTutorial;
        public Tile currentTile { get; private set; }
        public ProjectData CurrentProjectData { get; private set; }
        private List<Card> cardsInHand;
        private List<Tile> projectTiles;
        private UI_Gameplay UIGame;

        public delegate void GameplayStateEvent(GameplayState state);
        public GameplayStateEvent OnStateUpdate;
        public GameplayState state { get; private set; }

        void Start()
        {
            state = GameplayState.None;
            ResetValues();
        }

        private void OnDestroy()
        {
            EventsUnsubscribe();
        }

        public void StartGame()
        {
            displayTutorial = AppManager.I.AppSettings.TutorialEnabled;
            Debug.Log("Start here!");
            UIGame = UI_manager.I.PanelGameplay;
            BoardManager.I.EmptyProjectsContainer();
            CameraManager.I.StopMoveHome();
            if (AppManager.I.AppSettings.TutorialEnabled) {
                SetState(GameplayState.Tutorial);
            } else {
                SetState(GameplayState.Intro);
            }
        }

        public void ForceEndGame()
        {
            SetState(GameplayState.ForceEnd);
        }

        public void PauseGame()
        {
            SetState(GameplayState.Pause);
        }

        public void ExitGame()
        {
            SetState(GameplayState.None);
        }

        public void ResumeGame()
        {
            SetState(GameplayState.Play);
        }

        public void OnClickCard(ProjectData projectData, int cardIndex)
        {
            DestroyCurrentTile();

            foreach (var card in cardsInHand) {
                card.DoSelect(cardIndex == card.CardIndex);
            }

            CleanPointsPreview();
            currentCardIndex = cardIndex;
            CurrentProjectData = projectData;
            var texture = UICameraManager.I.GetUICameraTexture(currentCardIndex);
            UIGame.CardSelectionHUD(projectData.Title, texture);
        }

        public void ConfirmCurrentTile()
        {
            SoundManager.I.PlaySfx(AudioEnum.tile_confirmed);
            currentTile.OnTileConfirmed();
            CloseTutorialStep(TutorialStep.Confirm, 1);
            CloseTutorialStep(TutorialStep.PointsSynergy, 0);
            CloseTutorialStep(TutorialStep.PointsArea, 0);
            currentTile.transform.parent = BoardManager.I.ProjectsContainer.transform;
            ScoreManager.I.UpdateScore(currentTile);
            WallManager.I.ResetAllWalls();
            ResetProjectPanel();
            ResetValues();
            CleanPointsPreview();

            SetState(GameplayState.Score);
        }

        public void ResetProjectPanel()
        {
            UIGame.ResetProjectPanel();
        }

        public void OnProjectSelect()
        {
            if (currentTile != null) {
                UIGame.SlideToOriginalPosition();
            }
        }

        public void CleanPointsPreview()
        {
            PointsPreviewManager.I.CleanPreview();
        }

        public Tile GetTileByCardIndex(int index)
        {
            if (index >= 0 && index < projectTiles.Count) {
                return projectTiles[index];
            }

            return null;
        }

        public Card GetCard(int index)
        {
            if (index >= 0 && index < projectTiles.Count) {
                return cardsInHand[index];
            }
            return null;
        }

        public void PlayCardDebug(Tile tile)
        {
            currentTile = tile;
            ConfirmCurrentTile();
        }

        public void DebugEndGame()
        {
            SetState(GameplayState.ShowResult);
        }

        public void ForceTutorialClose()
        {
            displayTutorial = false;
            TutorialManager.I.Close();
            DestroyCurrentTile();
        }

        private void SetState(GameplayState newState)
        {
            if (state != newState) {
                ExitState();
                state = newState;
                EnterState();

                OnStateUpdate?.Invoke(state);
            }
        }

        private void ResetValues()
        {
            currentCardIndex = -1;
            currentTile = null;
            cardsInHand = new List<Card>();
            projectTiles = new List<Tile>();
        }

        public void OnPrefabSpawned(Tile tile)
        {
            currentTile = tile;
            UIGame.EnableFingerCanvas(false);
            SubscribeToPlaceableEvents();
        }

        private void OnPrefabSelect()
        {
            CameraManager.I.EnableCameraMove(false);
            //UIGame.PrefabSelectionHUD();
        }

        private void OnPrefabRelease()
        {
            //ShowTutorial();
            CloseTutorialStep(TutorialStep.Drag, 1);
            ShowTutorialStep(TutorialStep.Rotate, 1);
        }

        private void OnPrefabRotate()
        {
            CloseTutorialStep(TutorialStep.Rotate, 1);
            ShowTutorialStep(TutorialStep.Confirm, 1);
        }

        private void OnHexPosChange()
        {
            if (currentTile != null) {
                if (currentTile.IsValidPosition) {
                    PointsPreviewManager.I.PreviewScore(currentTile);
                } else {
                    PointsPreviewManager.I.CleanInvalidPosition();
                }
                HandleBtnConfirm();
            }
        }

        private void SubscribeToPlaceableEvents()
        {
            if (currentTile != null) {
                // Debug.Log("subscribe to: " + currentTile);
                //currentTile.OnValidPositionChange += HandleBtnConfirm;
                currentTile.OnStopUsingMe += StopUsingPlaceable;
                currentTile.OnSelectMe += OnPrefabSelect;
                currentTile.OnReleaseMe += OnPrefabRelease;
                currentTile.OnRotateMe += OnPrefabRotate;
                currentTile.OnHexPosChange += OnHexPosChange;
            }
        }

        private void UnsuscribeToPlaceableEvents()
        {
            if (currentTile != null) {
                // Debug.Log("unsubscribe to: " + currentTile);
                //currentTile.OnValidPositionChange -= HandleBtnConfirm;
                currentTile.OnStopUsingMe -= StopUsingPlaceable;
                currentTile.OnSelectMe -= OnPrefabSelect;
                currentTile.OnReleaseMe -= OnPrefabRelease;
                currentTile.OnRotateMe -= OnPrefabRotate;
                currentTile.OnHexPosChange -= OnHexPosChange;
            }
        }

        private void HandleBtnConfirm()
        {
            if (currentTile != null) {
                bool enable = true;
                if (displayTutorial) {
                    enable = currentTile.IsValidPosition && TutorialManager.I.IsExplanationCompleted(TutorialStep.Rotate);
                } else {
                    enable = currentTile.IsValidPosition;
                }
                
                UIGame.EnableBtnConfirm(enable);
            }
        }

        private void StopUsingPlaceable()
        {
            UnsuscribeToPlaceableEvents();
            UIGame.EnableFingerCanvas(true);
        }

        private void DestroyCurrentTile()
        {
            if (currentTile != null) {
                UnsuscribeToPlaceableEvents();
                Destroy(currentTile.gameObject);
            }
        }

        private void EmptyHand()
        {
            cardsInHand = new List<Card>();

            var cards = UIGame.CardsInUI();
            for (int i = 0; i < cards.Count; i++) {
                Destroy(cards[i]);
            }
        }

        /// <summary>
        /// Draw a new hand deleting the current cards in hand.
        /// </summary>
        private void DrawNewHand()
        {
            EmptyHand();
            projectTiles = new List<Tile>();
            var projectsData = DeckManager.I.DiscardAndGetNewHand(CurrentProjectData);
            if (projectsData.Count > 0) {
                int cardIndex = 0;
                foreach (var projectData in projectsData) {
                    // spawn the card inside the container
                    var cardInstance = Instantiate(CardPrefab, UIGame.GetHook(cardIndex).transform);
                    var card = cardInstance.GetComponent<Card>();
                    if (card != null) {
                        cardsInHand.Add(card);
                        card.HideInBottomScreen();
                        var cardTexture = UICameraManager.I.GetUICameraTexture(cardIndex);
                        var tilePrefab = GameData.I.Projects.GetTile(projectData);
                        card.Init(projectData, cardTexture, tilePrefab, cardIndex); // initialize the card component
                        int indexToPass = cardIndex;
                        //card.SetEvents(() => OnClickCard(tilePrefab.transform, projectData, indexToPass));
                        // spawn the object in camera UI
                        var UIPrefab = UICameraManager.I.SpawnPrefabInCamera(cardIndex, tilePrefab, projectData);
                        card.SetPlayable(IsTilePlaceableOnBoard(UIPrefab));
                    }
                    cardIndex++;
                }
                SoundManager.I.PlaySfx(AudioEnum.shuffle);
            } else {
                Debug.Log("End game");
                // NO MORE CARDS - GAME ENDS
                SetState(GameplayState.ShowResult);
            }
        }

        private bool IsTilePlaceableOnBoard(GameObject tilePrefab)
        {
            var tile = tilePrefab.GetComponent<Tile>();
            if (tile != null) {
                projectTiles.Add(tile);
                return GridManager.I.CanProjectBePlaced(tile);
            }
            return false;
        }

        private void CheckGameEnd()
        {
            bool cardAvailable = false;
            foreach (var card in cardsInHand) {
                if (card.IsPlayable()) {
                    cardAvailable = true;
                    break;
                }
            }
            //            Debug.Log("Cards available: " + cardAvailable);
            if (!cardAvailable) {
                SetState(GameplayState.ShowResult);
            } else {
                SetState(GameplayState.Play);
                UIGame.SetCanvasInteractable(true);
            }
        }

        private void EventsSubscribe()
        {
            //UIGame.OnCurrentProjectSelected += OnProjectSelect;
            UIGame.BtnConfirm.onClick.AddListener(() => ConfirmCurrentTile());
        }

        private void EventsUnsubscribe()
        {
            //UIGame.OnCurrentProjectSelected -= OnProjectSelect;
            if (UIGame != null && UIGame.BtnConfirm != null) {
                UIGame.BtnConfirm.onClick.RemoveAllListeners();
            }
        }

        private void CardEntrance()
        {
            UIGame.SetCanvasInteractable(false);
            float duration = GameplayConfig.I.CardAppearsTime;
            Sequence mySequence = DOTween.Sequence();
            float index = 0;
            foreach (var card in cardsInHand) {
                float time = (duration / 2.0f) * index++;
                mySequence.Insert(time,
                    card.Rect.DOAnchorPosY(0.0f, duration).OnComplete(() => SoundManager.I.PlaySfx(AudioEnum.card))
                );
            }

            mySequence.PrependInterval(duration);
            mySequence.AppendCallback(() => {
                CheckGameEnd();
            });
        }

        private void EndSetup()
        {
            SetState(GameplayState.Drawing);
        }

        private void ShowTutorialStep(TutorialStep tutorialKey, int turnValidity)
        {
            if (displayTutorial) {
                TutorialManager.I.ShowTutorialStep(tutorialKey, turnValidity);
            }
        }

        private void CloseTutorialStep(TutorialStep tutorialKey, int turnValidity)
        {
            if (displayTutorial) {
                TutorialManager.I.CloseTutorialStep(tutorialKey, turnValidity);
            }
        }

        private void EndTutorial()
        {
            SetState(GameplayState.Intro);
        }

        private void EnterState()
        {
            switch (state) {
                default:
                case GameplayState.None:
                    break;
                case GameplayState.Tutorial:
                    UIGame.EnableCanvas(false);
                    TutorialManager.I.BeginTutorial(EndTutorial);
                    TutorialManager.I.ShowNextIntroduction();
                    break;
                case GameplayState.Intro:
                    UI_manager.I.Show(UI_manager.States.Play);
                    UIGame.SetCanvasAlpha(0.0f);
                    SetState(GameplayState.Setup);
                    break;
                case GameplayState.Setup:
                    UIGame.ScoreUI.Init(0);
                    TileManager.I.Clean();
                    DeckManager.I.PrepareNewDeck();
                    GridManager.I.InitCells();
                    UI_manager.I.PanelGameResults.SetTotalScore(0);
                    ResetProjectPanel();
                    float duration = GameplayConfig.I.FadeInGameCanvas;
                    UIGame.FadeCanvas(1.0f, duration, () => EndSetup());
                    break;
                case GameplayState.Drawing:
                    DrawNewHand();
                    CardEntrance();
                    break;
                case GameplayState.Play:
                    CameraManager.I.EnableCameraMove(true);
                    break;
                case GameplayState.Score:
                    CleanPointsPreview();
                    TutorialManager.I.EndTurn();
                    SetState(GameplayState.Drawing);// handle score efx
                    break;
                case GameplayState.ShowResult:
                    UI_manager.I.ShowGamePlay(false);
                    UI_manager.I.PanelGameResults.SetAreaScore(ScoreManager.I.AreaScore);
                    UI_manager.I.PanelGameResults.SetAdjacencyScore(ScoreManager.I.AdjacencyScore);
                    UI_manager.I.PanelGameResults.SetPlacementScore(ScoreManager.I.PlacementScore);
                    int emptyCells = GridManager.I.FreeCells() * -1;
                    UI_manager.I.PanelGameResults.SetEmptyScore(emptyCells);
                    int totalScore = ScoreManager.I.Score + emptyCells;
                    bool isNewHiScore = ScoreManager.I.IsNewHiScore(totalScore);
                    ScoreManager.I.SaveHiScore(totalScore);
                    UI_manager.I.PanelGameResults.SetTotalScore(totalScore);
                    UI_manager.I.PanelGameResults.ShowNewHiScore(isNewHiScore);
                    UI_manager.I.ShowGameResult(true);
                    SetState(GameplayState.End);
                    break;
                case GameplayState.End:
                    CameraManager.I.ResetCameraGameplay();
                    break;
                case GameplayState.ForceEnd:
                    CameraManager.I.ResetCameraGameplay();
                    SetState(GameplayState.None);
                    break;
                case GameplayState.Pause:
                    break;
            }
        }

        private void ExitState()
        {
            switch (state) {
                default:
                case GameplayState.None:
                    break;
                case GameplayState.Tutorial:
                    TutorialManager.I.Close();
                    UIGame.EnableCanvas(true);
                    break;
                case GameplayState.Intro:
                    EventsSubscribe();
                    break;
                case GameplayState.Setup:
                    break;
                case GameplayState.Drawing:
                    ShowTutorialStep(TutorialStep.Drag, 1); // show how to drag a project
                    break;
                case GameplayState.Play:
                    CameraManager.I.EnableCameraMove(false);
                    break;
                case GameplayState.Score:
                    break;
                case GameplayState.ShowResult:
                    break;
                case GameplayState.ForceEnd:
                case GameplayState.End:
                    EventsUnsubscribe();
                    EmptyHand();
                    DestroyCurrentTile();
                    CleanPointsPreview();
                    BoardManager.I.EmptyProjectsContainer();
                    WallManager.I.ResetAllWalls();
                    PointsPreviewManager.I.ResetAll();
                    ScoreManager.I.ResetScore();
                    TutorialManager.I.ResetTutorial();
                    UI_manager.I.ShowGamePause(false);
                    UI_manager.I.ShowGameResult(false);
                    AppManager.I.OnHome();
                    break;
                case GameplayState.Pause:
                    break;
            }
        }
    }
}
