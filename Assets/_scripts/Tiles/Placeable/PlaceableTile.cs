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

        }
        #endregion

        #region Functions
        public void Init()
        {
            hexHandler = GetComponent<HexSnap>();
            if (hexHandler == null) {
                hexHandler = gameObject.AddComponent<HexSnap>();
            }
        }
        #endregion
    }
}
