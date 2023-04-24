using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb.lanoria
{
    public enum GameObjectEfxType
    {
        Bounce
    }

    public class GameObjectEfx : MonoBehaviour
    {
        #region Var
        public GameObjectEfxType EfxType;
        public Ease Curve = Ease.Linear;
        public float EfxVal = 1.5f;
        public float EfxDuration = 1.0f;
        private Vector3 originScale;
        #endregion

        private void Awake()
        {
            originScale = transform.localScale;
        }

        void Start()
        {
            DoEfx();
        }

        private void DoEfx()
        {
            switch (EfxType) {
                case GameObjectEfxType.Bounce:
                    Bounce(true);
                    break;
                default:
                    break;
            }
        }

        private void Bounce(bool increment)
        {
            float duration = GameplayConfig.I.BounceDuration;
            Vector3 to = Vector3.one * EfxVal;
            if (!increment) {
                to = originScale;
            }
            transform.DOScale(to, duration).SetEase(Curve).OnComplete(() => Bounce(!increment));
        }
    }
}
