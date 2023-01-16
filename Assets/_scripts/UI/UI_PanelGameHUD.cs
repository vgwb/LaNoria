using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace vgwb.lanoria
{
    public class UI_PanelGameHUD : MonoBehaviour
    {
        #region Var
        [Header("Panel HUD Elements")]
        public GameObject PanelHUD;
        [Header("Panel Cards Elements")]
        public GameObject PanelCards;
        [Header("Panel Current Project Elements")]
        public GameObject PanelCurrentProject;
        public TMP_Text ProjectTitle;
        [Header("Others")]
        public LeanSpawnWithFinger Spawner;
        public LeanFingerDownCanvas FingerCanvas;
        public GameObject PanelConfirm;
        private Placeable instancedPlaceable;
        #endregion

        #region MonoB
        void Awake()
        {
            instancedPlaceable = null;
            EnableBtnConfirm(false);
            SetProjectTitle("");
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
                SetProjectTitle(placeablePrefab.name);
            }
        }

        public void ConfirmProject()
        {
            if (instancedPlaceable != null) {
                instancedPlaceable.OnProjectConfirmed();
                instancedPlaceable = null;
            }

            Spawner.Prefab = null;
            SetProjectTitle("-");
            EnableBtnConfirm(false);
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

        private void EnableBtnConfirm(bool enable)
        {
            if (PanelConfirm != null) {
                PanelConfirm.SetActive(enable);
            }
        }

        private void HandleBtnConfirm()
        {
            if (instancedPlaceable != null) {
                EnableBtnConfirm(instancedPlaceable.IsValidPosition);
            }
        }

        private void StopUsingPlaceable()
        {
            UnsuscribeToPlaceableEvents();
            EnableFingerCanvas(true);
        }

        private void SetProjectTitle(string message)
        {
            if (ProjectTitle != null) {
                ProjectTitle.text = message;
            }
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
