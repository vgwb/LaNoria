using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.Analytics;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;

namespace vgwb.lanoria
{
    public class OnlineServices : MonoBehaviour
    {
        public static OnlineServices I;

        private bool AnalyticsEnabled => AppConfig.I.AnalyticsEnabled;
        private bool LeaderboardEnabled => AppConfig.I.LeaderboardEnabled;
        private const string LeaderboardId = "noria-leaderboard";

        async void Awake()
        {
            I = this;
            if (!AnalyticsEnabled && !LeaderboardEnabled)
                return;

            var options = new InitializationOptions();
            if (AppConfig.I.DevEnvironment) {
                options.SetEnvironmentName("dev");
                Debug.LogWarning("DEV environment");
            }
            await UnityServices.InitializeAsync(options);

            if (LeaderboardEnabled) {
                await SignInAnonymously();
            }
        }

        #region Analytics
        public void Points(int points, string action)
        {
            if (!AnalyticsEnabled)
                return;

            var myEvent = new CustomEvent("myPoints") {
                { "myPoints", points },
                { "myCardAction", action },
            };

            AnalyticsService.Instance.RecordEvent(myEvent);
#if UNITY_EDITOR
            AnalyticsService.Instance.Flush();
#endif
        }
        #endregion


        #region LeaderBoard
        async Task SignInAnonymously()
        {
            AuthenticationService.Instance.SignedIn += () => {
                Debug.Log("Signed in as: " + AuthenticationService.Instance.PlayerId);
            };
            AuthenticationService.Instance.SignInFailed += s => {
                // Take some action here...
                Debug.Log(s);
            };

            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        public async void AddScore(int score)
        {
            if (!LeaderboardEnabled)
                return;

            // Debug.Log("sending hi-score to Online Leaderboard");
            var scoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync(LeaderboardId, score);
            if (AppConfig.I.DebugMode)
                Debug.Log(JsonConvert.SerializeObject(scoreResponse));
        }

        public async void GetTopScores(Action<List<LeaderboardEntry>> callback)
        {
            if (!LeaderboardEnabled)
                return;

            var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId, new GetScoresOptions { Limit = 3 });
            callback?.Invoke(scoresResponse.Results);

            if (AppConfig.I.DebugMode)
                Debug.Log(JsonConvert.SerializeObject(scoresResponse));
        }

        public async void GetPlayerScore(Action<List<LeaderboardEntry>> callback)
        {
            if (!LeaderboardEnabled)
                return;

            var aroundscoresResponse = await LeaderboardsService.Instance.GetPlayerRangeAsync(LeaderboardId, new GetPlayerRangeOptions { RangeLimit = 1 });
            callback?.Invoke(aroundscoresResponse.Results);

            if (AppConfig.I.DebugMode)
                Debug.Log(JsonConvert.SerializeObject(aroundscoresResponse));
        }
        #endregion
    }
}
