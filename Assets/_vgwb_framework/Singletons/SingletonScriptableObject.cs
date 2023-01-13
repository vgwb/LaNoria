using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace vgwb.framework
{
    public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
    {
        private static T instance = null;
        public static T I
        {
            get {
                if (!instance)
                    instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
#if UNITY_EDITOR
                if (!instance) {
                    string[] configsGUIDs = UnityEditor.AssetDatabase.FindAssets("t:" + typeof(T).Name);
                    if (configsGUIDs.Length > 0) {
                        instance = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(UnityEditor.AssetDatabase.GUIDToAssetPath(configsGUIDs[0]));
                    }
                }
#endif
                return instance;
            }
        }

#if UNITY_EDITOR
        public void OnEnable()
        {
            AddToPreloaded();
        }

        private void AddToPreloaded()
        {
            if (I != null) {
                var preloadedAssets = PlayerSettings.GetPreloadedAssets().ToList();
                if (!preloadedAssets.Contains(I)) {
                    preloadedAssets.Add(I);
                    PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
                }
            }
        }
#endif
    }
}
