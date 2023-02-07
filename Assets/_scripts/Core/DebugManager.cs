using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using vgwb.framework;

namespace vgwb.lanoria
{
    public enum SubregionDebugType
    {
        None,
        Color,
        Name,
        ColorAndName
    }

    public class DebugManager : SingletonMonoBehaviour<DebugManager>
    {
        protected override void Awake()
        {
            if (!Application.isEditor) {
                AppConfig.I.ShowSubregionDebug = SubregionDebugType.None;
            }
        }

        void Update()
        {
            // test move in grid
            if (Input.GetKeyDown(KeyCode.E)) {
                GridManager.I.DebugSelectCell(5);
            }
            if (Input.GetKeyDown(KeyCode.W)) {
                GridManager.I.DebugSelectCell(0);
            }
            if (Input.GetKeyDown(KeyCode.A)) {
                GridManager.I.DebugSelectCell(1);
            }
            if (Input.GetKeyDown(KeyCode.Z)) {
                GridManager.I.DebugSelectCell(2);
            }
            if (Input.GetKeyDown(KeyCode.X)) {
                GridManager.I.DebugSelectCell(3);
            }
            if (Input.GetKeyDown(KeyCode.D)) {
                GridManager.I.DebugSelectCell(4);
            }

            // GamePlay play Tile
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                GameManager.I.AutomaticPlayCard(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                GameManager.I.AutomaticPlayCard(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3)) {
                GameManager.I.AutomaticPlayCard(3);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4)) {
                GameManager.I.AutomaticPlayCard(4);
            }
        }
    }
}
