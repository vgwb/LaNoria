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
    private GameObject instancedPrefab;
    #endregion

    #region MonoB
    void Awake()
    {
        instancedPrefab = null;
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
            if (instancedPrefab != null) {
                Destroy(instancedPrefab);
            }

            Spawner.Prefab = placeablePrefab;
            EnableBtnConfirm(true);
            SetChoiceTxt(placeablePrefab.name);
        }
    }

    public void ConfirmProject()
    {
        if (instancedPrefab != null) {
            var projectComp = instancedPrefab.GetComponent<Placeable>();
            if (projectComp != null) {
                projectComp.OnProjectConfirmed();
            }
            instancedPrefab = null;
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
        instancedPrefab = clone;
    }

    private void EnableBtnConfirm(bool enable)
    {
        if (BtnConfirm != null) {
            BtnConfirm.SetActive(enable);
        }
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
