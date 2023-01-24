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

        void Start()
        {
            BtnPlay.onClick.AddListener(() => OnPlay());
        }

        public void Show(bool status)
        {
            gameObject.SetActive(status);
        }

        private void OnPlay()
        {
            Debug.Log("PLAY!");
        }

    }
}
