using vgwb.framework;
using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Common;
using DG.Tweening;

namespace vgwb.lanoria
{
    public class CameraManager : SingletonMonoBehaviour<CameraManager>
    {
        #region Var
        [Header("Components")]
        public CameraMove Mover;
        #endregion

        #region MonoB
        protected override void Awake()
        {
            base.Awake();

            EnableCameraMove(false);
        }
        #endregion

        #region Functions
        public void EnableCameraMove(bool enable)
        {
            Mover.CanMove = enable;
        }

        public void ResetCameraGameplay()
        {
            Mover.ResetCam();
        }
        #endregion
    }
}
