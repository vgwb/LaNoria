using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace vgwb.lanoria
{
    public class TileCell : GenericCell
    {
        [SerializeField] ProjectCategories category;
        public TextMeshPro Label;
        public SpriteRenderer Icon;
        public GameObject Bridge;

        public ProjectCategories Category => category;
        public Color AssociatedColor => GameData.I.Categories.GetColor(category);
        public Material AssociatedMaterial => GameData.I.Categories.GetMaterial(category);

        protected override void Awake()
        {
            base.Awake();
            EnableBridge(false);
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

        public void SetupCategory(ProjectCategories newCategory)
        {
            category = newCategory;
        }

        public void RemoveCategory()
        {
            Icon.gameObject.SetActive(false);
        }

        public void ApplyColor()
        {
            var mat = AssociatedMaterial;
            ApplyMaterial(mat);
            Icon.sprite = GameData.I.Categories.GetIcon(category);
        }

        public void SetupLayerForUICamera()
        {
            mesh.gameObject.layer = LayerMask.NameToLayer("UICamera");
        }

        public void EnableBridge(bool enable)
        {
            if (Bridge != null) {
                Bridge.SetActive(enable);
            }
        }
    }
}
