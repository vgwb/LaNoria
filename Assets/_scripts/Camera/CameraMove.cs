using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using vgwb.lanoria;

public class CameraMove : MonoBehaviour
{
    #region Var
    public Camera Cam;
    public bool CanMove = false;
    private bool drag = false;
    private float fixDistance;  //where the screen world point should hit!
    private Vector3 origin;
    private Vector3 difference;
    private Vector3 resetCamera;
    #endregion

    #region MonoB
    private void Start()
    {
        resetCamera = Cam.transform.position;
        fixDistance = Cam.transform.position.z;
    }

    private void LateUpdate()
    {
        if (!CanMove) {
            return;
        }

        if (Input.GetMouseButton(0)) {
            var mousePos = Input.mousePosition;
            mousePos.z = fixDistance;
            var worldMouse = Cam.ScreenToWorldPoint(mousePos);
            difference = (worldMouse) - Cam.transform.position;
            if(drag == false) {
                drag = true;
                origin = worldMouse;
            }
        } else {
            drag = false;
        }

        if (drag) {
            Vector3 destination = origin - difference;
            float deltaX = Mathf.Abs(resetCamera.x - destination.x);
            float deltaY = Mathf.Abs(resetCamera.y - destination.y);
            float clampX = GameplayConfig.I.MovementXClamp;
            float clampY = GameplayConfig.I.MovementYClamp;
            if (deltaX <= clampX && deltaY <= clampY) {
                Cam.transform.position = destination;
            }
        }        
    }
    #endregion

    #region Functions
    public void ResetCam()
    {
        Cam.transform.position = resetCamera;
    }
    #endregion
}
