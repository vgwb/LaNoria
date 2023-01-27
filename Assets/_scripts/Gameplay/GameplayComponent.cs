using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb.lanoria
{
    [RequireComponent(typeof(GameplayManager))]
    public abstract class GameplayComponent : MonoBehaviour
    {
        protected GameplayManager manager;

        protected virtual void Awake()
        {
            manager = GetComponent<GameplayManager>();
        }
    }
}
