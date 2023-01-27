using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb.lanoria
{
    [RequireComponent(typeof(GameManager))]
    public abstract class GameplayComponent : MonoBehaviour
    {
        protected GameManager manager;

        protected virtual void Awake()
        {
            manager = GetComponent<GameManager>();
        }
    }
}
