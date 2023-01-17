using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb.lanoria
{
    public class GridCell : GenericCell
    {
        #region Var
        public bool Occupied;
        #endregion

        #region MonoB
        void Awake()
        {
            if (hexHandler == null) {
                hexHandler = GetComponent<HexSnap>();
            }
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
