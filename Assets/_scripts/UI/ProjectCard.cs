using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace vgwb.lanoria
{
    public class ProjectCard : MonoBehaviour
    {
        public TextMeshProUGUI Title;
        public TextMeshProUGUI Colors;

        void Init(string title)
        {
            Title.text = title;
        }

    }
}
