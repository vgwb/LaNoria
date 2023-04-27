using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb.lanoria
{
    public class CellEfx : MonoBehaviour
    {
        #region Var
        public SpriteRenderer SpriteComp;
        private float alphaVal;
        #endregion

        #region MonoB
        private void Start()
        {
            alphaVal = SpriteComp.color.a;
            Blink(0.0f);
        }

        private void OnDestroy()
        {
            SpriteComp.DOKill();
        }
        #endregion

        #region Functions
        public void SetColor(Color color)
        {
            SpriteComp.color = color;
        }

        public void StopBlink()
        {
            SpriteComp.DOKill();
            SpriteComp.DOFade(alphaVal, 0.0f);
        }

        private void Blink(float fadeVal)
        {
            float nextFadeVal = fadeVal == 0.0f ? alphaVal : 0.0f;
            SpriteComp.DOFade(fadeVal, 0.5f).OnComplete(() => Blink(nextFadeVal));
        }
        #endregion
    }
}
