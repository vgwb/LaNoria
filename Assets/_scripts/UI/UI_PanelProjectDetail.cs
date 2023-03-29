using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
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
            setDescription(projectData.Id);

            Info.text = "";
            if (projectData.Image != null) {
                ProjectImage.gameObject.SetActive(true);
                ProjectImage.sprite = projectData.Image;
            } else {
                ProjectImage.gameObject.SetActive(false);
            }
        }

        private void setDescription(string id)
        {
            LocalizeStringEvent LocalizeStringEvent = Description.gameObject.GetComponent<LocalizeStringEvent>();
            LocalizeStringEvent.StringReference = new LocalizedString("Projects", "proj." + id);
            // Description.text = projectData.Description;
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
