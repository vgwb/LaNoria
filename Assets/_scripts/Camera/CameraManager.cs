using vgwb;
using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : SingletonMonoBehaviour<CameraManager>
{
    #region Var
    public Camera Cam;
    public LeanMultiUpdate LeanUpdate;
    #endregion

    #region MonoB
    protected override void Awake()
    {
        base.Awake();

        EnableRotationWithFingers(true);
    }
    #endregion

    #region Functions
    public void EnableRotationWithFingers(bool enable)
    {
        LeanUpdate.enabled = enable;
    }
    #endregion
}
