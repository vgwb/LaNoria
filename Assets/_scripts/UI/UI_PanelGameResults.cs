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

        public void SetScore(string score)
        {
            if (Score != null) {
                Score.text = score;
                Debug.Log("arg: "+score+" txt: "+Score.text);
            }
        }

        public void SetScore(int score)
        {
            SetScore(score.ToString());
        }
    }
}
