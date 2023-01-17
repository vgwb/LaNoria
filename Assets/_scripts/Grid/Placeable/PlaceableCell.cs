using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb.lanoria
{
    public class PlaceableCell : GenericCell
    {
        #region MonoB
        protected override void Awake()
        {
            base.Awake();
        }
        #endregion

        #region Functions
        public void Init()
        {
            BaseSetup();
        }
        #endregion
    }
}
