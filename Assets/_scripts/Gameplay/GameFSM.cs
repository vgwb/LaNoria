using DG.Tweening;
using Lean.Touch;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb.lanoria
{
    public enum GameplayState
    {
        None, // no state defined
        Intro, // intro state, here efx like camera on regions are played
        Setup, // setup here we set things useful for gameplay
        Drawing, // draw step
        Play, // here we play!
        Score, // counting the score after a tile is placed
        End, // end the game and show the final score
        Pause // game is in pause
    }

    public class GameFSM : MonoBehaviour
    {
        public GameObject CardPrefab;
        public delegate void GameplayStateEvent(GameplayState state);
        public GameplayStateEvent OnStateUpdate;

        public GameplayState state { get; private set; }
        private int currentCardIndex;
        private Tile currentTile;

        public ProjectData currentProjectData
        {
            get; private set;
        }

        private List<Card> cardsInHand;
        private List<Tile> projectTiles;
        private UI_Gameplay UIGame;
        private LeanSpawnWithFinger spawner;

        void Start()
        {
            if (CardPrefab == null) {
                Debug.LogError("GameFSM - Awake(): no card prefab defined!");
            }

            state = GameplayState.None;
            ResetValues();
        }

        private void OnDestroy()
        {
            EventsUnsubscribe();
        }

        public void StartGame()
        {
            UIGame = UI_manager.I.PanelGameplay;
            spawner = UIGame.Spawner;
            EventsSubscribe();
            SetState(GameplayState.Intro);
        }

        public void EndGame()
        {
            EventsUnsubscribe();
            SetState(GameplayState.End);
        }

        public void PauseGame()
        {
            SetState(GameplayState.Pause);
        }

        public void ResumeGame()
        {
            SetState(GameplayState.Play);
        }

        public void OnClickCard(Transform placeablePrefab, ProjectData projectData, int cardIndex)
        {
            if (placeablePrefab != null && spawner != null) {
                if (currentTile != null) {
                    Destroy(currentTile.gameObject);
                }

                CleanPreview();
                currentCardIndex = cardIndex;
                currentProjectData = projectData;
                spawner.Prefab = placeablePrefab;
                var texture = UICameraManager.I.GetUICameraTexture(currentCardIndex);
                UIGame.CardSelectionHUD(projectData.Title, texture);
            }
        }

        public void ConfirmProject()
        {
            SoundManager.I.PlaySfx(AudioEnum.tile_confirmed);
            currentTile.OnProjectConfirmed();
            currentTile.transform.parent = BoardManager.I.ProjectsContainer.transform;
            ScoreManager.I.UpdateScore(currentTile);
            ResetProjectPanel();
            ResetValues();
            CleanPreview();

            SetState(GameplayState.Score);
        }

        public void ResetProjectPanel()
        {
            spawner.Prefab = null;
            UIGame.ResetProjectPanel();
        }

        public void OnProjectDrag()
        {
            if (currentTile != null) {
                UIGame.SlideOnTheRight();
            }
        }

        public void OnProjectSelect()
        {
            if (currentTile != null) {
                UIGame.SlideToOriginalPosition();
            }
        }

        public void CleanPreview()
        {
            PreviewManager.I.CleanPreview();
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

        private void SetState(GameplayState newState)
        {
            if (state != newState) {
                state = newState;
                ReadState();

                OnStateUpdate?.Invoke(state);
            }
        }

        private void ResetValues()
        {
            currentCardIndex = -1;
            currentTile = null;
            currentProjectData = null;
            cardsInHand = new List<Card>();
            projectTiles = new List<Tile>();
        }

        private void OnPrefabSpawned(GameObject clone)
        {
            currentTile = clone.GetComponent<Tile>();
            currentTile.SetupCellsColor(currentProjectData);
            currentTile.SetupForDrag();
            UIGame.EnableFingerCanvas(false);
            SubscribeToPlaceableEvents();
        }

        private void OnPrefabSelect()
        {
            CameraManager.I.EnableRotationWithFingers(false);
            UIGame.PrefabSelectionHUD();
        }

        private void OnHexPosChange()
        {
            if (currentTile != null) {
                if (currentTile.IsValidPosition) {
                    PreviewManager.I.PreviewScore(currentTile);
                } else {
                    PreviewManager.I.CleanPreview();
                }
            }
        }

        private void SubscribeToPlaceableEvents()
        {
            if (currentTile != null) {
                currentTile.OnValidPositionChange += HandleBtnConfirm;
                currentTile.OnStopUsingMe += StopUsingPlaceable;
                currentTile.OnSelectMe += OnPrefabSelect;
                currentTile.OnHexPosChange += OnHexPosChange;
            }
        }

        private void UnsuscribeToPlaceableEvents()
        {
            if (currentTile != null) {
                currentTile.OnValidPositionChange -= HandleBtnConfirm;
                currentTile.OnStopUsingMe -= StopUsingPlaceable;
                currentTile.OnSelectMe -= OnPrefabSelect;
                currentTile.OnHexPosChange -= OnHexPosChange;
            }
        }

        private void HandleBtnConfirm()
        {
            if (currentTile != null) {
                UIGame.EnableBtnConfirm(currentTile.IsValidPosition);
            }
        }

        private void StopUsingPlaceable()
        {
            UnsuscribeToPlaceableEvents();
            UIGame.EnableFingerCanvas(true);
        }

        private void EnableSpawner(bool enable)
        {
            if (spawner != null) {
                spawner.enabled = enable;
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
            var projectsData = DeckManager.I.GetNewHand();
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
                        card.Init(projectData, cardTexture, tilePrefab); // initialize the card component
                        int indexToPass = cardIndex;
                        card.SetEvents(() => OnClickCard(tilePrefab.transform, projectData, indexToPass));
                        // spawn the object in camera UI
                        var UIPrefab = UICameraManager.I.SpawnPrefabInCamera(cardIndex, tilePrefab, projectData);
                        bool placeable = IsProjectPlaceable(UIPrefab);
                        card.SetPlayable(placeable);
                    }
                    cardIndex++;
                }

                SoundManager.I.PlaySfx(AudioEnum.shuffle);
            } else {
                // NO MORE CARDS - GAME ENDS
                SetState(GameplayState.End);
            }
        }

        private bool IsProjectPlaceable(GameObject prefabInstance)
        {
            var tile = prefabInstance.GetComponent<Tile>();
            if (tile != null) {
                projectTiles.Add(tile);
                return BoardManager.I.CanProjectBePlaced(tile);
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
                SetState(GameplayState.End);
            } else {
                SetState(GameplayState.Play);
                UIGame.SetCanvasInteractable(true);
            }
        }

        private void EventsSubscribe()
        {
            spawner.OnSpawnedClone += OnPrefabSpawned;
            UIGame.OnProjectDragged += OnProjectDrag;
            UIGame.OnCurrentProjectSelected += OnProjectSelect;
            UIGame.BtnConfirm.onClick.AddListener(() => ConfirmProject());
        }

        private void EventsUnsubscribe()
        {
            spawner.OnSpawnedClone -= OnPrefabSpawned;
            UIGame.OnProjectDragged -= OnProjectDrag;
            UIGame.OnCurrentProjectSelected += OnProjectSelect;
            UIGame.BtnConfirm.onClick.RemoveListener(() => ConfirmProject());
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
            CameraManager.I.EnableRotationWithFingers(true);
        }

        private void ReadState()
        {
            switch (state) {
                default:
                case GameplayState.None:
                    break;
                case GameplayState.Intro:
                    UI_manager.I.Show(UI_manager.States.Play);
                    UIGame.SetCanvasAlpha(0.0f);
                    CameraManager.I.EnableAutoRotate(false);
                    CameraManager.I.ResetToOriginalRotY(() => SetState(GameplayState.Setup));
                    break;
                case GameplayState.Setup:
                    UIGame.ScoreUI.Init(0);
                    //CameraManager.I.SwitchToPlayCamera();
                    DeckManager.I.PrepareNewDeck();
                    ResetProjectPanel();
                    float duration = GameplayConfig.I.FadeInGameCanvas;
                    UIGame.FadeCanvas(1.0f, duration, () => EndSetup());
                    break;
                case GameplayState.Drawing:
                    DrawNewHand();
                    CardEntrance();
                    break;
                case GameplayState.Play:
                    break;
                case GameplayState.Score:
                    CleanPreview();
                    SetState(GameplayState.Drawing);// handle score efx
                    break;
                case GameplayState.End:
                    Debug.Log("End Game!");
                    UI_manager.I.ShowGameResult(true);
                    //CameraManager.I.SwitchToMenuCamera(); // TODO: check if camera swap should gop here!
                    break;
                case GameplayState.Pause:
                    break;
            }
        }
    }
}
