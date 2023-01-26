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
        [Header("Other comps")]
        public LeanSpawnWithFinger Spawner;
        public CanvasGroup Canvas;

        [Header("Panel HUD Elements")]
        public GameObject PanelHUD;
        public TMP_Text ScoreTxt;

        [Header("Panel Cards Elements")]
        public GameObject PanelCards;
        public List<GameObject> Hooks;

        [Header("Panel Current Project")]
        public RectTransform PanelCurrentProject;
        public TMP_Text ProjectTitle;
        public RawImage CurrentProjectImg;
        public LeanFingerDownCanvas LeanSpawnCanvas;
        public Button BtnDetailProject;

        [Header("Panel Confirm Elements")]
        public GameObject PanelConfirm;
        public Button BtnConfirm;

        [Header("Preview")]
        public Transform PanelPreview;

        public delegate void GameHUDEvent();
        public GameHUDEvent OnProjectDragged;
        public GameHUDEvent OnCurrentProjectSelected;

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

            BtnDetailProject.onClick.AddListener(() => OnOpenProjectDetail());
        }


        public void OpenPanel()
        {
            gameObject.SetActive(true);
        }

        public void ClosePanel()
        {
            gameObject.SetActive(false);
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
            for (int i = 0; i < Hooks.Count; i++) {
                if (Hooks[i].transform.childCount > 0) {
                    var card = Hooks[i].transform.GetChild(0);
                    if (card != null) {
                        cards.Add(card.gameObject);
                    }
                }
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

        public void EnableFingerCanvas(bool enable)
        {
            if (LeanSpawnCanvas != null) {
                LeanSpawnCanvas.enabled = enable;
            }
        }

        public void EnableCanvas(bool enable)
        {
            if (Canvas != null) {
                Canvas.alpha = enable ? 1.0f : 0.0f;
                Canvas.blocksRaycasts = enable;
                Canvas.interactable = enable;
            }
        }

        public void SetCanvasAlpha(float alpha)
        {
            if (Canvas != null) {
                Canvas.alpha = alpha;
            }
        }

        public void SetCanvasInteractable(bool enable)
        {
            if (Canvas != null) {
                Canvas.blocksRaycasts = enable;
                Canvas.interactable = enable;
            }
        }

        public void FadeCanvas(float endVal, float duration = 1.0f, TweenCallback callback = null)
        {
            if (Canvas != null) {
                Canvas.DOFade(endVal, duration).OnComplete(callback);
            }
        }

        public GameObject GetHook(int index)
        {
            if (index >= 0 && index < Hooks.Count) {
                return Hooks[index];
            }

            return null;
        }

        public void ResetProjectPanel()
        {
            SlideOnTheRight();
            SetProjectTitle("");
            EnableBtnConfirm(false);
            SetupCurrentProjectImg(null);
        }

        public void OnOpenProjectDetail()
        {
            Debug.Log("SHOW PROJECT ");
        }

        public void PrefabSelectionHUD()
        {
            SlideOnTheRight();
            EnableCurrentProjectImg(false);
        }

        public void CardSelectionHUD(string title, Texture projectImg)
        {
            SetProjectTitle(title);
            EnableBtnConfirm(false);
            SlideToOriginalPosition();
            SetupCurrentProjectImg(projectImg);
            EnableCurrentProjectImg(true);
        }

        public void OnProjectDragFromCanvas()
        {
            if (OnProjectDragged != null) {
                OnProjectDragged();
            }
        }

        public void OnProjectSelectedFromCanvas()
        {
            if (OnCurrentProjectSelected != null) {
                OnCurrentProjectSelected();
            }
        }

        public List<GameObject> GetPreviewElements()
        {
            if (PanelPreview == null) {
                return null;
            }

            List<GameObject> elements = new List<GameObject>();
            int childs = PanelPreview.childCount;
            for (int i = 0; i < childs; i++) {
                elements.Add(PanelPreview.GetChild(i).gameObject);
            }

            return elements;
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
