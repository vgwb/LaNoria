using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace vgwb.lanoria
{
    public class GridCell : GenericCell
    {
        #region Var
        public bool Occupied;
        public Region MyRegion = Region.Region_1;
        public Subregion MySubregion = Subregion.No_Subregion;
        #endregion

        #region MonoB
        protected override void Awake()
        {
            base.Awake();
            //            Debug.Log("category: " + category);
        }
        #endregion

        #region Functions
        public void Init(bool isOccupied)
        {
            Occupied = isOccupied;
            BaseSetup();
        }

        public bool IsCapital()
        {
            return MySubregion == Subregion.Capital;
        }
        #endregion
    }
}
