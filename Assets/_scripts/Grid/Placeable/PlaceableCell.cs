using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb.lanoria
{
    public class PlaceableCell : GenericCell
    {
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

        public void SetupCategory(ProjectCategories newCategory)
        {
            category = newCategory;
        }
        #endregion
    }
}
