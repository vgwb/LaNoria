using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

using vgwb.lanoria;

namespace vgwb.framework
{
    public class language_toggle : MonoBehaviour
    {
        public Button btnLanguageToggle;
        public Sprite[] langIcons;

        private int localeCount;
        private int currentLocaleIndex;

        IEnumerator Start()
        {
            // Wait for the localization system to initialize
            yield return LocalizationSettings.InitializationOperation;

            currentLocaleIndex = 0;
            updateLanguageIcon();

            localeCount = 2; //LocalizationSettings.AvailableLocales.Locales.Count;

            btnLanguageToggle.onClick.AddListener(() => changeLocale());
        }

        private void changeLocale()
        {
            SoundManager.I.PlaySfx(AudioEnum.click);
            currentLocaleIndex++;
            if (currentLocaleIndex >= localeCount) {
                currentLocaleIndex = 0;
            }
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[currentLocaleIndex];
            updateLanguageIcon();
            //Debug.Log(LocalizationSettings.AvailableLocales.Locales[currentLocaleIndex].name);
        }

        private void updateLanguageIcon()
        {
            btnLanguageToggle.gameObject.GetComponent<Image>().sprite = langIcons[currentLocaleIndex];

        }
    }

}
