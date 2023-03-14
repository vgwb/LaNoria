using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace vgwb.lanoria
{
    public class ProjectCard : MonoBehaviour
    {
        public TextMeshProUGUI Title;
        public Image ProjectImage;
        public GameObject[] Cats;

        private ProjectData projectData;

        public void Init(ProjectData data)
        {
            projectData = data;
            Title.text = projectData.Title;
            if (projectData.Image != null) {
                ProjectImage.gameObject.SetActive(true);
                ProjectImage.sprite = projectData.Image;
            } else {
                ProjectImage.gameObject.SetActive(false);
            }
            int CatCounter = 0;

            foreach (ProjectCategories cat in projectData.Sequence) {
                Cats[CatCounter].SetActive(true);
                Cats[CatCounter].GetComponent<Image>().color = GameData.I.Categories.GetColor(cat);
                CatCounter++;
            }
            if (CatCounter < 3) {
                Cats[2].SetActive(false);
            }
            if (CatCounter < 4) {
                Cats[3].SetActive(false);
            }

            GetComponent<Button>().onClick.AddListener(() => OnClick());
        }

        private void OnClick()
        {
            //            Debug.Log("CLICKED " + projectData.Title);
            UI_manager.I.OpenProjectDetail(projectData);
        }

    }
}
