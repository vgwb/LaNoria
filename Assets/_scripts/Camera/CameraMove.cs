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
    private Vector3 Origin;
    private Vector3 Difference;
    private Vector3 ResetCamera;
    #endregion

    #region MonoB
    private void Start()
    {
        ResetCamera = Cam.transform.position;
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
            Difference = (worldMouse) - Cam.transform.position;
            if(drag == false) {
                drag = true;
                Origin = worldMouse;
            }
        } else {
            drag = false;
        }

        if (drag) {
            Vector3 destination = Origin - Difference;
            float deltaX = Mathf.Abs(ResetCamera.x - destination.x);
            float deltaY = Mathf.Abs(ResetCamera.y - destination.y);
            float clampX = GameplayConfig.I.MovementXClamp;
            float clampY = GameplayConfig.I.MovementYClamp;
            if (deltaX <= clampX && deltaY <= clampY) {
                Cam.transform.position = destination;
            }
        }        
    }
    #endregion
}
