using Lean.Touch;
using System;
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
        public GameObject CardPrefab;
        [Header("Score")]
        public ScoreManager Scorer;

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
            UI_manager.I.Show(UI_manager.States.Play);
            UIGame.SetScoreUI(0);
            UIGame.SlideOnTheRight();
            UIGame.EnableBtnConfirm(false);
            UIGame.SetProjectTitle("");
            DrawNewHand();
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
            spawner.Prefab = null;
            UIGame.SetProjectTitle("");
            UIGame.EnableBtnConfirm(false);
            UIGame.SetupCurrentProjectImg(null);
            DrawNewHand();

            if (OnProjectConfirmed != null) {
                OnProjectConfirmed(instancedPlaceable);
            }

            Scorer.UpdateScore(instancedPlaceable);
            chosenCardIndex = -1;
            instancedPlaceable = null;
            chosenProjectData = null;
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
            var projectsData = Dealer.DrawProjects();
            int cardIndex = 0;
            foreach (var projectData in projectsData) {
                var cardInstance = Instantiate(CardPrefab, UIGame.CardContainer); // spawn the card inside the container
                var cardComp = cardInstance.GetComponent<CardInGame>();
                if (cardComp != null) {
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
        }

        private void EventsUnsubscribe()
        {
            spawner.OnSpawnedClone -= OnPrefabSpawned;
            Scorer.OnScoreUpdate -= OnScoreUpdate;
            UIGame.OnProjectDragged -= OnProjectDrag;
            UIGame.BtnConfirm.onClick.RemoveListener(() => ConfirmProject());
        }
        #endregion
    }
}
