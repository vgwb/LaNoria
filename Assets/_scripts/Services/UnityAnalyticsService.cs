using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.Analytics;

namespace vgwb.lanoria
{
    public class UnityAnalyticsService : MonoBehaviour
    {
        private bool AnalyticsEnabled => AppConfig.I.AnalyticsEnabled;

        async void Awake()
        {
            if (!AnalyticsEnabled)
                return;

            var options = new InitializationOptions();
            if (AppConfig.I.DevEnvironment) {
                options.SetEnvironmentName("dev");
                Debug.LogWarning("Analytics in DEV environment");
            }
            await UnityServices.InitializeAsync(options);
        }

        public void TestEvent()
        {
            if (!AnalyticsEnabled)
                return;

            var parameters = new Dictionary<string, object>()
            {
                { "myNativeLang", AppManager.I.AppSettings.Locale },
            };

            AnalyticsService.Instance.CustomData("myTestEvent", parameters);
            AnalyticsService.Instance.Flush();
            Debug.Log("Analytics TestEvent");
        }

        public void Activity(string activityCode, string result)
        {
            if (!AnalyticsEnabled)
                return;

            var parameters = new Dictionary<string, object>()
            {
                { "myActivity", activityCode },
                { "myActivityResult", result },
            };

            AnalyticsService.Instance.CustomData("myActivity", parameters);
#if UNITY_EDITOR
            AnalyticsService.Instance.Flush();
#endif
            // Debug.Log("Analytics myActivity");
        }

        public void Points(int points, string action)
        {
            if (!AnalyticsEnabled)
                return;

            var parameters = new Dictionary<string, object>()
            {
                { "myPoints", points },
                { "myCardAction", action },
            };

            AnalyticsService.Instance.CustomData("myPoints", parameters);
#if UNITY_EDITOR
            AnalyticsService.Instance.Flush();
#endif
        }

        public void App(string action)
        {
            if (!AnalyticsEnabled)
                return;

            var parameters = new Dictionary<string, object>()
            {
                { "myAction", action },
            };

            AnalyticsService.Instance.CustomData("myApp", parameters);
#if UNITY_EDITOR
            AnalyticsService.Instance.Flush();
#endif
        }

    }
}
