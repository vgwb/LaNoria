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
        End,
        Pause
    }

    public class GameplayBehaviour : MonoBehaviour
    {
        #region Var
        public GameObject CardPrefab;
        public delegate void GameplayStateEvent(GameplayState state);
        public GameplayStateEvent OnStateUpdate;

        [SerializeField] private GameplayState state;
        private int chosenCardIndex;
        private Placeable instancedPlaceable;
        private ProjectData chosenProjectData;
        private List<CardInGame> spawnedCards;
        private UI_GameHUD UIGame;
        private LeanSpawnWithFinger spawner;
        private CardDealer dealer;
        private ScoreManager scorer;
        #endregion

        #region MonoB
        void Awake()
        {
            if (CardPrefab == null) {
                Debug.LogError("CardDealer - Awake(): no card prefab defined!");
            }

            state = GameplayState.None;
            ResetValues();
        }

        private void OnDestroy()
        {
            EventsUnsubscribe();
        }
        #endregion

        #region Functions
        public void StartGame(GameplayManager manager)
        {
            dealer = manager.Dealer;
            scorer = manager.Scorer;
            UIGame = UI_manager.I.PanelGameHUD;
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

        public void OnClickCard(Transform placeablePrefab, ProjectData projectData, int cardIndex)
        {
            if (placeablePrefab != null && spawner != null) {
                if (instancedPlaceable != null) {
                    Destroy(instancedPlaceable.gameObject);
                }

                chosenCardIndex = cardIndex;
                chosenProjectData = projectData;
                spawner.Prefab = placeablePrefab;
                var texture = UICameraManager.I.GetUICameraTexture(chosenCardIndex);
                UIGame.CardSelectionHUD(projectData.Title, texture);
            }
        }

        public void ConfirmProject()
        {
            instancedPlaceable.OnProjectConfirmed();
            instancedPlaceable.transform.parent = BoardManager.I.ProjectsContainer.transform;
            scorer.UpdateScore(instancedPlaceable);
            ResetProjectPanel();
            ResetValues();

            SetState(GameplayState.Drawing);
        }

        public void ResetProjectPanel()
        {
            spawner.Prefab = null;
            UIGame.ResetProjectPanel();
        }

        public void OnProjectDrag()
        {
            if (instancedPlaceable != null) {
                UIGame.SlideOnTheRight();
            }
        }

        public void OnProjectSelect()
        {
            if (instancedPlaceable != null) {
                UIGame.SlideToOriginalPosition();
            }
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
            chosenCardIndex = -1;
            instancedPlaceable = null;
            chosenProjectData = null;
            spawnedCards = new List<CardInGame>();
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
            UIGame.PrefabSelectionHUD();
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
            var projectsData = dealer.DrawProjects();
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
                    cardComp.SetCardEvents(() => OnClickCard(associatedPrefab.transform, projectData, indexToPass));
                    // spawn the object in camera UI
                    UICameraManager.I.SpawnPrefabInCamera(cardIndex, associatedPrefab, projectData);
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
                    UIGame.SetScoreUI(0);
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
                case GameplayState.End:
                    break;
                case GameplayState.Pause:
                    break;
            }
        }
        #endregion
    }
}
