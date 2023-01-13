using vgwb.framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb.lanoria
{
    [CreateAssetMenu(fileName = "AppSettings", menuName = "Scriptable Object/General/AppConfig", order = 1)]
    public class AppConfig : SingletonScriptableObject<AppConfig>
    {
        [Header("App")]
        /// <summary>
        /// Incremental version
        /// </summary>
        public int Version = 1;
        public string UrlSupportWebsite;

        [Header("Services")]
        public bool AnalyticsEnabled;
        public bool AnalyticsDevEnv;

        [Header("Development")]
        public bool DebugMode;

        #region Var
        [Header("Placeable Settings")]
        public Color MovingColor;
        public Color OverlapColor;
        public Outline.Mode PlaceableOutlineMode = Outline.Mode.OutlineVisible;
        public float OutlineWidth = 7.0f;
        public float DragYOffset = 0.2f;
        #endregion
    }
}
