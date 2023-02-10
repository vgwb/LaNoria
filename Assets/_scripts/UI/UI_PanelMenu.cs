using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace vgwb.lanoria
{
    public class UI_PanelMenu : MonoBehaviour
    {
        public Button BtnPlay;
        public Button BtnAbout;
        public Button BtnHelp;
        public Button BtnOptions;

        void Start()
        {
            BtnPlay.onClick.AddListener(() => OnPlay());
            BtnPlay.onClick.AddListener(() => SoundManager.I.PlaySfx(AudioEnum.click));
            BtnAbout.onClick.AddListener(() => AppManager.I.OnAbout());
            BtnAbout.onClick.AddListener(() => SoundManager.I.PlaySfx(AudioEnum.click));
            BtnHelp.onClick.AddListener(() => SoundManager.I.PlaySfx(AudioEnum.click));
            BtnHelp.onClick.AddListener(() => SoundManager.I.PlaySfx(AudioEnum.click));
            BtnOptions.onClick.AddListener(() => AppManager.I.OnOptions());
            BtnOptions.onClick.AddListener(() => SoundManager.I.PlaySfx(AudioEnum.click));
        }

        public void OpenPanel()
        {
            gameObject.SetActive(true);
        }

        public void ClosePanel()
        {
            gameObject.SetActive(false);
        }

        private void OnPlay()
        {
            AppManager.I.OnPlay();
        }

    }
}
