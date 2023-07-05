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
        public List<UI_Leaderboard_Entry> LeaderboardEntries;

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
            //Leaderboard.text = "";
            StartCoroutine(LoadHiScores());
        }

        private IEnumerator LoadHiScores()
        {
            yield return new WaitForSeconds(1);
            //            Debug.Log("Load Hi Scores");
            OnlineServices.I.GetTopScores(DisplayTopLeaderboard);
            if (AppManager.I.AppSettings.HiScore > 0) {
                OnlineServices.I.GetPlayerScore(DisplayPlayerLeaderboard);
            } else {
                DisplayPlayerLeaderboard(new List<LeaderboardEntry>());
            }
        }

        // callback
        private void DisplayTopLeaderboard(List<LeaderboardEntry> scores)
        {
            LeaderboardContainer.SetActive(true);
            //Leaderboard.text = "";

            if (scores.Count > 0) {
                LeaderboardEntries[0].Set((int)scores[0].Score, 1, scores[0].Score == AppManager.I.AppSettings.HiScore);
                LeaderboardEntries[0].Enable(true);
            } else {
                LeaderboardEntries[0].Enable(false);
            }

            if (scores.Count > 1) {
                LeaderboardEntries[1].Set((int)scores[1].Score, 2, scores[1].Score == AppManager.I.AppSettings.HiScore);
                LeaderboardEntries[1].Enable(true);
            } else {
                LeaderboardEntries[1].Enable(false);
            }

            if (scores.Count > 2) {
                LeaderboardEntries[2].Set((int)scores[2].Score, 3, scores[2].Score == AppManager.I.AppSettings.HiScore);
                LeaderboardEntries[2].Enable(true);
            } else {
                LeaderboardEntries[2].Enable(false);
            }
        }

        // callback
        private void DisplayPlayerLeaderboard(List<LeaderboardEntry> scores)
        {
            if (scores.Count > 0) {
                LeaderboardEntries[3].Set((int)scores[0].Score, scores[0].Rank + 1, scores[0].Score == AppManager.I.AppSettings.HiScore);
                LeaderboardEntries[3].Enable(true);
            } else {
                LeaderboardEntries[3].Enable(false);
            }

            if (scores.Count > 1) {
                LeaderboardEntries[4].Set((int)scores[1].Score, scores[1].Rank + 1, scores[1].Score == AppManager.I.AppSettings.HiScore);
                LeaderboardEntries[4].Enable(true);
            } else {
                LeaderboardEntries[4].Enable(false);
            }

            if (scores.Count > 2) {
                LeaderboardEntries[5].Set((int)scores[2].Score, scores[2].Rank + 1, scores[2].Score == AppManager.I.AppSettings.HiScore);
                LeaderboardEntries[5].Enable(true);
            } else {
                LeaderboardEntries[5].Enable(false);
            }

        }

        private void DisplayScoreEntry(LeaderboardEntry entry)
        {
            string entryText = "#" + (entry.Rank + 1) + " - " + entry.Score;
            if (entry.Score == AppManager.I.AppSettings.HiScore) {
                entryText = "<color=#FFDD00>" + entryText + "</color>";
            }
            //Leaderboard.text += entryText + "\n";
        }
    }
}
