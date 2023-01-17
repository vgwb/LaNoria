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
        #endregion
    } 
}
