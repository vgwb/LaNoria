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
        private Placeable instancedPlaceable;
        #endregion

        #region MonoB
        void Awake()
        {
            if (CardPrefab == null) {
                Debug.LogError("CardDealer - Awake(): no card prefab defined!");
            }
            instancedPlaceable = null;
            UIGame.EnableBtnConfirm(false);
            UIGame.SetProjectTitle("");
            Spawner.OnSpawnedClone += OnPrefabSpawned;
        }

        private void Start()
        {
            DrawNewHand();
        }

        private void OnDestroy()
        {
            Spawner.OnSpawnedClone -= OnPrefabSpawned;
        }
        #endregion

        #region Functions
        public void ChosePrefab(Transform placeablePrefab, ProjectData projectData)
        {
            if (placeablePrefab != null && Spawner != null) {
                if (instancedPlaceable != null) {
                    Destroy(instancedPlaceable.gameObject);
                }

                Spawner.Prefab = placeablePrefab;
                UIGame.SetProjectTitle(projectData.Title);
            }
        }

        public void ConfirmProject()
        {
            if (instancedPlaceable != null) {
                instancedPlaceable.OnProjectConfirmed();
                instancedPlaceable = null;
            }

            Spawner.Prefab = null;
            UIGame.SetProjectTitle("");
            UIGame.EnableBtnConfirm(false);
            DrawNewHand();
        }

        public void ResetDetailPanel()
        {
            Spawner.Prefab = null;
        }

        private void OnPrefabSpawned(GameObject clone)
        {
            instancedPlaceable = clone.GetComponent<Placeable>();
            EnableFingerCanvas(false);
            SubscribeToPlaceableEvents();
        }

        private void SubscribeToPlaceableEvents()
        {
            if (instancedPlaceable != null) {
                instancedPlaceable.OnValidPositionChange += HandleBtnConfirm;
                instancedPlaceable.OnStopUsingMe += StopUsingPlaceable;
            }
        }

        private void UnsuscribeToPlaceableEvents()
        {
            if (instancedPlaceable != null) {
                instancedPlaceable.OnValidPositionChange -= HandleBtnConfirm;
                instancedPlaceable.OnStopUsingMe -= StopUsingPlaceable;
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
            foreach (var projectData in projectsData) {
                var cardInstance = Instantiate(CardPrefab, UIGame.CardContainer); // spawn the card inside the container
                var cardComp = cardInstance.GetComponent<CardInGame>();
                if (cardComp != null) {
                    cardComp.InitCard(projectData); // initialize the card component
                    // get the associated model and bind it to the card clickable area
                    string modelKey = projectData.Model;
                    var associatedPrefab = GameplayConfig.I.GetProjectModelByKey(modelKey);
                    cardComp.SetCardEvents(() => ChosePrefab(associatedPrefab.transform, projectData));
                }
            }
        }
        #endregion
    }
}