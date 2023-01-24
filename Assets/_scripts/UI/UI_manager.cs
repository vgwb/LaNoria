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

        private States currentState;

        void Start()
        {

        }

        public void Show(States area)
        {

        }

    }
}
