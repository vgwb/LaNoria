using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace vgwb.lanoria
{
    public class PointPreview : MonoBehaviour
    {
        public RectTransform Rect;
        public TMP_Text Txt;
        private Vector3 targetWorldPos;

        private void Update()
        {
            SetUIPosOnWorldPos();
        }

        public void Init(Vector3 worldPos, string points)
        {
            if (Rect == null) {
                Rect = GetComponent<RectTransform>();
            }

            targetWorldPos = worldPos;

            if (Txt == null) {
                Txt = GetComponent<TMP_Text>();
            }

            Txt.text = points;
        }

        public void Init(Vector3 pos, int points)
        {
            Init(pos, points.ToString());
        }

        public void SetUIPosOnWorldPos()
        {
            var activecamera = CameraManager.I.GetActiveCamera();
            Vector3 posUI = activecamera.WorldToScreenPoint(targetWorldPos);
            Rect.position = posUI;
        }
    }
}
