using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using vgwb.framework;

namespace vgwb.lanoria
{
    public class UI_Credits : MonoBehaviour
    {
        public TextAsset CreditsText;

        public Button BtnClose;
        public TextMeshProUGUI TfCredits;

        public Color Level0Color = Color.blue;
        public int Level0FontPerc = 140;
        public Color Level1Color = Color.yellow;
        public int Level1FontPerc = 110;

        void Start()
        {
            BtnClose.onClick.AddListener(() => ClosePanel());
            BtnClose.onClick.AddListener(() => SoundManager.I.PlaySfx(AudioEnum.click));
            TfCredits.text = FormatCredits(CreditsText.text);
        }
        public void OpenPanel()
        {
            gameObject.SetActive(true);
        }

        public void ClosePanel()
        {
            gameObject.SetActive(false);
        }

        string FormatCredits(string _txt)
        {
            // Format
            string lv0 = "<size=" + Level0FontPerc + "%><color=#" + ColorUtilities.ColorToHex(Level0Color) + ">";
            string lv1 = "<size=" + Level1FontPerc + "%><color=#" + ColorUtilities.ColorToHex(Level1Color) + ">";
            _txt = _txt.Replace("[0]", lv0);
            _txt = _txt.Replace("[0E]", "</color></size>");
            _txt = _txt.Replace("[1]", lv1);
            _txt = _txt.Replace("[1E]", "</color></size>");
            // Fix missing characters
            _txt = _txt.Replace("ö", "o");

            return _txt;
        }
    }
}
