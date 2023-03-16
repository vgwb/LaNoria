using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace vgwb.lanoria
{
    public class UI_PanelGameResults : MonoBehaviour
    {
        public Button BtnResume;
        public Button BtnExit;
        public TMP_Text Score;
        public TMP_Text PlacementScore;
        public TMP_Text AdjacencyScore;
        public TMP_Text AreaScore;
        public TMP_Text EmptyScore;
        public GameObject NewScore;

        void Start()
        {
            BtnExit.onClick.AddListener(() => GameManager.I.ExitGame());
        }

        public void ShowResult(int blablabla_score)
        {

        }

        public void OpenPanel()
        {
            gameObject.SetActive(true);
        }

        public void ClosePanel()
        {
            gameObject.SetActive(false);
        }

        public void SetTotalScore(string score)
        {
            if (Score != null) {
                Score.text = score;
            }
        }

        public void ShowNewHiScore(bool show)
        {
            if (NewScore != null) {
                NewScore.SetActive(show);
            }
        }

        public void SetTotalScore(int score)
        {
            SetTotalScore(score.ToString());
        }

        public void SetPlacementScore(string score)
        {
            if (Score != null) {
                PlacementScore.text = score;
            }
        }

        public void SetPlacementScore(int score)
        {
            SetPlacementScore(score.ToString());
        }

        public void SetAdjacencyScore(string score)
        {
            if (Score != null) {
                AdjacencyScore.text = score;
            }
        }

        public void SetAdjacencyScore(int score)
        {
            SetAdjacencyScore(score.ToString());
        }

        public void SetAreaScore(string score)
        {
            if (Score != null) {
                AreaScore.text = score;
            }
        }

        public void SetAreaScore(int score)
        {
            SetAreaScore(score.ToString());
        }

        public void SetEmptyScore(string score)
        {
            if (Score != null) {
                EmptyScore.text = score;
            }
        }

        public void SetEmptyScore(int score)
        {
            SetEmptyScore(score.ToString());
        }
    }
}
