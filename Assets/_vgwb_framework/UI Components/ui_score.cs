using vgwb.lanoria;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace vgwb.framework.ui
{
    public class ui_score : MonoBehaviour
    {
        public GameObject ScoreGO;
        public TextMeshProUGUI ScoreText;
        public GameObject BonusPrefab;

        private Sequence PunchAnimation;
        private GameObject bonusGO;

        void Start()
        {
            ScoreText.text = "";

            PunchAnimation = DOTween.Sequence()
           .Insert(0, ScoreGO.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 1, 3, 0f))
           .SetAutoKill(false).Pause();
        }

        public void Init(int score)
        {
            ScoreText.text = score.ToString();
        }

        public void AddScore(int totalScore, int deltaPoints)
        {
            bonusGO = Instantiate(BonusPrefab, transform);
            bonusGO.GetComponent<ui_score_bonus>().Init(deltaPoints);
            UpdateScore(totalScore);
        }

        private void UpdateScore(int value)
        {
            ScoreText.text = value.ToString();
            PunchAnimation.Rewind();
            PunchAnimation.Play();
            SoundManager.I.PlaySfx(AudioEnum.score);
        }
    }
}
