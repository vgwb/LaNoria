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
        #endregion
    }
}
