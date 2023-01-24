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
        public Button BtnOptions;
        public Button BtnHelp;

        void Start()
        {
            BtnPlay.onClick.AddListener(() => OnPlay());
            BtnAbout.onClick.AddListener(() => AppManager.I.OnAbout());
            BtnOptions.onClick.AddListener(() => AppManager.I.OnOptions());
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
