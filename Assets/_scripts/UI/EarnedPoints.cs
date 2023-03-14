using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace vgwb.lanoria
{
    public class EarnedPoints : MonoBehaviour
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
        private List<CanvasGroup> canvas;

        private void Awake()
        {
            canvas = new List<CanvasGroup>();
            myRect = GetComponent<RectTransform>();
        }

        public void SetPlacementPoints(int points)
        {
            if (points > 0) {
                PlacementPointsTxt.text = points.ToString();
                canvas.Add(PlacementCanvas);
            } else {
                PlacementPointsTxt.transform.parent.gameObject.SetActive(false);
            }
        }

        public void SetAdjacencyPoints(int points)
        {
            if (points > 0) {
                AdjacencyPointsTxt.text = points.ToString();
                canvas.Add(AdjacencyCanvas);
            } else {
                AdjacencyPointsTxt.transform.parent.gameObject.SetActive(false);
            }
        }

        public void SetAreaPoints(int points)
        {
            if (points > 0) {
                AreaPointsTxt.text = points.ToString();
                canvas.Add(AreaPointsCanvas);
            } else {
                AreaPointsTxt.transform.parent.gameObject.SetActive(false);
            }
        }

        public void Animate()
        {
            Sequence mySequence = DOTween.Sequence();
            float time = 0.0f;
            float fadeInTime = GameplayConfig.I.FadeInTimeScore;
            foreach (var target in canvas) {
                target.alpha = 0.0f;
                mySequence.Insert(time, target.DOFade(1.0f, fadeInTime));
                time += GameplayConfig.I.FadeInterval;
            }

            float endvalue = myRect.anchoredPosition.y + GameplayConfig.I.MovementYOffset;
            float moveDuration = GameplayConfig.I.MovementTime;
            mySequence.Insert(time, myRect.DOAnchorPosY(endvalue, moveDuration));
            mySequence.AppendCallback(() => {
                DestroyMe();
            });
        }

        private void DestroyMe()
        {
            Destroy(gameObject);
        }
    } 
}
