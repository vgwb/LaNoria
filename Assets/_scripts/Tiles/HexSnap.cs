using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace vgwb
{
    [ExecuteInEditMode]
    public class HexSnap : MonoBehaviour
    {

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
            Vector3 newPos = localHex.ToWorld(0f);
            transform.localPosition = newPos;
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
            UnityEditor.Handles.Label(transform.position, hex.ToString());
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
