using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb.lanoria
{
    public class GameplayManager : MonoBehaviour
    {
        #region Var
        [Header("Other Ref")]
        public UI_PanelGameHUD UIGame;
        public LeanSpawnWithFinger Spawner;
        public LeanFingerDownCanvas FingerCanvas;
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

        private Placeable instancedPlaceable;
        private ProjectData chosenProjectData;
        #endregion

        #region MonoB
        void Awake()
        {
            if (CardPrefab == null) {
                Debug.LogError("CardDealer - Awake(): no card prefab defined!");
            }
            instancedPlaceable = null;
            chosenProjectData = null;
            UIGame.EnableBtnConfirm(false);
            UIGame.SetProjectTitle("");
            EventsSubscribe();
        }

        private void Start()
        {
            UIGame.SetScoreUI(0);
            UIGame.SlideOnTheRight();
            DrawNewHand();
        }

        private void OnDestroy()
        {
            EventsUnsubscribe();
        }
        #endregion

        #region Functions
        public void ChosePrefab(Transform placeablePrefab, ProjectData projectData, int cardIndex)
        {
            if (placeablePrefab != null && Spawner != null) {
                if (instancedPlaceable != null) {
                    Destroy(instancedPlaceable.gameObject);
                }
                chosenProjectData = projectData;
                Spawner.Prefab = placeablePrefab;
                UIGame.SetProjectTitle(projectData.Title);
                UIGame.SlideToOriginalPosition();
                var texture = UICameraManager.I.GetUICameraTexture(cardIndex);
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
            Spawner.Prefab = null;
            UIGame.SetProjectTitle("");
            UIGame.EnableBtnConfirm(false);
            UIGame.SetupCurrentProjectImg(null);
            DrawNewHand();

            if (OnProjectConfirmed != null) {
                OnProjectConfirmed(instancedPlaceable);
            }

            Scorer.UpdateScore(instancedPlaceable);
            instancedPlaceable = null;
            chosenProjectData = null;
        }

        public void ResetDetailPanel()
        {
            Spawner.Prefab = null;
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
            EnableFingerCanvas(false);
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
            EnableFingerCanvas(true);
        }

        private void EnableSpawner(bool enable)
        {
            if (Spawner != null) {
                Spawner.enabled = enable;
            }
        }

        private void EnableFingerCanvas(bool enable)
        {
            if (FingerCanvas != null) {
                FingerCanvas.enabled = enable;
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
                    string modelKey = projectData.Model;
                    var associatedPrefab = GameplayConfig.I.GetProjectModelByKey(modelKey);
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
            Spawner.OnSpawnedClone += OnPrefabSpawned;
            Scorer.OnScoreUpdate += OnScoreUpdate;
        }

        private void EventsUnsubscribe()
        {
            Spawner.OnSpawnedClone -= OnPrefabSpawned;
            Scorer.OnScoreUpdate -= OnScoreUpdate;
        }
        #endregion
    }
}
