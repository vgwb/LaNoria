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
        #region Var
        [Header("UI Elements")]
        public TMP_Text CardTitle;
        public Button ClickableComp;
        public RawImage PrefabImg;
        [Header("Project Data")]
        public ProjectData CardData;

        private RectTransform rect;
        #endregion

        #region Attributes
        public RectTransform Rect
        {
            get { return rect; }
        }
        #endregion

        #region MonoB
        private void Awake()
        {
            rect = GetComponent<RectTransform>();
        }
        #endregion

        #region Functions
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
            if (rect != null) {
                float height = rect.sizeDelta.y;
                var pos = rect.localPosition;
                pos.y = -height;
                rect.localPosition = pos;
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
        #endregion
    }
}
