using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace vgwb.lanoria
{
    public class UI_PanelGamePause : MonoBehaviour
    {
        public Button BtnResume;
        public Button BtnExit;

        void Start()
        {
            BtnResume.onClick.AddListener(() => GameplayManager.I.ResumeGame());
            BtnExit.onClick.AddListener(() => GameplayManager.I.ExitGame());
        }

        public void OpenPanel()
        {
            gameObject.SetActive(true);
        }

        public void ClosePanel()
        {
            gameObject.SetActive(false);
        }
    }
}
