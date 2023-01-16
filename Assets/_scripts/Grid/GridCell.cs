using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb.lanoria
{
    public class GridCell : MonoBehaviour
    {
        #region Var
        public bool Occupied;
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
        void Awake()
        {
            if (hexHandler == null) {
                hexHandler = GetComponent<HexSnap>();
            }
        }

        void Update()
        {

        }
        #endregion

        #region Functions
        public void Init(bool isOccupied)
        {
            Occupied = isOccupied;
            hexHandler = GetComponent<HexSnap>();
        }
        #endregion
    }
}
