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
        }
        #endregion
    }
}
