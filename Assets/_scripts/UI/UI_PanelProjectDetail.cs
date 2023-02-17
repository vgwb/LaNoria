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
        public Image ProjectImage;
        public Button BtnClose;

        private void Init(ProjectData projectData)
        {
            Title.text = projectData.Title;
            Description.text = projectData.Description;
            ProjectImage.sprite = projectData.Image;
            BtnClose.onClick.AddListener(() => ClosePanel());
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

    }
}
