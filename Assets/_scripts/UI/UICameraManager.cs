using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using vgwb.framework;

namespace vgwb.lanoria
{
    public class UICameraManager : SingletonMonoBehaviour<UICameraManager>
    {
        #region Var
        public List<UICamera> Cams;
        #endregion

        #region Functions
        public Texture GetUICameraTexture(int cameraIndex)
        {
            if (IsIndexValid(cameraIndex)) {
                return Cams[cameraIndex].CameraTexture;
            }

            return null;
        }

        public GameObject SpawnPrefabInCamera(int cameraIndex, GameObject prefab, ProjectData projectData)
        {
            //            Debug.Log("Call function!!! "+prefab);
            if (IsIndexValid(cameraIndex) && prefab != null) {
                CleanCameraSubject(cameraIndex);
                //                Debug.Log("Call function!!!");
                var container = Cams[cameraIndex].SubjectPivot;
                var instance = Instantiate(prefab);
                instance.transform.localPosition = Vector3.zero;
                var placeable = instance.GetComponent<Placeable>();
                if (placeable != null) {
                    placeable.SetupForUI();

                    var ParentInPivot = new GameObject(); // create an empty object
                    ParentInPivot.transform.position = placeable.Pivot.transform.position;
                    placeable.transform.parent = ParentInPivot.transform;
                    ParentInPivot.transform.parent = container;
                    ParentInPivot.transform.localPosition = Vector3.zero;
                    ParentInPivot.transform.localRotation = Quaternion.identity;

                    if (projectData != null) {
                        placeable.SetupCellsColor(projectData);
                    }
                }
                else {
                    instance.transform.localPosition = Vector3.zero;
                }
            }

            return null;
        }

        public void CleanCameraSubject(int cameraIndex)
        {
            if (IsIndexValid(cameraIndex)) {
                Cams[cameraIndex].CleanSubject();
            }
        }

        private bool IsIndexValid(int cameraIndex)
        {
            bool indexOk = cameraIndex >= 0 && cameraIndex < Cams.Count;
            if (indexOk) {
                return Cams[cameraIndex] != null;
            }

            return false;
        }
        #endregion
    }
}
