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
        private int _score;

        void Start()
        {
            PunchAnimation = DOTween.Sequence()
           .Insert(0, ScoreGO.transform.DOPunchScale(new Vector3(0.3f, 0.3f, 0.3f), 1, 3, 0f))
           .SetAutoKill(false).Pause();
        }

        public void Init(int score)
        {
            _score = score;
            ScoreText.text = _score.ToString();
        }

        public void AddScore(int totalScore, int deltaPoints)
        {
            Debug.Log("AddScore +" + deltaPoints + " TOTLE: " + totalScore);
            _score += deltaPoints;
            //            bonusGO = Instantiate(BonusPrefab, transform);
            //            bonusGO.GetComponent<ui_score_bonus>().Init(deltaPoints);
            UpdateScore(_score);
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
