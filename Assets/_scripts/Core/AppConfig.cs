using vgwb.framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb.lanoria
{
    [CreateAssetMenu(fileName = "AppSettings", menuName = "VGWB/App Config", order = 1)]
    public class AppConfig : SingletonScriptableObject<AppConfig>
    {
        [Header("App")]
        /// <summary>
        /// Incremental version
        /// </summary>
        public int Version = 1;
        public string UrlSupportWebsite;

        public GameData GameData;

        [Header("Services")]
        public bool AnalyticsEnabled;
        public bool AnalyticsDevEnv;

        [Header("Development")]
        public bool DebugMode;
        public bool ShowHexCoords;
        public bool ShowTutorial = true;
        public SubregionDebugType ShowSubregionDebug;
    }
}
