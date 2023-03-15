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
        public TMP_Text PlacementTxt;
        public TMP_Text PlacementPointsTxt;
        public CanvasGroup PlacementCanvas;

        [Header("Adjacency Points")]
        public TMP_Text AdjacencyTxt;
        public TMP_Text AdjacencyPointsTxt;
        public CanvasGroup AdjacencyCanvas;

        [Header("Area Points")]
        public TMP_Text AreaTxt;
        public TMP_Text AreaPointsTxt;
        public CanvasGroup AreaPointsCanvas;

        private RectTransform myRect;
        private List<CanvasGroup> canvas;

        private void Awake()
        {
            canvas = new List<CanvasGroup>();
            myRect = GetComponent<RectTransform>();
            SetOutline(PlacementTxt);
            SetOutline(AdjacencyTxt);
            SetOutline(AreaTxt);
        }

        public void SetPlacementPoints(int points)
        {
            if (points > 0) {
                PlacementPointsTxt.text = points.ToString();
                PlacementPointsTxt.outlineWidth = 0.25f;
                PlacementPointsTxt.outlineColor = Color.white;
                canvas.Add(PlacementCanvas);
            } else {
                PlacementPointsTxt.transform.parent.gameObject.SetActive(false);
            }
        }

        public void SetAdjacencyPoints(int points)
        {
            if (points > 0) {
                AdjacencyPointsTxt.text = points.ToString();
                AdjacencyPointsTxt.outlineWidth = 0.25f;
                AdjacencyPointsTxt.outlineColor = Color.white;
                canvas.Add(AdjacencyCanvas);
            } else {
                AdjacencyPointsTxt.transform.parent.gameObject.SetActive(false);
            }
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
            foreach (var target in canvas) {
                target.alpha = 0.0f;
                mySequence.Insert(time,
                    target.DOFade(1.0f, fadeInTime).OnComplete(() => SoundManager.I.PlaySfx(AudioEnum.score_efx)));
                time += GameplayConfig.I.FadeInterval;
            }

            float endvalue = myRect.anchoredPosition.y + GameplayConfig.I.MovementYOffset;
            float moveDuration = GameplayConfig.I.MovementTime;
            mySequence.Insert(time, myRect.DOAnchorPosY(endvalue, moveDuration));
            mySequence.AppendCallback(() => {
                DestroyMe();
            });
        }

        private void SetCanvas(int points, TMP_Text textArea, CanvasGroup targetCanvas)
        {
            if (points > 0) {
                textArea.text = points.ToString();
                SetOutline(textArea);
                canvas.Add(targetCanvas);
            } else {
                textArea.transform.parent.gameObject.SetActive(false);
            }
        }

        private void SetOutline(TMP_Text targetTxt)
        {
            targetTxt.outlineWidth = GameplayConfig.I.TextOutlineWidth;
            targetTxt.outlineColor = GameplayConfig.I.TextOutlineColor;
        }

        private void DestroyMe()
        {
            Destroy(gameObject);
        }
    }
}
