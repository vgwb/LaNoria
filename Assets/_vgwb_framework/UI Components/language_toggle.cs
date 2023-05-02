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

            switch (AppManager.I.AppSettings.Locale) {
                case "en":
                    currentLocaleIndex = 1;
                    break;
                case "it":
                    currentLocaleIndex = 2;
                    break;
                default:
                    currentLocaleIndex = 0;
                    break;
            }

            updateLanguageIcon();
            localeCount = LocalizationSettings.AvailableLocales.Locales.Count;
            btnLanguageToggle.onClick.AddListener(() => changeLocale());
        }

        private void changeLocale()
        {
            SoundManager.I.PlaySfx(AudioEnum.click);
            currentLocaleIndex++;
            if (currentLocaleIndex >= localeCount) {
                currentLocaleIndex = 0;
            }
            updateLanguageIcon();
            //Debug.Log(LocalizationSettings.AvailableLocales.Locales[currentLocaleIndex].Identifier.Code);
            AppManager.I.AppSettings.SetLocale(LocalizationSettings.AvailableLocales.Locales[currentLocaleIndex].Identifier.Code);
        }

        private void updateLanguageIcon()
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[currentLocaleIndex];
            btnLanguageToggle.gameObject.GetComponent<Image>().sprite = langIcons[currentLocaleIndex];
        }
    }

}
