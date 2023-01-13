using vgwb.framework;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace vgwb.lanoria
{
    public class AppManager : SingletonMonoBehaviour<AppManager>
    {
        public AppConfig ApplicationConfig;
        public Image LoadingObscurer;

        // protected override void Init()
        // {
        //     if (LoadingObscurer != null)
        //         LoadingObscurer.color = new Color(LoadingObscurer.color.r, LoadingObscurer.color.g, LoadingObscurer.color.b, 1f);
        // }

        public IEnumerator Start()
        {
            Application.runInBackground = true;

            // Init localization
            yield return LocalizationSettings.InitializationOperation;

        }

    }
}
