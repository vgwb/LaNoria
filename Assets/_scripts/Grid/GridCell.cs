using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace vgwb.lanoria
{
    public class GridCell : GenericCell
    {
        public bool Occupied;

        public Region Region = Region.Region_1;
        public AreaId Area = AreaId.undefined;

        protected override void Awake()
        {
            base.Awake();
            //            Debug.Log("category: " + category);
        }

        public void Init(bool isOccupied)
        {
            Occupied = isOccupied;
            BaseSetup();
        }

        public bool IsCapital()
        {
            return Area == AreaId.Subregion_7;
        }
    }
}
