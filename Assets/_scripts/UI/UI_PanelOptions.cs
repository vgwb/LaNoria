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
            BtnClose.onClick.AddListener(() => AppManager.I.OnHome());
            BtnClose.onClick.AddListener(() => SoundManager.I.PlaySfx(AudioEnum.click));
            BtnCredits.onClick.AddListener(() => OnCredits());

            ToggleSfx.SetIsOnWithoutNotify(true);
            ToggleMusic.SetIsOnWithoutNotify(true);
            ToggleTutorial.SetIsOnWithoutNotify(false);
            ToggleAccessibility.SetIsOnWithoutNotify(false);
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
            Application.OpenURL("https://vgwb.org/projects/lanoria/");
        }
    }
}
