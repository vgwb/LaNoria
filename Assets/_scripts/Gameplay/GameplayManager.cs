using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb.lanoria
{
    public class GameplayManager : MonoBehaviour
    {
        #region Var
        public UI_PanelGameHUD UIGame;
        public LeanSpawnWithFinger Spawner;
        public LeanFingerDownCanvas FingerCanvas;
        private Placeable instancedPlaceable;
        #endregion

        #region MonoB
        void Awake()
        {
            instancedPlaceable = null;
            UIGame.EnableBtnConfirm(false);
            UIGame.SetProjectTitle("");
            Spawner.OnSpawnedClone += OnPrefabSpawned;
        }

        private void OnDestroy()
        {
            Spawner.OnSpawnedClone -= OnPrefabSpawned;
        }
        #endregion

        #region Functions
        public void ChosePrefab(Transform placeablePrefab)
        {
            if (placeablePrefab != null && Spawner != null) {
                if (instancedPlaceable != null) {
                    Destroy(instancedPlaceable.gameObject);
                }

                Spawner.Prefab = placeablePrefab;
                UIGame.SetProjectTitle(placeablePrefab.name);
            }
        }

        public void ConfirmProject()
        {
            if (instancedPlaceable != null) {
                instancedPlaceable.OnProjectConfirmed();
                instancedPlaceable = null;
            }

            Spawner.Prefab = null;
            UIGame.SetProjectTitle("-");
            UIGame.EnableBtnConfirm(false);
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
        #endregion
    }
}
