using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using vgwb;

public class UIManager : MonoBehaviour
{
    #region Var
    public LeanSpawnWithFinger Spawner;
    public TMP_Text ChoiceTxt;
    public GameObject BtnConfirm;
    public List<Button> BtnsSpawn;
    private Placeable instancedPlaceable;
    #endregion

    #region MonoB
    void Awake()
    {
        instancedPlaceable = null;
        EnableBtnConfirm(false);
        EnableBtnsSpawn(true);
        SetChoiceTxt("");
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
            SetChoiceTxt(placeablePrefab.name);
        }
    }

    public void ConfirmProject()
    {
        if (instancedPlaceable != null) {
            instancedPlaceable.OnProjectConfirmed();
            instancedPlaceable = null;
        }

        EnableBtnConfirm(false);
        EnableBtnsSpawn(true);
    }

    public void ResetDetailPanel()
    {
        Spawner.Prefab = null;
    }

    private void OnPrefabSpawned(GameObject clone)
    {
        instancedPlaceable = clone.GetComponent<Placeable>();
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
        if (BtnConfirm != null) {
            BtnConfirm.SetActive(enable);
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
    }

    private void EnableBtnsSpawn(bool enable)
    {
        if (BtnsSpawn.Count > 0) {
            foreach (var btn in BtnsSpawn) {
                btn.interactable = enable;
            }
        }
    }

    private void SetChoiceTxt(string message)
    {
        if (ChoiceTxt != null) {
            ChoiceTxt.text = message;
        }
    }
    #endregion
}
