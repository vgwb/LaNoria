using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace vgwb.lanoria
{
    public class CardInGame : MonoBehaviour
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

        public void InitCard(ProjectData cardData, Texture texture)
        {
            CardData = cardData;
            SetCardTitle(CardData.Title);
            SetPrefabImg(texture); // visualize the 3d prefab into the canvas
        }

        public void SetCardEvents(UnityAction action)
        {
            if (action != null) {
                ClickableComp.onClick.AddListener(action);
            }
        }

        public void SetCardAsPlayable(bool isPlayable)
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

        private void SetCardTitle(string title)
        {
            if (CardTitle != null) {
                CardTitle.text = title;
            }
        }

        private void SetPrefabImg(Texture texture)
        {
            if (PrefabImg != null) {
                PrefabImg.texture = texture;
            }
        }
    }
}
