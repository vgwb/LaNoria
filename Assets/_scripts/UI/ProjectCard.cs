using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace vgwb.lanoria
{
    public class ProjectCard : MonoBehaviour
    {
        public TextMeshProUGUI Title;
        public GameObject Cat_1;
        public GameObject Cat_2;
        public GameObject Cat_3;
        public GameObject Cat_4;

        public void Init(ProjectData data)
        {
            Title.text = data.Title;

            foreach (ProjectCategories cat in data.Sequence) {
                switch (cat) {
                    case ProjectCategories.Environment:
                        break;
                    case ProjectCategories.Equality:
                        break;
                    case ProjectCategories.Tech:
                        break;
                    case ProjectCategories.People:
                        break;
                }

            }

        }

    }
}
