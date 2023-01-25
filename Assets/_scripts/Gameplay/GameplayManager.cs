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
        [Header("Test to remove")] // TODO: remove sounds from here!
        public AudioClip ShuffleClip; // this...
        public AudioClip CardClip; // ... and this
        [Header("Cards")]
        public CardDealer Dealer;
        public GameObject CardPrefab;
        [Header("Score")]
        public ScoreManager Scorer;
        [Header("State")]
        public GameplayStateHandler StateHandler;

        public delegate void GameplayEvent();
        public GameplayEvent OnHandDrawed;
        public GameplayEvent OnProjectChosen;
        public delegate void GameplayEventOneParam(Placeable placeable);
        public GameplayEventOneParam OnProjectConfirmed;

        private int chosenCardIndex;
        private UI_GameHUD UIGame;
        private LeanSpawnWithFinger spawner;
        private Placeable instancedPlaceable;
        private ProjectData chosenProjectData;
        private List<CardInGame> spawnedCards;
        #endregion

        #region MonoB
        protected override void Awake()
        {
            base.Awake();

            if (CardPrefab == null) {
                Debug.LogError("CardDealer - Awake(): no card prefab defined!");
            }

            chosenCardIndex = -1;
            instancedPlaceable = null;
            chosenProjectData = null;
            spawnedCards = new List<CardInGame>();
        }

        private void Start()
        {
            UIGame = UI_manager.I.PanelGameHUD;
            spawner = UIGame.Spawner;
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
            SetState(GameplayState.Intro);
        }

        public void ChosePrefab(Transform placeablePrefab, ProjectData projectData, int cardIndex)
        {
            if (placeablePrefab != null && spawner != null) {
                if (instancedPlaceable != null) {
                    Destroy(instancedPlaceable.gameObject);
                }

                chosenCardIndex = cardIndex;
                chosenProjectData = projectData;
                spawner.Prefab = placeablePrefab;
                UIGame.SetProjectTitle(projectData.Title);
                UIGame.SlideToOriginalPosition();
                UIGame.EnableBtnConfirm(false);
                var texture = UICameraManager.I.GetUICameraTexture(chosenCardIndex);
                UIGame.SetupCurrentProjectImg(texture);
                UIGame.EnableCurrentProjectImg(true);

                if (OnProjectChosen != null) {
                    OnProjectChosen();
                }
            }
        }

        public void ConfirmProject()
        {
            instancedPlaceable.OnProjectConfirmed();
            instancedPlaceable.transform.parent = BoardManager.I.ProjectsContainer.transform;
            spawner.Prefab = null;
            UIGame.SetProjectTitle("");
            UIGame.EnableBtnConfirm(false);
            UIGame.SetupCurrentProjectImg(null);
            UIGame.SlideOnTheRight();
            Scorer.UpdateScore(instancedPlaceable);
            chosenCardIndex = -1;
            instancedPlaceable = null;
            chosenProjectData = null;

            if (OnProjectConfirmed != null) {
                OnProjectConfirmed(instancedPlaceable);
            }

            SetState(GameplayState.Drawing);
        }

        public void ResetDetailPanel()
        {
            spawner.Prefab = null;
        }

        public void OnProjectDrag()
        {
            if (instancedPlaceable != null) {
                UIGame.SlideOnTheRight();
            }
        }

        private void OnPrefabSpawned(GameObject clone)
        {
            instancedPlaceable = clone.GetComponent<Placeable>();
            instancedPlaceable.SetupCellsColor(chosenProjectData);
            UIGame.EnableFingerCanvas(false);
            SubscribeToPlaceableEvents();
        }

        private void OnPrefabSelect()
        {
            CameraManager.I.EnableRotationWithFingers(false);
            UIGame.EnableCurrentProjectImg(false);
            UIGame.SlideOnTheRight();
        }

        private void OnScoreUpdate(int score)
        {
            UIGame.SetScoreUI(score);
        }

        private void SubscribeToPlaceableEvents()
        {
            if (instancedPlaceable != null) {
                instancedPlaceable.OnValidPositionChange += HandleBtnConfirm;
                instancedPlaceable.OnStopUsingMe += StopUsingPlaceable;
                instancedPlaceable.OnSelectMe += OnPrefabSelect;
            }
        }

        private void UnsuscribeToPlaceableEvents()
        {
            if (instancedPlaceable != null) {
                instancedPlaceable.OnValidPositionChange -= HandleBtnConfirm;
                instancedPlaceable.OnStopUsingMe -= StopUsingPlaceable;
                instancedPlaceable.OnSelectMe -= OnPrefabSelect;
            }
        }

        private void HandleBtnConfirm()
        {
            if (instancedPlaceable != null) {
                UIGame.EnableBtnConfirm(instancedPlaceable.IsValidPosition);
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
            spawnedCards = new List<CardInGame>();
            var projectsData = Dealer.DrawProjects();
            int cardIndex = 0;
            foreach (var projectData in projectsData) {
                var cardInstance = Instantiate(CardPrefab, UIGame.GetHook(cardIndex).transform); // spawn the card inside the container
                var cardComp = cardInstance.GetComponent<CardInGame>();
                if (cardComp != null) {
                    spawnedCards.Add(cardComp);
                    cardComp.HideInBottomScreen();
                    var cardTexture = UICameraManager.I.GetUICameraTexture(cardIndex);
                    cardComp.InitCard(projectData, cardTexture); // initialize the card component
                    // get the associated model and bind it to the card clickable area
                    var associatedPrefab = GameplayConfig.I.GetProjectModelFromData(projectData);
                    int indexToPass = cardIndex;
                    cardComp.SetCardEvents(() => ChosePrefab(associatedPrefab.transform, projectData, indexToPass));
                    // spawn the object in camera UI
                    UICameraManager.I.SpawnPrefabInCamera(cardIndex, associatedPrefab, projectData);
                }
                cardIndex++;
            }

            SoundManager.I.PlaySfx(SfxEnum.shuffle);

            if (OnHandDrawed != null) {
                OnHandDrawed();
            }
        }

        private void EventsSubscribe()
        {
            spawner.OnSpawnedClone += OnPrefabSpawned;
            Scorer.OnScoreUpdate += OnScoreUpdate;
            UIGame.OnProjectDragged += OnProjectDrag;
            UIGame.BtnConfirm.onClick.AddListener(() => ConfirmProject());
            StateHandler.OnStateUpdate += ReadState;
        }

        private void EventsUnsubscribe()
        {
            spawner.OnSpawnedClone -= OnPrefabSpawned;
            Scorer.OnScoreUpdate -= OnScoreUpdate;
            UIGame.OnProjectDragged -= OnProjectDrag;
            UIGame.BtnConfirm.onClick.RemoveListener(() => ConfirmProject());
            StateHandler.OnStateUpdate -= ReadState;
        }

        private void CardEntrance()
        {
            UIGame.SetCanvasInteractable(false);
            float duration = GameplayConfig.I.CardAppearsTime;
            Sequence mySequence = DOTween.Sequence();
            float index = 0;
            foreach (var card in spawnedCards) {
                float time = (duration/2.0f) * index++;
                mySequence.Insert(time, card.Rect.DOAnchorPosY(0.0f, duration).OnComplete(() => SoundManager.I.PlaySfx(SfxEnum.card)));
            }
            
            mySequence.PrependInterval(duration);
            mySequence.AppendCallback(() => {
                SetState(GameplayState.Play);
                UIGame.SetCanvasInteractable(true);
            });
        }

        private void ReadState(GameplayState state)
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
                    UIGame.SetScoreUI(0);
                    UIGame.SlideOnTheRight();
                    UIGame.EnableBtnConfirm(false);
                    UIGame.SetProjectTitle("");
                    float duration = GameplayConfig.I.FadeInGameCanvas;
                    UIGame.FadeCanvas(1.0f, duration, () => SetState(GameplayState.Drawing));
                    break;
                case GameplayState.Drawing:
                    DrawNewHand();
                    CardEntrance();
                    break;
                case GameplayState.Play:
                    break;
                case GameplayState.End:
                    break;
                case GameplayState.Pause:
                    break;
            }
        }

        private void SetState(GameplayState state)
        {
            if (StateHandler != null) {
                StateHandler.SetState(state);
            }
        }
        #endregion
    }
}
