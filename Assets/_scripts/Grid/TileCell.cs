using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb.lanoria
{
    public class TileCell : GenericCell
    {
        protected override void Awake()
        {
            base.Awake();
        }

        public void Init()
        {
            BaseSetup();
        }

        public void SetupLayerForUICamera()
        {
            mesh.gameObject.layer = LayerMask.NameToLayer("UICamera");
        }
    }
}
