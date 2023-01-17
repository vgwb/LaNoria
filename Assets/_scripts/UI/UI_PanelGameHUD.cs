using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace vgwb.lanoria
{
    public class UI_PanelGameHUD : MonoBehaviour
    {
        #region Var
        [Header("Panel HUD Elements")]
        public GameObject PanelHUD;
        [Header("Panel Cards Elements")]
        public GameObject PanelCards;
        [Header("Panel Current Project Elements")]
        public GameObject PanelCurrentProject;
        public TMP_Text ProjectTitle;
        [Header("Panel Confirm Elements")]        
        public GameObject PanelConfirm;
        #endregion

        #region Attributes
        public Transform CardContainer
        {
            get { return PanelCards.transform; }
        }
        #endregion

        #region MonoB
        void Awake()
        {

        }
        #endregion

        #region Functions
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

        public List<GameObject> CardsInUI()
        {
            List<GameObject> cards = new List<GameObject>();
            int cardsNum = PanelCards.transform.childCount;
            for (int i = 0; i < cardsNum; i++) {
                var child = PanelCards.transform.GetChild(i);
                cards.Add(child.gameObject);
            }

            return cards;
        }
        #endregion
    }
}
