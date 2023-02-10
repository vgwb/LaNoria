using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace vgwb.lanoria
{
    public class UI_PanelOptions : MonoBehaviour
    {
        public Button BtnClose;

        void Start()
        {
            BtnClose.onClick.AddListener(() => AppManager.I.OnHome());
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
