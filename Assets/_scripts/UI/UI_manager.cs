using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using vgwb.framework;

namespace vgwb.lanoria
{
    public class UI_manager : SingletonMonoBehaviour<UI_manager>
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
        public UI_Gameplay PanelGameplay;
        public UI_PanelProjectDetail PanelProjectDetail;
        public UI_PanelOptions PanelOptions;
        public UI_PanelGamePause PanelGamePause;
        public UI_PanelGameResults PanelGameResults;
        public UI_Tutorial PanelTutorial;

        private States currentUIState;

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
                case States.Play:
                    PanelGameplay.OpenPanel();
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
                case States.Play:
                    PanelGameplay.ClosePanel();
                    break;
            }
        }

        public void OpenProjectDetail(ProjectData data)
        {
            PanelProjectDetail.OpenPanel(data);
        }

        public void ShowGamePlay(bool status)
        {
            if (status) {
                PanelGameplay.OpenPanel();
            } else {
                PanelGameplay.ClosePanel();
            }
        }

        public void ShowGamePause(bool status)
        {
            if (status) {
                PanelGamePause.OpenPanel();
            } else {
                PanelGamePause.ClosePanel();
            }
        }

        public void ShowGameResult(bool status)
        {
            if (status) {
                PanelGameResults.OpenPanel();
            } else {
                PanelGameResults.ClosePanel();
            }
        }

    }
}
