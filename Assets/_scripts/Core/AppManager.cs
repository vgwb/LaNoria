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
        public AppSettings AppSettings;

        [Header("Debug")]
        public bool DebugDirectPlay;
        public bool DebugHome;

        public IEnumerator Start()
        {
            Application.runInBackground = true;

            AppSettings = new AppSettings();

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
            if (DebugHome)
                return;
            UI_manager.I.Show(UI_manager.States.Home);
            BoardManager.I.ShowMap(true);
            SoundManager.I.PlayMusic(AudioEnum.music_2);
            CameraManager.I.MoveInHome();
        }

        public void OnAbout()
        {
            UI_manager.I.Show(UI_manager.States.About);
        }

        public void OnPlay()
        {
            BoardManager.I.ShowMap(false);
            UI_manager.I.Show(UI_manager.States.Play);
            GameManager.I.StartGame();
            SoundManager.I.PlayMusic(AudioEnum.music_1);
        }

        public void OnOptions()
        {
            UI_manager.I.ShowOptions(true);
        }

    }
}
