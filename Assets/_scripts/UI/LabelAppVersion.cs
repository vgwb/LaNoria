using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace vgwb.lanoria
{
    public class LabelAppVersion : MonoBehaviour
    {
        public TextMeshProUGUI Label;

        void Start()
        {
            string version = "v " + (AppConfig.I.Version * 0.1).ToString("N1");

            if (AppConfig.I.DebugMode) {
                version = version + " DEBUG";
            }
            Label.text = version;
        }

    }
}
