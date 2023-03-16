using vgwb.framework;
using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Common;
using DG.Tweening;
using NaughtyAttributes;

namespace vgwb.lanoria
{
    public class CameraManager : SingletonMonoBehaviour<CameraManager>
    {
        [Header("Components")]
        public Camera MyCamera;
        public CameraController CameraController;
        public List<Vector3> IntroPath;
        private int pathIndex;
        private Vector3 originalPosition;
        private Tween activeTween;

        protected override void Awake()
        {
            base.Awake();
            originalPosition = transform.position;
            pathIndex = -1;
            IntroPath.Add(originalPosition);
            activeTween = null;
            EnableCameraMove(false);
            MoveInHome();
        }

        public void EnableCameraMove(bool enable)
        {
            CameraController.CanMove = enable;
        }

        public void ResetCameraGameplay()
        {
            CameraController.ResetCam();
        }

        public void MoveInHome()
        {
            ResetMovement();
            MoveToPoint();
        }

        public void StopMoveHome()
        {
            ResetMovement();
        }

        [Button]
        public void AddIntroPathPoint()
        {
            IntroPath.Add(transform.position);
        }

        private void MoveToPoint()
        {
            pathIndex = (pathIndex + 1) % IntroPath.Count;
            Vector3 nextPoint = IntroPath[pathIndex];
            Ease curve = GameplayConfig.I.CameraHomeCurve;
            float speed = GameplayConfig.I.CameraHomeSpeed;
            float duration = Vector3.Distance(transform.position, nextPoint) / speed;
            float wait = GameplayConfig.I.CameraHomeWait;
            activeTween = DOVirtual.DelayedCall(wait, () =>
            transform.DOMove(nextPoint, duration)
                    .SetEase(curve).OnComplete(() => MoveToPoint()));
        }

        private void ResetMovement()
        {
            transform.DOKill();
            activeTween.Kill();
            transform.position = originalPosition;
            pathIndex = -1;
        }
    }
}
