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
            BtnResume.onClick.AddListener(() => SoundManager.I.PlaySfx(AudioEnum.click));
            BtnExit.onClick.AddListener(() => GameManager.I.ForceEndGame());
            BtnExit.onClick.AddListener(() => SoundManager.I.PlaySfx(AudioEnum.click));
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
