using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.Leaderboards;
using UnityEngine;

namespace vgwb.lanoria
{
    public class LeaderboardService : MonoBehaviour
    {
        private bool LeaderboardEnabled => AppConfig.I.LeaderboardEnabled;

        const string LeaderboardId = "lanoria-hiscores";
        string VersionId { get; set; }
        int Offset { get; set; }
        int Limit { get; set; }
        int RangeLimit { get; set; }
        List<string> FriendIds { get; set; }

        async void Awake()
        {
            if (!LeaderboardEnabled)
                return;

            var options = new InitializationOptions();
            if (AppConfig.I.AnalyticsDevEnv) {
                options.SetEnvironmentName("dev");
                Debug.LogWarning("Analytics in DEV environment");
            }
            await UnityServices.InitializeAsync(options);

            await SignInAnonymously();
        }

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

        public async void AddScore()
        {
            var scoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync(LeaderboardId, 102);
            Debug.Log(JsonConvert.SerializeObject(scoreResponse));
        }

        public async void GetScores()
        {
            var scoresResponse =
                await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId);
            Debug.Log(JsonConvert.SerializeObject(scoresResponse));
        }

        public async void GetPaginatedScores()
        {
            Offset = 10;
            Limit = 10;
            var scoresResponse =
                await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId, new GetScoresOptions { Offset = Offset, Limit = Limit });
            Debug.Log(JsonConvert.SerializeObject(scoresResponse));
        }

        public async void GetPlayerScore()
        {
            var scoreResponse =
                await LeaderboardsService.Instance.GetPlayerScoreAsync(LeaderboardId);
            Debug.Log(JsonConvert.SerializeObject(scoreResponse));
        }

        public async void GetPlayerRange()
        {
            var scoresResponse =
                await LeaderboardsService.Instance.GetPlayerRangeAsync(LeaderboardId, new GetPlayerRangeOptions { RangeLimit = RangeLimit });
            Debug.Log(JsonConvert.SerializeObject(scoresResponse));
        }

        public async void GetScoresByPlayerIds()
        {
            var scoresResponse =
                await LeaderboardsService.Instance.GetScoresByPlayerIdsAsync(LeaderboardId, FriendIds);
            Debug.Log(JsonConvert.SerializeObject(scoresResponse));
        }

        // If the Leaderboard has been reset and the existing scores were archived,
        // this call will return the list of archived versions available to read from,
        // in reverse chronological order (so e.g. the first entry is the archived version
        // containing the most recent scores)
        public async void GetVersions()
        {
            var versionResponse =
                await LeaderboardsService.Instance.GetVersionsAsync(LeaderboardId);

            // As an example, get the ID of the most recently archived Leaderboard version
            VersionId = versionResponse.Results[0].Id;
            Debug.Log(JsonConvert.SerializeObject(versionResponse));
        }

        public async void GetVersionScores()
        {
            var scoresResponse =
                await LeaderboardsService.Instance.GetVersionScoresAsync(LeaderboardId, VersionId);
            Debug.Log(JsonConvert.SerializeObject(scoresResponse));
        }

        public async void GetPaginatedVersionScores()
        {
            Offset = 10;
            Limit = 10;
            var scoresResponse =
                await LeaderboardsService.Instance.GetVersionScoresAsync(LeaderboardId, VersionId, new GetVersionScoresOptions { Offset = Offset, Limit = Limit });
            Debug.Log(JsonConvert.SerializeObject(scoresResponse));
        }

        public async void GetPlayerVersionScore()
        {
            var scoreResponse =
                await LeaderboardsService.Instance.GetVersionPlayerScoreAsync(LeaderboardId, VersionId);
            Debug.Log(JsonConvert.SerializeObject(scoreResponse));
        }
    }
}
