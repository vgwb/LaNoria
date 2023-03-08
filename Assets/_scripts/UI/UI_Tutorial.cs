using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Tutorial : MonoBehaviour
{
    public List<GameObject> Introductions;
    public List<GameObject> Explanations;

    public void OpenPanel()
    {
        gameObject.SetActive(true);
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    public void HideAllIntroductions()
    {
        foreach (var intro in Introductions) {
            intro.SetActive(false);
        }
    }

    public void HideAllExplanations()
    {
        foreach (var exp in Explanations) {
            exp.SetActive(false);
        }
    }
}
