using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace vgwb.lanoria
{
    public class Card : MonoBehaviour
    {
        [Header("UI Elements")]
        public TMP_Text CardTitle;
        public Button ClickableComp;
        public RawImage PrefabImg;

        [HideInInspector]
        public ProjectData CardData;

        public RectTransform Rect { get; private set; }

        private void Awake()
        {
            Rect = GetComponent<RectTransform>();
        }

        public void Init(ProjectData cardData, Texture texture)
        {
            CardData = cardData;
            SetTitle(CardData.Title);
            SetImage(texture); // visualize the 3d prefab into the canvas
        }

        public void SetEvents(UnityAction action)
        {
            if (action != null) {
                ClickableComp.onClick.AddListener(action);
            }
        }

        public void SetPlayable(bool isPlayable)
        {
            if (ClickableComp != null) {
                ClickableComp.interactable = isPlayable;
            }
        }

        public void HideInBottomScreen()
        {
            if (Rect != null) {
                float height = Rect.sizeDelta.y;
                var pos = Rect.localPosition;
                pos.y = -height;
                Rect.localPosition = pos;
            }
        }

        public bool IsPlayable()
        {
            if (ClickableComp != null) {
                return ClickableComp.interactable;
            }

            return false;
        }

        private void SetTitle(string title)
        {
            if (CardTitle != null) {
                CardTitle.text = title;
            }
        }

        private void SetImage(Texture texture)
        {
            if (PrefabImg != null) {
                PrefabImg.texture = texture;
            }
        }
    }
}
