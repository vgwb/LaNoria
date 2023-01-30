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
        public Camera Cam;
        public LeanMultiUpdate LeanUpdate;
        public LeanPitchYaw PitchYaw;
        public LeanPitchYawAutoRotate AutoRotate;

        private Vector3 originalRot;
        #endregion

        #region MonoB
        protected override void Awake()
        {
            base.Awake();

            originalRot = transform.eulerAngles;
            EnableRotationWithFingers(false);
        }
        #endregion

        #region Functions
        public void EnableRotationWithFingers(bool enable)
        {
            if (LeanUpdate != null) {
                LeanUpdate.enabled = enable;
            }
        }

        public void EnableAutoRotate(bool enable)
        {
            if (AutoRotate != null) {
                AutoRotate.enabled = enable;
            }
        }

        public void EnablePitchYaw(bool enable)
        {
            if (PitchYaw != null) {
                PitchYaw.enabled = enable;
            }
        }

        public void ResetToOriginalRotY(TweenCallback callback)
        {
            float fromVal = PitchYaw.Yaw % 360.0f;
            float toVal = originalRot.y;
            float duration = GameplayConfig.I.ResetCameraRotYOnPlay;
            DOVirtual.Float(fromVal, toVal, duration, y => PitchYaw.Yaw = y).OnComplete(callback);
        }
        #endregion
    }
}
