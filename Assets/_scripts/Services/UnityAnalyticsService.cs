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

        private void AddSharedParameters(CustomEvent myEvent)
        {
            myEvent.Add("myNativeLang", AppManager.I.AppSettings.Locale);
        }
        public void TestEvent()
        {
            if (!AnalyticsEnabled)
                return;

            var myEvent = new CustomEvent("myTestEvent") {
            };
            AddSharedParameters(myEvent);
            AnalyticsService.Instance.RecordEvent(myEvent);
            AnalyticsService.Instance.Flush();
            Debug.Log("Analytics TestEvent");
        }

        public void Activity(string activityCode, string result)
        {
            if (!AnalyticsEnabled)
                return;

            var myEvent = new CustomEvent("myActivity")
                        {
                { "myActivity", activityCode },
                { "myActivityResult", result },
            };

            AnalyticsService.Instance.RecordEvent(myEvent);
#if UNITY_EDITOR
            AnalyticsService.Instance.Flush();
#endif
            // Debug.Log("Analytics myActivity");
        }

        public void Points(int points, string action)
        {
            if (!AnalyticsEnabled)
                return;

            var myEvent = new CustomEvent("myPoints")
                                    {
                { "myPoints", points },
                { "myCardAction", action },
            };

            AnalyticsService.Instance.RecordEvent(myEvent);

#if UNITY_EDITOR
            AnalyticsService.Instance.Flush();
#endif
        }

        public void App(string action)
        {
            if (!AnalyticsEnabled)
                return;

            var myEvent = new CustomEvent("myApp") {
                { "myAction", action },
            };

            AnalyticsService.Instance.RecordEvent(myEvent);

#if UNITY_EDITOR
            AnalyticsService.Instance.Flush();
#endif
        }

    }
}
