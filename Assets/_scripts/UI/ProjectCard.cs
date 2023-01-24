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
        public Material Mat_1;
        public Material Mat_2;
        public Material Mat_3;
        public Material Mat_4;

        public GameObject[] Cats;

        public void Init(ProjectData data)
        {
            Title.text = data.Title;

            int CatCounter = 0;

            foreach (ProjectCategories cat in data.Sequence) {
                Cats[CatCounter].SetActive(true);
                Cats[CatCounter].GetComponent<Image>().material = GetCatMaterial(cat);
                CatCounter++;
            }
            if (CatCounter < 3) {
                Cats[2].SetActive(false);
            }
            if (CatCounter < 4) {
                Cats[3].SetActive(false);
            }
        }

        private Material GetCatMaterial(ProjectCategories cat)
        {
            switch (cat) {
                case ProjectCategories.Environment:
                    return Mat_1;
                case ProjectCategories.Equality:
                    return Mat_2;
                case ProjectCategories.Tech:
                    return Mat_3;
                case ProjectCategories.People:
                    return Mat_4;
                default:
                    return null;
            }
        }

    }
}
