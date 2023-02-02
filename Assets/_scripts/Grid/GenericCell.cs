using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb.lanoria
{
    public abstract class GenericCell : MonoBehaviour
    {
        [SerializeField] protected ProjectCategories category;
        [SerializeField] protected MeshRenderer mesh;

        public Hex hex => Hex.FromWorld(transform.position);
        private Hex localHex => Hex.FromWorld(transform.localPosition);

        public Vector3 HexPosition
        {
            get => hex.ToWorld(0.0f);
            set { }
        }

        public ProjectCategories Category => category;
        public Color AssociatedColor => GameData.I.Categories.GetColor(category);
        public Material AssociatedMaterial => GameData.I.Categories.GetMaterial(category);

        protected virtual void Awake()
        {
            BaseSetup();
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
