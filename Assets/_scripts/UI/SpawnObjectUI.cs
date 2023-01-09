using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using vgwb;

public class SpawnObjectUI : MonoBehaviour
{
    #region Var
    public Transform SpawnPoint;
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
    }
    #endregion

    #region Functions
    public void SpawnPrefab(GameObject projectPrefab)
    {
        if (projectPrefab != null && SpawnPoint != null) {
            if (instancedPrefab != null) {
                Destroy(instancedPrefab);
            }
            instancedPrefab = Instantiate(projectPrefab);
            instancedPrefab.transform.position = SpawnPoint.position;
            EnableBtnConfirm(true);
            SetChoiceTxt(projectPrefab.name);
        }
    }

    public void ConfirmProject()
    {
        if (instancedPrefab != null) {
            var projectComp = instancedPrefab.GetComponent<Project>();
            if (projectComp != null) {
                projectComp.EnableLeanComponents(false);
                projectComp.OccupatyGrid();
            }
            instancedPrefab = null;
        }

        EnableBtnConfirm(false);
        EnableBtnsSpawn(true);
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
