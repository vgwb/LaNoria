using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using vgwb.framework;

namespace vgwb.lanoria
{
    public class UICameraManager : SingletonMonoBehaviour<UICameraManager>
    {
        public List<UICamera> Cams;
        public Texture GetUICameraTexture(int cameraIndex)
        {
            if (IsIndexValid(cameraIndex)) {
                return Cams[cameraIndex].CameraTexture;
            }

            return null;
        }

        public GameObject SpawnPrefabInCamera(int cameraIndex, GameObject prefab, ProjectData projectData)
        {
            if (IsIndexValid(cameraIndex) && prefab != null) {
                CleanCameraSubject(cameraIndex);
                var container = Cams[cameraIndex].SubjectPivot;
                var instance = Instantiate(prefab);
                instance.transform.localPosition = Vector3.zero;
                var tile = instance.GetComponent<Tile>();
                if (tile != null) {
                    tile.SetupForUI();
                    var ParentInPivot = new GameObject(); // create an empty object
                    ParentInPivot.transform.position = tile.Pivot.transform.position;
                    tile.transform.parent = ParentInPivot.transform;
                    ParentInPivot.transform.parent = container;
                    ParentInPivot.transform.localPosition = Vector3.zero;
                    ParentInPivot.transform.localRotation = Quaternion.Euler(GameplayConfig.I.UIProjectRotation);

                    if (projectData != null) {
                        tile.SetupCellsColor(projectData);
                    }
                } else {
                    instance.transform.localPosition = Vector3.zero;
                }

                return instance;
            }

            return null;
        }

        public void CleanCameraSubject(int cameraIndex)
        {
            if (IsIndexValid(cameraIndex)) {
                Cams[cameraIndex].CleanSubject();
            }
        }

        public Vector3 GetUIObjectRotation(int cameraIndex)
        {
            Vector3 rot = Vector3.zero;
            if (IsIndexValid(cameraIndex)) {
                rot = Cams[cameraIndex].GetSubjectRotation();
            }
            return rot;
        }

        private bool IsIndexValid(int cameraIndex)
        {
            bool indexOk = cameraIndex >= 0 && cameraIndex < Cams.Count;
            if (indexOk) {
                return Cams[cameraIndex] != null;
            }
            return false;
        }
    }
}
