using DG.Tweening;
using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace vgwb.lanoria
{
    public class UI_GameHUD : MonoBehaviour
    {

        [Header("Panel HUD Elements")]
        public GameObject PanelHUD;
        public TMP_Text ScoreTxt;

        [Header("Panel Cards Elements")]
        public GameObject PanelCards;

        [Header("Panel Current Project Elements")]
        public RectTransform PanelCurrentProject;
        public TMP_Text ProjectTitle;
        public RawImage CurrentProjectImg;

        [Header("Panel Confirm Elements")]
        public GameObject PanelConfirm;

        private bool iSProjectPanelShifted;
        private Vector2 projectPanelOriginalPosition;

        public Transform CardContainer
        {
            get { return PanelCards.transform; }
        }

        void Awake()
        {
            iSProjectPanelShifted = false;
            projectPanelOriginalPosition = PanelCurrentProject.anchoredPosition;
        }

        public void EnableBtnConfirm(bool enable)
        {
            if (PanelConfirm != null) {
                PanelConfirm.SetActive(enable);
            }
        }

        public void SetProjectTitle(string message)
        {
            if (ProjectTitle != null) {
                ProjectTitle.text = message;
            }
        }

        public void SetScoreTxt(string newScore)
        {
            if (ScoreTxt != null) {
                ScoreTxt.text = newScore;
            }
        }

        public void SetScoreUI(int newScore)
        {
            SetScoreTxt(newScore.ToString());
        }

        public List<GameObject> CardsInUI()
        {
            var cards = new List<GameObject>();
            int cardsNum = PanelCards.transform.childCount;
            for (int i = 0; i < cardsNum; i++) {
                var child = PanelCards.transform.GetChild(i);
                cards.Add(child.gameObject);
            }

            return cards;
        }

        public void SlideToOriginalPosition()
        {
            if (iSProjectPanelShifted) {
                float duration = GameplayConfig.I.SlideProjectPanelTime;
                PanelCurrentProject.DOAnchorPosX(projectPanelOriginalPosition.x, duration).OnComplete(() => SetProjectPanelShifted(false));
            }
        }

        public void SlideOnTheRight()
        {
            if (!iSProjectPanelShifted) {
                iSProjectPanelShifted = true;
                float duration = GameplayConfig.I.SlideProjectPanelTime;
                float pixelIn = GameplayConfig.I.SlideProjectPanelPixelIn;
                SlideActualProjectPanel(duration, pixelIn, () => SetProjectPanelShifted(true));
            }
        }

        public void SetupCurrentProjectImg(Texture img)
        {
            if (CurrentProjectImg != null) {
                CurrentProjectImg.texture = img;
            }
        }

        public void EnableCurrentProjectImg(bool enable)
        {
            if (CurrentProjectImg != null) {
                CurrentProjectImg.gameObject.SetActive(enable);
            }
        }

        private void SlideActualProjectPanel(float duration, float pixelIn = 50.0f, TweenCallback callback = null)
        {
            if (PanelCurrentProject != null) {
                float width = PanelCurrentProject.sizeDelta.x;
                float destination = (width - pixelIn) + PanelCurrentProject.anchoredPosition.x;
                PanelCurrentProject.DOAnchorPosX(destination, duration).OnComplete(callback);
            }
        }

        private void SetProjectPanelShifted(bool isShifted)
        {
            iSProjectPanelShifted = isShifted;
        }
    }
}
