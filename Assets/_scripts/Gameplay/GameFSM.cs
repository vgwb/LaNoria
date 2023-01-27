using DG.Tweening;
using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb.lanoria
{
    public enum GameplayState
    {
        None,
        Intro,
        Setup,
        Drawing,
        Play,
        Score,
        End,
        Pause
    }

    public class GameFSM : GameplayComponent
    {
        public GameObject CardPrefab;
        public delegate void GameplayStateEvent(GameplayState state);
        public GameplayStateEvent OnStateUpdate;

        [SerializeField] private GameplayState state;
        private int currentCardIndex;
        private Tile currentTile;

        public ProjectData currentProjectData
        {
            get; private set;
        }

        private List<Card> spawnedCards;
        private UI_Gameplay UIGame;
        private LeanSpawnWithFinger spawner;
        private CardDealer dealer;
        private ScoreManager scorer;
        private PreviewManager preview;

        protected override void Awake()
        {
            base.Awake();

            if (CardPrefab == null) {
                Debug.LogError("CardDealer - Awake(): no card prefab defined!");
            }

            dealer = manager.Dealer;
            scorer = manager.Scorer;
            preview = manager.Preview;

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
            currentTile.OnProjectConfirmed();
            currentTile.transform.parent = BoardManager.I.ProjectsContainer.transform;
            scorer.UpdateScore(currentTile);
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
            preview.CleanPreview();
        }

        private void SetState(GameplayState newState)
        {
            if (state != newState) {
                state = newState;
                ReadState();

                if (OnStateUpdate != null) {
                    OnStateUpdate(state);
                }
            }
        }

        private void ResetValues()
        {
            currentCardIndex = -1;
            currentTile = null;
            currentProjectData = null;
            spawnedCards = new List<Card>();
        }

        private void OnPrefabSpawned(GameObject clone)
        {
            currentTile = clone.GetComponent<Tile>();
            currentTile.SetupCellsColor(currentProjectData);
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
                preview.PreviewScore(currentTile);
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

        /// <summary>
        /// Clean the actual hand of cards.
        /// </summary>
        private void CleanHand()
        {
            var cards = UIGame.CardsInUI();
            for (int i = 0; i < cards.Count; i++) {
                Destroy(cards[i]);
            }
        }

        /// <summary>
        /// Draw a new hand deleting the actual cards in hand.
        /// </summary>
        private void DrawNewHand()
        {
            CleanHand();
            spawnedCards = new List<Card>();
            var projectsData = dealer.DrawProjects();
            int cardIndex = 0;
            foreach (var projectData in projectsData) {
                // spawn the card inside the container
                var cardInstance = Instantiate(CardPrefab, UIGame.GetHook(cardIndex).transform);
                var card = cardInstance.GetComponent<Card>();
                if (card != null) {
                    spawnedCards.Add(card);
                    card.HideInBottomScreen();
                    var cardTexture = UICameraManager.I.GetUICameraTexture(cardIndex);
                    card.Init(projectData, cardTexture); // initialize the card component
                    // get the associated model and bind it to the card clickable area
                    var tilePrefab = GameData.I.Projects.GetTile(projectData);
                    int indexToPass = cardIndex;
                    card.SetEvents(() => OnClickCard(tilePrefab.transform, projectData, indexToPass));
                    // spawn the object in camera UI
                    UICameraManager.I.SpawnPrefabInCamera(cardIndex, tilePrefab, projectData);
                }
                cardIndex++;
            }

            SoundManager.I.PlaySfx(SfxEnum.shuffle);
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
            foreach (var card in spawnedCards) {
                float time = (duration / 2.0f) * index++;
                mySequence.Insert(time, card.Rect.DOAnchorPosY(0.0f, duration).OnComplete(() => SoundManager.I.PlaySfx(SfxEnum.card)));
            }

            mySequence.PrependInterval(duration);
            mySequence.AppendCallback(() => {
                SetState(GameplayState.Play);
                UIGame.SetCanvasInteractable(true);
            });
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
                    ResetProjectPanel();
                    float duration = GameplayConfig.I.FadeInGameCanvas;
                    UIGame.FadeCanvas(1.0f, duration, () => SetState(GameplayState.Drawing));
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
                    break;
                case GameplayState.Pause:
                    break;
            }
        }
    }
}
