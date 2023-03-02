using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using vgwb.framework;

namespace vgwb.lanoria
{
    public class TutorialManager : SingletonMonoBehaviour<TutorialManager>
    {
        #region Var
        [SerializeField] private int activeIntroduction;
        [SerializeField] private int activeExplanation;
        private int turn;
        private UI_Tutorial UITutorial;
        private Action OnIntroOver;
        #endregion

        #region MonoB
        void Start()
        {
            UITutorial = UI_manager.I.PanelTutorial;
            ResetTutorial();
        }
        #endregion

        #region Functions
        public void ResetTutorial()
        {
            OnIntroOver = null;
            turn = 1;
            activeIntroduction = -1;
            activeExplanation = 0;
            UITutorial.HideAllIntroductions();
            UITutorial.HideAllExplanations();
        }

        public void BeginTutorial(Action callback)
        {
            UITutorial.OpenPanel();
            ResetTutorial();
            SetupExplanation();
            SetupIntroduction(callback);
        }

        public void SetupIntroduction(Action callback)
        {
            OnIntroOver = callback;
            foreach (var intro in UITutorial.Introductions) {
                var btn = intro.GetComponentInChildren<Button>();
                if (btn != null) {
                    btn.onClick.RemoveAllListeners();
                    btn.onClick.AddListener(() => ShowNextIntroduction());
                }
            }
        }

        public void ShowNextIntroduction()
        {
            activeIntroduction++;
            UITutorial.HideAllIntroductions();
            if (activeIntroduction >= 0 && activeIntroduction < UITutorial.Introductions.Count) {
                UITutorial.Introductions[activeIntroduction].SetActive(true);
            } else if (activeIntroduction == UITutorial.Introductions.Count) {
                if (OnIntroOver != null) {
                    OnIntroOver();
                }
            }
        }

        public void SetupExplanation()
        {
            foreach (var exp in UITutorial.Explanations) {
                var btn = exp.GetComponentInChildren<Button>();
                if (btn != null) {
                    btn.onClick.RemoveAllListeners();
                    //btn.onClick.AddListener(() => CloseExplanation());
                }
            }
        }

        public void ShowExplanation(int tutorialKey)
        {
            if (activeExplanation != tutorialKey || !IsFirstTurn()) {
                return;
            }

            UITutorial.OpenPanel();
            UITutorial.HideAllExplanations();
            if (activeExplanation >= 0 && activeExplanation < UITutorial.Explanations.Count) {
                UITutorial.Explanations[activeExplanation].SetActive(true);
            } else if (activeExplanation == UITutorial.Explanations.Count) {

            }
        }

        public void CloseExplanation(int tutorialKey)
        {
            if (activeExplanation != tutorialKey || !IsFirstTurn()) {
                return;
            }

            if (activeExplanation >= 0 && activeExplanation < UITutorial.Explanations.Count) {
                UITutorial.Explanations[activeExplanation].SetActive(false);
            }
            activeExplanation++;
            UITutorial.ClosePanel();
        }

        public void Close()
        {
            UITutorial.ClosePanel();
        }

        public bool IsExplanationReached(int tutorialKey)
        {
            return activeExplanation >= tutorialKey;
        }

        public void EndTurn()
        {
            turn++;
        }

        private bool IsFirstTurn()
        {
            return turn == 1;
        }
        #endregion
    }
}
