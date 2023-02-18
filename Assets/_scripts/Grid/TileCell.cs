using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace vgwb.lanoria
{
    public class TileCell : GenericCell
    {
        public TextMeshPro Label;
        public SpriteRenderer Icon;

        protected override void Awake()
        {
            base.Awake();
        }

        public void Init()
        {
            BaseSetup();
            SetLabel("");
        }

        public void SetLabel(string label)
        {
            Label.text = label;
        }

        public void SetupLayerForUICamera()
        {
            mesh.gameObject.layer = LayerMask.NameToLayer("UICamera");
        }
    }
}
