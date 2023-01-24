using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb.lanoria
{
    public class UI_manager : MonoBehaviour
    {
        public enum States
        {
            Home = 1,
            About = 2,
            Play = 3,
            Options = 4
        }

        public UI_PanelMenu PanelMenu;
        public UI_PanelAbout PanelAbout;
        public UI_GameHUD PanelGameHUD;
        public UI_PanelProjectDetail PanelProjectDetail;
        public UI_PanelOptions PanelOptions;
        public UI_PanelGamePause PanelGamePause;

        private States currentUIState;

        void Start()
        {

        }

        public void Show(States newState)
        {
            CloseState(currentUIState);
            switch (newState) {
                case States.Home:
                    PanelMenu.OpenPanel();
                    break;
                case States.About:
                    PanelAbout.OpenPanel();
                    break;
                case States.Options:
                    PanelOptions.OpenPanel();
                    break;
            }
            currentUIState = newState;
        }

        private void CloseState(States state)
        {
            switch (state) {
                case States.Home:
                    PanelMenu.ClosePanel();
                    break;
                case States.About:
                    PanelAbout.ClosePanel();
                    break;
                case States.Options:
                    PanelOptions.ClosePanel();
                    break;
            }
        }

    }
}
