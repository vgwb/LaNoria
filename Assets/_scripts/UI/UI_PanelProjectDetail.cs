using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace vgwb.lanoria
{
    public class UI_PanelProjectDetail : MonoBehaviour
    {
        public TextMeshProUGUI Title;
        public TextMeshProUGUI Description;
        public TextMeshProUGUI Year;
        public TextMeshProUGUI Entity;
        public Image ProjectImage;
        public Button BtnClose;
        public Button BtnNext;
        public Button BtnPrev;
        public TextMeshProUGUI Info;

        private void Init(ProjectData projectData)
        {
            BtnClose.onClick.AddListener(() => ClosePanel());
            BtnNext.onClick.AddListener(() => OnNext());
            BtnPrev.onClick.AddListener(() => OnPrevious());
            ShowProject(projectData);
        }

        public void ShowProject(ProjectData projectData)
        {
            Title.text = projectData.Title;
            Year.text = projectData.Year;
            Entity.text = projectData.Entity;
            Description.text = projectData.Description;
            Info.text = "";
            if (projectData.Image != null) {
                ProjectImage.gameObject.SetActive(true);
                ProjectImage.sprite = projectData.Image;
            } else {
                ProjectImage.gameObject.SetActive(false);
            }
        }

        public void OpenPanel(ProjectData data)
        {
            Init(data);
            gameObject.SetActive(true);
        }

        public void ClosePanel()
        {
            gameObject.SetActive(false);
        }

        private void OnNext()
        {
            SoundManager.I.PlaySfx(AudioEnum.click);

        }

        private void OnPrevious()
        {
            SoundManager.I.PlaySfx(AudioEnum.click);

        }

    }
}
