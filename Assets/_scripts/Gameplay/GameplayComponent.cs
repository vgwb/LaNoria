using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb.lanoria
{
    [RequireComponent(typeof(GameplayManager))]
    public abstract class GameplayComponent : MonoBehaviour
    {
        #region Var
        protected GameplayManager manager;
        #endregion

        #region MonoB
        protected virtual void Awake()
        {
            manager = GetComponent<GameplayManager>();
        }
        #endregion
    }
}
