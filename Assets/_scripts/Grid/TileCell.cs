using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace vgwb.lanoria
{
    public class TileCell : GenericCell
    {
        public TextMeshPro Label;

        protected override void Awake()
        {
            base.Awake();
        }

        public void Init()
        {
            BaseSetup();
            Label.text = "";
        }

        public void SetupLayerForUICamera()
        {
            mesh.gameObject.layer = LayerMask.NameToLayer("UICamera");
        }
    }
}
