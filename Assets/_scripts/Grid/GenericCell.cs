using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb.lanoria
{
    [RequireComponent(typeof(HexSnap))]
    public abstract class GenericCell : MonoBehaviour
    {
        #region Var
        [SerializeField] protected ProjectCategories category;
        [SerializeField] protected HexSnap hexHandler;
        [SerializeField] protected MeshRenderer mesh;
        #endregion

        #region Attributes
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
                    return GameplayConfig.I.GetCategoryColorsByType(category);
                }

                return Color.white;
            }
        }

        public Material AssociatedMaterial
        {
            get {
                if (GameplayConfig.I != null) {
                    return GameplayConfig.I.GetCategoryMaterialByType(category);
                }

                return null;
            }
        }
        #endregion

        #region MonoB
        protected virtual void Awake()
        {
            BaseSetup();
        }
        #endregion

        #region Functions
        public virtual void SetupCategory(ProjectCategories newCategory)
        {
            category = newCategory;
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

        protected virtual void ApplyColor()
        {
            var mat = AssociatedMaterial;
            if (mat != null) {
                mesh.material = AssociatedMaterial;
            }
        }
        #endregion
    }
}
