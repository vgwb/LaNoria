using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using vgwb.framework;

namespace vgwb.lanoria
{
    [Serializable]
    public class MapTutorialPanel
    {
        public TutorialStep Key;
        public int PanelIndex;
        public bool IsCompleted;
    }

    public class TutorialManager : SingletonMonoBehaviour<TutorialManager>
    {
        public List<MapTutorialPanel> TutorialSteps;
        [SerializeField] private int activeIntroduction;
        [SerializeField] private int turn;
        [SerializeField] private TutorialStep savedStep;
        private UI_Tutorial UITutorial;
        private Action OnIntroOver;

        void Start()
        {
            UITutorial = UI_manager.I.PanelTutorial;
            ResetTutorial();
        }

        public void ResetTutorial()
        {
            OnIntroOver = null;
            turn = 1;
            savedStep = TutorialStep.None;
            activeIntroduction = -1;
            UITutorial.HideAllIntroductions();
            UITutorial.HideAllExplanations();
        }

        public void BeginTutorial(Action callback)
        {
            UITutorial.OpenPanel();
            ResetTutorial();
            SetupTutorialSteps();
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

        public void SetupTutorialSteps()
        {
            foreach (var exp in UITutorial.Explanations) {
                var btn = exp.GetComponentInChildren<Button>();
                if (btn != null) {
                    btn.onClick.RemoveAllListeners();
                    //btn.onClick.AddListener(() => CloseExplanation());
                }
            }
        }

        public void ShowTutorialStep(TutorialStep tutorialKey, int turnValidity = 1)
        {
            if (!IsValidKey(tutorialKey) || !IsTurnOk(turnValidity) || IsExplanationCompleted(tutorialKey)) {
                return;
            }

            savedStep = tutorialKey; // store the key
            //Debug.Log("Display: "+tutorialKey.ToString());
            UITutorial.OpenPanel();
            UITutorial.HideAllExplanations();
            int panelIndex = GetTutorialPanel(tutorialKey);
            if (panelIndex >= 0) {
                UITutorial.Explanations[panelIndex].SetActive(true);
            }
        }

        public void CloseTutorialStep(TutorialStep tutorialKey, int turnValidity = 1)
        {
            if (savedStep != tutorialKey || !IsTurnOk(turnValidity)) {
                return;
            }

            int panelIndex = GetTutorialPanel(tutorialKey);
            if (panelIndex >= 0) {
                UITutorial.Explanations[panelIndex].SetActive(false);
            }

            savedStep = TutorialStep.None;
            SetTutorialCompleted(tutorialKey);
            UITutorial.ClosePanel();
        }

        public void Close()
        {
            UITutorial.ClosePanel();
        }

        public bool IsExplanationCompleted(TutorialStep tutorialKey)
        {
            var tuple = TutorialSteps.Find(x => x.Key == tutorialKey);
            if (tuple != null) {
                return tuple.IsCompleted;
            }

            return false;
        }

        public void EndTurn()
        {
            turn++;
        }

        public bool IsPlayingStep(TutorialStep step)
        {
            return savedStep == step;
        }

        /// <summary>
        /// Check if the turn is good. A value < 1 is always ok.
        /// </summary>
        /// <returns></returns>
        private bool IsTurnOk(int turnValidity)
        {
            if (turnValidity < 1) { //
                return true;
            } else {
                return turn == turnValidity;
            }
        }

        private bool IsValidKey(TutorialStep tutorialKey)
        {
            return savedStep == TutorialStep.None;
        }

        private int GetTutorialPanel(TutorialStep key)
        {
            var tuple = TutorialSteps.Find(x => x.Key == key);

            if (tuple != null) {
                return tuple.PanelIndex;
            }

            return -1;
        }

        private void SetTutorialCompleted(TutorialStep key)
        {
            var tuple = TutorialSteps.Find(x => x.Key == key);

            if (tuple != null) {
                tuple.IsCompleted = true;
            }
        }
    }
}
