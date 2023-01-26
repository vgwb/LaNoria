using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb.lanoria
{
    [RequireComponent(typeof(HexSnap))]
    public abstract class GenericCell : MonoBehaviour
    {
        [SerializeField] protected ProjectCategories category;
        [SerializeField] protected HexSnap hexHandler;
        [SerializeField] protected MeshRenderer mesh;

        public Vector3 HexPosition
        {
            get {
                if (hexHandler != null) {
                    return hexHandler.hex.ToWorld(0.0f);
                }
                return Vector3.negativeInfinity;
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
                if (GameplayConfig.I != null) {
                    return DataManager.I.Data.CategoriesData.GetColor(category);
                }

                return Color.white;
            }
        }

        public Material AssociatedMaterial
        {
            get {
                if (GameplayConfig.I != null) {
                    return DataManager.I.Data.CategoriesData.GetMaterial(category);
                }
                return null;
            }
        }

        protected virtual void Awake()
        {
            BaseSetup();
        }

        public void EnableHexComponent(bool enable)
        {
            if (hexHandler != null) {
                hexHandler.enabled = enable;
            }
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

            if (hexHandler == null) {
                hexHandler = GetComponent<HexSnap>();
            }
        }
    }
}
