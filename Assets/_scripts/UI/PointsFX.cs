using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace vgwb.lanoria
{
    public class PointsFX : MonoBehaviour
    {
        [Header("Placement Points")]
        public TMP_Text PlacementPointsTxt;
        public CanvasGroup PlacementCanvas;

        [Header("Adjacency Points")]
        public TMP_Text AdjacencyPointsTxt;
        public CanvasGroup AdjacencyCanvas;

        [Header("Area Points")]
        public TMP_Text AreaPointsTxt;
        public CanvasGroup AreaPointsCanvas;

        private RectTransform myRect;
        private List<CanvasScore> canvas;

        private class CanvasScore
        {
            public CanvasGroup Canvas;
            public int Score;

            public CanvasScore(CanvasGroup canvas, int score)
            {
                Canvas = canvas;
                Score = score;
            }
        }

        private void Awake()
        {
            canvas = new List<CanvasScore>();
            myRect = GetComponent<RectTransform>();
        }

        public void SetPlacementPoints(int points)
        {
            SetCanvas(points, PlacementPointsTxt, PlacementCanvas);
        }

        public void SetAdjacencyPoints(int points)
        {
            SetCanvas(points, AdjacencyPointsTxt, AdjacencyCanvas);
        }

        public void SetAreaPoints(int points)
        {
            SetCanvas(points, AreaPointsTxt, AreaPointsCanvas);
        }

        public void Animate()
        {
            Sequence mySequence = DOTween.Sequence();
            float time = 0.0f;
            float fadeInTime = GameplayConfig.I.FadeInTimeScore;
            float moveDuration = GameplayConfig.I.MovementTime;
            foreach (var target in canvas) {
                target.Canvas.alpha = 0.0f; // set alpha to 0
                // fade in...
                mySequence.Insert(time,
                    target.Canvas.DOFade(1.0f, fadeInTime).OnComplete(() => SoundManager.I.PlaySfx(AudioEnum.score_efx)));
                time += fadeInTime;

                // ...move
                var targetRect = target.Canvas.GetComponent<RectTransform>();
                float endvalue = targetRect.anchoredPosition.y + GameplayConfig.I.MovementYOffset;
                mySequence.Insert(time,
                    targetRect.DOAnchorPosY(endvalue, moveDuration).OnComplete(() => OnEndMove(target)));
                time += moveDuration;
            }
            // destroy this object
            mySequence.AppendCallback(() => {
                DestroyMe();
            });
        }

        private void SetCanvas(int points, TMP_Text textArea, CanvasGroup targetCanvas)
        {
            if (points > 0) {
                textArea.text = "+" + points.ToString();
                CanvasScore canvasScore = new CanvasScore(targetCanvas, points);
                canvas.Add(canvasScore);
            } else {
                textArea.transform.parent.gameObject.SetActive(false);
            }
        }

        private void DestroyMe()
        {
            Destroy(gameObject);
        }

        private void OnEndMove(CanvasScore target)
        {
            int score = ScoreManager.I.Score;
            UI_manager.I.PanelGameplay.SetScoreUI(score, target.Score);
            Destroy(target.Canvas.gameObject);
        }
    }
}
