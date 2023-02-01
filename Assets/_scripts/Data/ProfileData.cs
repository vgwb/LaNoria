using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Serialization;

namespace vgwb.lanoria
{
    [Serializable]
    public class AppSettings
    {
        public bool TutorialCompleted;
        public string NativeLocale;
        public bool SfxDisabled;
        public bool NotificationsDisabled;
        public bool AnalyticsDisabled;
    }

    [Serializable]
    public class ProfileData
    {
        public int Version;
        public AppSettings AppSettings;
        public int HiScore;
    }
}
