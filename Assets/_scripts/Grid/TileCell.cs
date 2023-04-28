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
            Vector3 dir = (to - from).normalized;
            bridge.transform.forward = dir; // set the direction
            float halfDistance = Vector3.Distance(from, to) / 2.0f;
            Vector3 pos = from + (dir *halfDistance);
            pos.y = GameplayConfig.I.BridgeEfxHeight;
            bridge.transform.position = pos; // set position between the two cells

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

        public void RotateIcon(float rot)
        {
            if (Icon != null) {
                var iconRot = Icon.transform.eulerAngles - Vector3.up * rot;
                Icon.transform.eulerAngles = iconRot;
            }
        }
    }
}
