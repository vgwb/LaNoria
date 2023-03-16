using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace vgwb.lanoria
{
    public class LabelHiscore : MonoBehaviour
    {
        public TextMeshProUGUI Label;

        void Start()
        {
            Refresh();
        }

        public void Refresh()
        {
            Label.text = AppManager.I.AppSettings.HiScore.ToString();
        }

    }
}
