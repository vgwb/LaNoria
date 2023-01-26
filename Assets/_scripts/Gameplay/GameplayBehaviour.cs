using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb.lanoria
{
    public enum GameplayState
    {
        None,
        Intro,
        Setup,
        Drawing,
        Play,
        End,
        Pause
    }

    public class GameplayBehaviour : MonoBehaviour
    {
        #region Var
        public delegate void GameplayStateEvent(GameplayState state);
        public GameplayStateEvent OnStateUpdate;

        [SerializeField] private GameplayState state;
        #endregion

        #region MonoB
        void Awake()
        {
            state = GameplayState.None;
        }
        #endregion

        #region Functions
        public void SetState(GameplayState newState)
        {
            if (state != newState) {
                state = newState;

                if (OnStateUpdate != null) {
                    OnStateUpdate(state);
                }
            }
        }
        #endregion
    }
}
