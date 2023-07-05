using UnityEngine;
using UnityEditor;

namespace vgwb.framework
{
    public class EditorTools : ScriptableObject
    {
        [MenuItem("VGWB/Utility/Reveal Data Path")]
        static void RevealDataPath()
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }

        [MenuItem("VGWB/Utility/Delete ALL Prefs")]
        static void RevealPrefsPath()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
