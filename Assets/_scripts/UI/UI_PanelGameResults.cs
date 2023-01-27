using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace vgwb.lanoria
{
    public class UI_PanelGameResults : MonoBehaviour
    {
        public Button BtnResume;
        public Button BtnExit;

        void Start()
        {
            BtnExit.onClick.AddListener(() => GameManager.I.ExitGame());
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
