using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb.lanoria
{
    public class UICamera : MonoBehaviour
    {

        #region Var
        public Camera MyCam;
        [SerializeField] private Transform subjectPivot;
        #endregion

        #region Attributes
        public Transform SubjectPivot
        {
            get { return subjectPivot; }
        }

        public Texture CameraTexture
        {
            get { return MyCam.targetTexture; }
        }
        #endregion

        #region MonoB
        private void Awake()
        {
            if (MyCam == null) {
                MyCam = GetComponent<Camera>();
            }
        }
        #endregion

        #region Functions
        public void CleanSubject()
        {
            int childsNum = subjectPivot.childCount;
            if (childsNum > 0) {
                for (int i = 0; i < childsNum; i++) {
                    Destroy(subjectPivot.GetChild(i).gameObject);
                }
            }
        }

        public Vector3 GetSubjectRotation()
        {
            Vector3 rot = Vector3.zero;
            int childsNum = subjectPivot.childCount;
            if (subjectPivot.childCount > 0) {
                rot = subjectPivot.GetChild(0).transform.eulerAngles;
            }

            return rot;
        }
        #endregion
    }
}
