using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Core
{
    [CreateAssetMenu(fileName = "AppSettings", menuName = "Scriptable Object/General/Settings", order = 1)]
    public class AppSettings : SingletonScriptableObject<AppSettings>
    {
        #region Var
        [Header("Placeable Settings")]
        public Color MovingColor;
        public Color OverlapColor;
        public Outline.Mode PlaceableOutlineMode = Outline.Mode.OutlineVisible;
        public float OutlineWidth = 7.0f;
        #endregion
    } 
}
