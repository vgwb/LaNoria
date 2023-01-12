using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb
{
    public class PlaceableTile : MonoBehaviour
    {
        #region Var
        [SerializeField] private HexSnap hexHandler;
        [SerializeField] private Outline outlineHandler;
        #endregion

        #region Attributes
        public Vector3 HexPosition
        {
            get {
                if (hexHandler != null) {
                    return hexHandler.hex.ToWorld(0.0f);
                }

                return Vector3.negativeInfinity;
            }

            set { }
        }
        #endregion

        #region MonoB
        private void Start()
        {
            SetupOutline();
        }
        #endregion

        #region Functions
        public void Init()
        {
            hexHandler = GetComponent<HexSnap>();
            if (hexHandler == null) {
                hexHandler = gameObject.AddComponent<HexSnap>();
            }

            outlineHandler = GetComponentInChildren<Outline>();
            if (outlineHandler == null) {
                var mesh = GetComponentInChildren<MeshRenderer>();
                if (mesh != null) {
                    outlineHandler = mesh.gameObject.AddComponent<Outline>();
                }
            }

            SetupOutline();
        }

        public void ChangeOutlineColor(Color color)
        {
            outlineHandler.OutlineColor = color;
        }

        public void EnableOutline(bool enable)
        {
            outlineHandler.enabled = enable;
        }

        private void SetupOutline()
        {
            outlineHandler.OutlineColor = AppSettings.I.MovingColor;
            outlineHandler.OutlineWidth = AppSettings.I.OutlineWidth;
            outlineHandler.OutlineMode = AppSettings.I.PlaceableOutlineMode;
        }
        #endregion
    }
}
