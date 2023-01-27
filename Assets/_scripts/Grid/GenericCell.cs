using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb.lanoria
{
    public abstract class GenericCell : MonoBehaviour
    {
        [SerializeField] protected ProjectCategories category;
        [SerializeField] protected MeshRenderer mesh;

        private Hex hex => Hex.FromWorld(transform.position);
        private Hex localHex => Hex.FromWorld(transform.localPosition);

        public Vector3 HexPosition
        {
            get {
                return hex.ToWorld(0.0f);
            }
            set { }
        }

        public ProjectCategories Category
        {
            get { return category; }
        }

        public Color AssociatedColor
        {
            get {
                return GameData.I.Categories.GetColor(category);
            }
        }

        public Material AssociatedMaterial
        {
            get {
                return GameData.I.Categories.GetMaterial(category);
            }
        }

        protected virtual void Awake()
        {
            BaseSetup();
        }

        public void EnableHexComponent(bool enable)
        {
            // if (hexHandler != null) {
            //     hexHandler.enabled = enable;
            // }
        }

        public virtual void SetupCategory(ProjectCategories newCategory)
        {
            category = newCategory;
        }

        public virtual void ApplyColor()
        {
            var mat = AssociatedMaterial;
            if (mat != null) {
                mesh.material = AssociatedMaterial;
            }
        }

        protected virtual void BaseSetup()
        {
            if (mesh == null) {
                mesh = GetComponentInChildren<MeshRenderer>();
            }
        }
    }
}
