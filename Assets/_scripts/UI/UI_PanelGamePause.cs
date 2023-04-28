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
        public Button BtnOptions;
        [Header("Confirm Quit Refs")]
        public GameObject WindowConfirmQuit;
        public Button BtnConfirmQuitYes;
        public Button BtnConfirmQuitNo;

        void Start()
        {
            BtnResume.onClick.AddListener(() => GameManager.I.ResumeGame());
            BtnResume.onClick.AddListener(() => SoundManager.I.PlaySfx(AudioEnum.click));
            BtnExit.onClick.AddListener(() => EnableWindowConfirmQuit(true));
            BtnExit.onClick.AddListener(() => SoundManager.I.PlaySfx(AudioEnum.click));
            BtnOptions.onClick.AddListener(() => AppManager.I.OnOptions());
            BtnOptions.onClick.AddListener(() => SoundManager.I.PlaySfx(AudioEnum.click));

            BtnConfirmQuitYes.onClick.AddListener(() => GameManager.I.ForceEndGame());
            BtnConfirmQuitYes.onClick.AddListener(() => SoundManager.I.PlaySfx(AudioEnum.click));
            BtnConfirmQuitNo.onClick.AddListener(() => EnableWindowConfirmQuit(false));
            BtnConfirmQuitNo.onClick.AddListener(() => SoundManager.I.PlaySfx(AudioEnum.click));
            EnableWindowConfirmQuit(false);
        }

        public void OpenPanel()
        {
            gameObject.SetActive(true);
        }

        public void ClosePanel()
        {
            gameObject.SetActive(false);
        }

        public void EnableWindowConfirmQuit(bool enable)
        {
            if (WindowConfirmQuit != null) {
                WindowConfirmQuit.SetActive(enable);
            }
        }
    }
}
