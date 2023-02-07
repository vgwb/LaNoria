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
            BtnResume.onClick.AddListener(() => GameManager.I.ResumeGame());
            BtnExit.onClick.AddListener(() => GameManager.I.ForceEndGame());
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
