using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;

using TMPro;

namespace vgwb.lanoria
{
    public class UI_Hiscores : MonoBehaviour
    {

        public TextMeshProUGUI LocalHiscore;
        public GameObject LeaderboardContainer;
        public TextMeshProUGUI Leaderboard;

        void Start()
        {
            LocalHiscore.text = AppManager.I.AppSettings.HiScore.ToString();
            LeaderboardContainer.SetActive(false);
        }

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.L)) {
                Refresh();
            }
        }

        public void Refresh()
        {
            LocalHiscore.text = AppManager.I.AppSettings.HiScore.ToString();
            Leaderboard.text = "";
            StartCoroutine(LoadHiScores());
        }

        private IEnumerator LoadHiScores()
        {
            yield return new WaitForSeconds(1);
            //            Debug.Log("Load Hi Scores");
            OnlineServices.I.GetTopScores(DisplayTopLeaderboard);
            OnlineServices.I.GetPlayerScore(DisplayPlayerLeaderboard);
        }

        // callback
        private void DisplayTopLeaderboard(List<LeaderboardEntry> scores)
        {
            LeaderboardContainer.SetActive(true);
            Leaderboard.text = "";

            foreach (var entry in scores) {
                DisplayScoreEntry(entry);
            }
        }

        // callback
        private void DisplayPlayerLeaderboard(List<LeaderboardEntry> scores)
        {
            Leaderboard.text += "...\n";
            foreach (var entry in scores) {
                DisplayScoreEntry(entry);
            }
        }

        private void DisplayScoreEntry(LeaderboardEntry entry)
        {
            string entryText = "#" + (entry.Rank + 1) + " - " + entry.Score;
            if (entry.Score == AppManager.I.AppSettings.HiScore) {
                entryText = "<color=#FFDD00>" + entryText + "</color>";
            }
            Leaderboard.text += entryText + "\n";
        }
    }
}
