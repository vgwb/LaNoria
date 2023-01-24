using vgwb.framework;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using UnityEditor;

namespace vgwb.lanoria
{
    public class AppManager : SingletonMonoBehaviour<AppManager>
    {
        public AppConfig ApplicationConfig;

        [Header("Debug")]
        public bool DebugDirectPlay;

        public IEnumerator Start()
        {
            Application.runInBackground = true;

            // Init localization
            yield return LocalizationSettings.InitializationOperation;

            if (DebugDirectPlay) {
                OnPlay();
            } else {
                OnHome();
            }
        }

        public void OnHome()
        {
            Debug.Log("HOME!");
            UI_manager.I.Show(UI_manager.States.Home);
        }

        public void OnAbout()
        {
            Debug.Log("ABOUT!");
            UI_manager.I.Show(UI_manager.States.About);
        }

        public void OnPlay()
        {
            UI_manager.I.Show(UI_manager.States.Play);
            Debug.Log("PLAY NEW GAME!");
        }

        public void OnOptions()
        {
            Debug.Log("OPTIONS!");
            UI_manager.I.Show(UI_manager.States.Options);
        }

    }
}
