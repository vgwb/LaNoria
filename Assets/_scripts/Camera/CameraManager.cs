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
        [Header("Components")]
        public Camera MyCamera;
        public CameraController CameraController;

        protected override void Awake()
        {
            base.Awake();
            EnableCameraMove(false);
        }

        public void EnableCameraMove(bool enable)
        {
            CameraController.CanMove = enable;
        }

        public void ResetCameraGameplay()
        {
            CameraController.ResetCam();
        }
    }
}
