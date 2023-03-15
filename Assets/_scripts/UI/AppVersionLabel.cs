using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace vgwb.lanoria
{
    public class AppVersionLabel : MonoBehaviour
    {
        public TextMeshProUGUI Label;

        void Start()
        {
            string version = (AppConfig.I.Version * 0.1).ToString();

            if (AppConfig.I.DebugMode) {
                version = version + " DEBUG";
            }
            Label.text = version;
        }

    }
}
