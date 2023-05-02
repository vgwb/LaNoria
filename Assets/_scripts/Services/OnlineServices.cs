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

        public async void GetScores(Action<List<int>> callback)
        {
            if (!LeaderboardEnabled)
                return;

            var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId);
            if (scoresResponse.Results.Count > 0) {
                var scores = new List<int>();
                foreach (var score in scoresResponse.Results) {
                    scores.Add((int)score.Score);
                }
                callback?.Invoke(scores);
            }
            if (AppConfig.I.DebugMode)
                Debug.Log(JsonConvert.SerializeObject(scoresResponse));
        }
        #endregion
    }
}
