using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace vgwb.lanoria
{
    public class UI_PanelOptions : MonoBehaviour
    {
        public Button BtnClose;
        public Button BtnCredits;

        public Toggle ToggleSfx;
        public Toggle ToggleMusic;
        public Toggle ToggleTutorial;
        public Toggle ToggleAccessibility;

        void Start()
        {
            BtnClose.onClick.AddListener(() => ClosePanel());
            BtnClose.onClick.AddListener(() => SoundManager.I.PlaySfx(AudioEnum.click));
            BtnCredits.onClick.AddListener(() => OnCredits());

            ToggleSfx.SetIsOnWithoutNotify(AppManager.I.AppSettings.SfxEnabled);
            ToggleMusic.SetIsOnWithoutNotify(AppManager.I.AppSettings.MusicEnabled);
            ToggleTutorial.SetIsOnWithoutNotify(AppManager.I.AppSettings.TutorialEnabled);
            ToggleAccessibility.SetIsOnWithoutNotify(AppManager.I.AppSettings.AccessibilityEnabled);

            ToggleSfx.onValueChanged.AddListener((value) => OnSfx(value));
            ToggleMusic.onValueChanged.AddListener((value) => OnMusic(value));
            ToggleTutorial.onValueChanged.AddListener((value) => OnTutorial(value));
            ToggleAccessibility.onValueChanged.AddListener((value) => OnAccessibility(value));
        }

        public void OpenPanel()
        {
            gameObject.SetActive(true);
        }

        public void ClosePanel()
        {
            gameObject.SetActive(false);
        }

        private void OnCredits()
        {
            UI_manager.I.ShowCredits(true);
            //Application.OpenURL("https://vgwb.org/projects/lanoria/");
        }

        private void OnSfx(bool status)
        {
            SoundManager.I.PlaySfx(AudioEnum.click);
            AppManager.I.AppSettings.SetSfx(status);
        }

        private void OnMusic(bool status)
        {
            SoundManager.I.PlaySfx(AudioEnum.click);
            AppManager.I.AppSettings.SetMusic(status);
            SoundManager.I.PlayMusic(status);
        }

        private void OnTutorial(bool status)
        {
            SoundManager.I.PlaySfx(AudioEnum.click);
            AppManager.I.AppSettings.SetTutorial(status);
        }

        private void OnAccessibility(bool status)
        {
            SoundManager.I.PlaySfx(AudioEnum.click);
            AppManager.I.AppSettings.SetAccessibility(status);
        }

    }
}
