using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb.lanoria
{
    public enum GameObjectEfxType
    {
        Bounce,
        Fade
    }

    public class GameObjectEfx : MonoBehaviour
    {
        #region Var
        public GameObjectEfxType EfxType;
        public Ease Curve = Ease.Linear;
        public float EfxVal = 1.5f;
        public float EfxDuration = 1.0f;
        public bool PlayOnStart = true;
        private Vector3 originScale;
        private Tweener myTween;
        #endregion

        private void Awake()
        {
            originScale = transform.localScale;
        }

        void Start()
        {
            if (PlayOnStart) {
                DoEfx();
            }
        }

        public void Stop()
        {
            if (myTween != null) {
                myTween.Kill();
                myTween = null;
            }
        }

        public void Play()
        {
            if (myTween == null) {
                DoEfx();
            }
        }

        private void DoEfx()
        {
            switch (EfxType) {
                case GameObjectEfxType.Bounce:
                    Bounce(true);
                    break;

                case GameObjectEfxType.Fade:
                    Fade(false);
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
            myTween = transform.DOScale(to, duration).SetEase(Curve).OnComplete(() => Bounce(!increment));
        }

        private void Fade(bool increment)
        {
            float from = increment ? 0.0f : 1.0f;
            float to = increment ? 1.0f : 0.0f;
            myTween = DOVirtual.Float(from, to, EfxDuration, ApplyColor).OnComplete(() => Fade(!increment));
        }

        private void ApplyColor(float targetval)
        {
            var mesh = GetComponent<MeshRenderer>();
            if (mesh != null && mesh.material.HasProperty("_BaseColor")) {
                Color target = mesh.material.GetColor("_BaseColor");
                target.a = targetval;
                mesh.material.SetColor("_BaseColor", target);
            }
        }
    }
}
