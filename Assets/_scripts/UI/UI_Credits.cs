using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace vgwb.lanoria
{
    public class UI_Credits : MonoBehaviour
    {
        public Button BtnClose;

        void Start()
        {
            BtnClose.onClick.AddListener(() => ClosePanel());
            BtnClose.onClick.AddListener(() => SoundManager.I.PlaySfx(AudioEnum.click));
        }

        public void OpenPanel()
        {
            gameObject.SetActive(true);
        }

        public void ClosePanel()
        {
            gameObject.SetActive(false);
        }

    }
}
