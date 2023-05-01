using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace vgwb.lanoria
{
    public class UI_Hiscores : MonoBehaviour
    {

        public TextMeshProUGUI LocalHiscore;
        public TextMeshProUGUI Leaderboard;

        void Start()
        {
            Refresh();
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
            Debug.Log("Load Hi Scores");
            OnlineServices.I.GetScores(DisplayLeaderboard);
        }

        private void DisplayLeaderboard(List<int> scores)
        {
            Leaderboard.text = "";
            int count = 1;
            foreach (var score in scores) {
                Leaderboard.text += count + ": " + score + "\n";
                count++;
            }
        }
    }
}
