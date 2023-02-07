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
        public GameObject CameraGameplayPrefab;
        public LeanMultiUpdate LeanUpdate;
        public LeanPitchYaw PitchYaw;
        public LeanPitchYawAutoRotate AutoRotate;

        [SerializeField] private Camera cameraMenu;
        private Camera cameraGameplay;
        private GameObject cameraGameplayContainer;
        private CameraMove cameraMover;
        private Vector3 originalRot;
        private Vector3 startMoveFrom;
        private Vector3 deltaMov;
        private Camera activeCamera;
        private bool drag;
        #endregion

        #region MonoB
        protected override void Awake()
        {
            base.Awake();

            Setup();
            originalRot = transform.eulerAngles;
            EnableRotationWithFingers(false);
            EnableCameraMove(false);
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

        public Camera GetActiveCamera()
        {
            return activeCamera;
        }

        public void SwitchToMenuCamera()
        {
            activeCamera = cameraMenu;
            cameraMenu.tag = "MainCamera";
            cameraGameplay.tag = "Untagged";
            cameraMenu.gameObject.SetActive(true);
            cameraGameplay.gameObject.SetActive(false);
        }

        public void SwitchToPlayCamera()
        {
            activeCamera = cameraGameplay;
            cameraMenu.tag = "Untagged";
            cameraGameplay.tag = "MainCamera";
            cameraGameplay.gameObject.SetActive(true);
            cameraMenu.gameObject.SetActive(false);
        }

        public void EnableCameraMove(bool enable)
        {
            cameraMover.CanMove = enable;
        }

        private void Setup()
        {
            if (CameraGameplayPrefab == null) {
                Debug.LogError("CameraManager - Setup(): no camera prefab defined!");
                return;
            }

            cameraGameplayContainer = Instantiate(CameraGameplayPrefab);
            cameraGameplay = cameraGameplayContainer.GetComponentInChildren<Camera>();
            cameraGameplay.fieldOfView = cameraMenu.fieldOfView;
            cameraMover = cameraGameplayContainer.GetComponentInChildren<CameraMove>();
            SwitchToMenuCamera();

            cameraGameplay.transform.position = cameraMenu.transform.position;
            cameraGameplay.transform.rotation = cameraMenu.transform.rotation;
        }
        #endregion
    }
}
