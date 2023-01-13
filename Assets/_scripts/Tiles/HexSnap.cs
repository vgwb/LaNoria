using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace vgwb.lanoria
{
    [ExecuteInEditMode]
    public class HexSnap : MonoBehaviour
    {

        public bool SnapTo0Y = false;
        public bool ShowRange = false;
        public bool ShowNeighbours = false;
        [Range(1, 10)]
        public int minRadius;
        [Range(1, 10)]
        public int maxRadius;

        public HexUtils hex => HexUtils.FromWorld(transform.position);

        public HexUtils localHex => HexUtils.FromWorld(transform.localPosition);

        public void ApplyTransform()
        {
            Vector3 newPos = localHex.ToWorld(SnapTo0Y ? 0 : transform.localPosition.y);
            transform.localPosition = newPos;

            // if (transform.localEulerAngles.y > 0) {
            //     Debug.Log(transform.localEulerAngles.y);
            // }
            // int increment = 60;
            // transform.eulerAngles = new Vector3(transform.eulerAngles.x, Mathf.Round(transform.eulerAngles.y / increment) * increment, transform.eulerAngles.z);
        }

#if UNITY_EDITOR
        void Update()
        {
            if (!Application.isPlaying) {
                ApplyTransform();
            }
        }

        void OnDrawGizmosSelected()
        {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.black;
            UnityEditor.Handles.Label(transform.position, hex.ToString(), style);
            if (ShowRange) {
                Gizmos.color = Color.cyan;
                foreach (HexUtils hex in HexUtils.Spiral(hex, minRadius, maxRadius)) {
                    Gizmos.DrawSphere(hex.ToWorld(), .25f);
                }
            }
            if (ShowNeighbours) {
                Gizmos.color = Color.red;
                foreach (HexUtils neighbour in hex.Neighbours()) {
                    Gizmos.DrawSphere(neighbour.ToWorld(), .25f);
                }
            }
        }
#endif

    }
}
