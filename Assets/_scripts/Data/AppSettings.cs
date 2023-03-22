using UnityEngine;

namespace vgwb.lanoria
{
    public class AppSettings
    {
        public bool MusicEnabled = true;
        public bool SfxEnabled = true;
        public bool AccessibilityEnabled = false;
        public bool TutorialEnabled = true;
        public string Locale;
        public int HiScore = 0;

        public AppSettings()
        {
            Load();
        }

        public void SetMusic(bool status)
        {
            MusicEnabled = status;
            PlayerPrefs.SetInt("MusicEnabled", MusicEnabled ? 1 : 0);
        }

        public void SetSfx(bool status)
        {
            // Debug.Log("SetSfx " + status);
            SfxEnabled = status;
            PlayerPrefs.SetInt("SfxEnabled", SfxEnabled ? 1 : 0);
        }

        public void SetAccessibility(bool status)
        {
            AccessibilityEnabled = status;
            PlayerPrefs.SetInt("AccessibilityEnabled", AccessibilityEnabled ? 1 : 0);
        }

        public void SetTutorial(bool status)
        {
            TutorialEnabled = status;
            PlayerPrefs.SetInt("TutorialEnabled", TutorialEnabled ? 1 : 0);
        }

        public void SetLocale(string locale)
        {
            Locale = locale;
            PlayerPrefs.SetString("Locale", Locale);
        }

        public void SetHiScore(int hiscore)
        {
            HiScore = hiscore;
            PlayerPrefs.SetInt("HiScore", hiscore);
        }

        private void Load()
        {
            // Debug.Log("Load Appsertings");
            MusicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
            SfxEnabled = PlayerPrefs.GetInt("SfxEnabled", 1) == 1;
            AccessibilityEnabled = PlayerPrefs.GetInt("AccessibilityEnabled", 0) == 1;
            TutorialEnabled = PlayerPrefs.GetInt("TutorialEnabled", 1) == 1;
            Locale = PlayerPrefs.GetString("Locale");
            HiScore = PlayerPrefs.GetInt("HiScore", 0);
        }

        private bool Exists()
        {
            return PlayerPrefs.HasKey("MusicEnabled");
        }
    }

}
