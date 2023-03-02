using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Tutorial : MonoBehaviour
{
    #region Var
    public List<GameObject> Introductions;
    public List<GameObject> Explanations;
    private int activeIntroduction;
    private int activeExplanation;
    private Action OnIntroOver;
    #endregion

    #region MonoB
    void Awake()
    {
        activeIntroduction = -1;
        activeExplanation = -1;
        HideAllIntroductions();
        HideAllExplanations();
    }
    #endregion

    #region Functions

    public void ResetTutorial()
    {
        OnIntroOver = null;
        activeIntroduction = -1;
        activeExplanation = -1;
        HideAllIntroductions();
        HideAllExplanations();
    }

    public void BeginTutorial(Action callback)
    {
        OpenPanel();
        ResetTutorial();
        SetupExplanation();
        SetupIntroduction(callback);
    }

    public void SetupIntroduction(Action callback)
    {
        OnIntroOver = callback;
        foreach (var intro in Introductions) {
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
        HideAllIntroductions();
        if (activeIntroduction >= 0 && activeIntroduction < Introductions.Count) {
            Introductions[activeIntroduction].SetActive(true);
        } else if (activeIntroduction == Introductions.Count) {
            if (OnIntroOver != null) {
                OnIntroOver();
            }
        }
    }

    public void SetupExplanation()
    {
        foreach (var exp in Explanations) {
            var btn = exp.GetComponentInChildren<Button>();
            if (btn != null) {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => CloseExplanation());
            }
        }
    }

    public void ShowNextExplanation()
    {
        OpenPanel();
        activeExplanation++;
        HideAllExplanations();
        if (activeExplanation >= 0 && activeExplanation < Explanations.Count) {
            Explanations[activeExplanation].SetActive(true);
        } else if (activeExplanation == Explanations.Count) {

        }
    }

    public void CloseExplanation()
    {
        if (activeExplanation >= 0 && activeExplanation < Explanations.Count) {
            Explanations[activeExplanation].SetActive(false);
        }
        ClosePanel();
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    private void HideAllIntroductions()
    {
        foreach (var intro in Introductions) {
            intro.SetActive(false);
        }
    }

    private void HideAllExplanations()
    {
        foreach (var exp in Explanations) {
            exp.SetActive(false);
        }
    }
    #endregion
}
