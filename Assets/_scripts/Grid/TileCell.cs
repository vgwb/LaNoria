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
        public GameObject BridgePrefab;
        public SpriteRenderer Highlight;

        public ProjectCategories Category => category;
        public Color AssociatedColor => GameData.I.Categories.GetColor(category);
        public Material AssociatedMaterial => GameData.I.Categories.GetMaterial(category);

        protected override void Awake()
        {
            base.Awake();
        }

        public void Init()
        {
            BaseSetup();
            SetHighlightColor(GameplayConfig.I.MovingColor);
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
            if (!AppManager.I.AppSettings.AccessibilityEnabled) {
                Icon.gameObject.SetActive(false);
            }
        }

        public void ApplyColor()
        {
            var mat = AssociatedMaterial;
            ApplyMaterial(mat);
            if (AppManager.I.AppSettings.AccessibilityEnabled) {
                Icon.gameObject.SetActive(true);
                Icon.sprite = GameData.I.Categories.GetIcon(category);
            } else {
                Icon.gameObject.SetActive(false);
            }
        }

        public void SetupLayerForUICamera()
        {
            mesh.gameObject.layer = LayerMask.NameToLayer("UICamera");
            Icon.gameObject.layer = LayerMask.NameToLayer("UICamera");
        }


        public GameObject SpawnBridgeBetween(Vector3 from, Vector3 to)
        {
            var bridge = Instantiate(BridgePrefab, transform);
            bridge.transform.forward = to - from;

            return bridge;
        }

        public void EnableHighlight(bool enable)
        {
            if (Highlight != null) {
                Highlight.gameObject.SetActive(enable);
            }
        }

        public void SetHighlightColor(Color newColor)
        {
            if (Highlight != null) {
                Highlight.color = newColor;
            }
        }
    }
}
