using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace vgwb.lanoria
{
    public class UITutorialHighlight : MonoBehaviour
    {
        #region Var
        public TutorialStep DisplayStep = TutorialStep.Confirm;
        public Image TutorialImg;
        private bool displayTutorial;
        private Vector2 originSizeDelta;
        #endregion

        #region MonoB
        void Awake()
        {
            displayTutorial = false;
            originSizeDelta = TutorialImg.rectTransform.sizeDelta;
            TutorialImg.gameObject.SetActive(false);
        }

        void Update()
        {
            HandleTutorial();
        }

        private void OnDestroy()
        {
            TutorialImg.DOKill();
        }
        #endregion

        #region Functions
        private void HandleTutorial()
        {
            bool isStepInPlay = TutorialManager.I.IsPlayingStep(DisplayStep);
            if (displayTutorial != isStepInPlay) {
                displayTutorial = isStepInPlay;
                TutorialImg.gameObject.SetActive(displayTutorial);
                if (displayTutorial) {
                    Bounce(false);
                }
            }
        }

        private void Bounce(bool increment)
        {
            float from = GameplayConfig.I.BounceMinDim;
            float to = 1.0f;
            float duration = GameplayConfig.I.BounceDuration;
            if (!increment) {
                from = 1.0f;
                to = GameplayConfig.I.BounceMinDim;
            }
            DOVirtual.Float(from, to, duration, ResizeRect).OnComplete(() => Bounce(!increment));
        }

        private void ResizeRect(float perc)
        {
            if (TutorialImg != null) {
                TutorialImg.rectTransform.sizeDelta = originSizeDelta * perc;
            }
        }
        #endregion
    }
}
