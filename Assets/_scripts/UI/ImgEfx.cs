using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb.lanoria
{
    public enum ImgEfxType
    {
        Punch,
        Bounce
    }

    public class ImgEfx : MonoBehaviour
    {
        public RectTransform MyRect;
        public ImgEfxType EfxType;
        public Ease Curve = Ease.Linear;
        public float EfxVal = 1.5f;
        public float EfxDuration = 1.0f;

        void Start()
        {
            DoEfx();        
        }

        private void DoEfx()
        {
            switch (EfxType) {
                case ImgEfxType.Punch:
                    Punch();
                    break;

                case ImgEfxType.Bounce:
                    Bounce(true);
                    break;

                default:
                    break;
            }
        }

        private void Punch()
        {
            Vector2 punch = Vector2.one * EfxVal;
            MyRect.DOPunchAnchorPos(punch, EfxDuration).OnComplete(() => Punch());
        }

        private void Bounce(bool increment)
        {
            Vector2 to = increment ? MyRect.sizeDelta * EfxVal : MyRect.sizeDelta / EfxVal;
            MyRect.DOSizeDelta(to, EfxDuration).SetEase(Curve).OnComplete(() => Bounce(!increment));
        }
    } 
}
