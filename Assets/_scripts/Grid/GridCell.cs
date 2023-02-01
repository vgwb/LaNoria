using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DG.Tweening;

namespace vgwb.lanoria
{
    public class GridCell : GenericCell
    {
        public bool Occupied;

        public Region Region = Region.Region_1;
        public AreaId Area = AreaId.undefined;

        private bool highlighted;

        protected override void Awake()
        {
            base.Awake();
            //            Debug.Log("category: " + category);
        }

        public void Init(bool isOccupied)
        {
            Occupied = isOccupied;
            highlighted = false;
            BaseSetup();
        }

        public bool IsCapital()
        {
            return Area == AreaId.Subregion_7;
        }

        public void Highlight(bool doHighlight)
        {
            //Debug.Log("HIGHLITH " + status);
            if (doHighlight && !highlighted) {
                highlighted = true;
                transform.DORotate(new Vector3(90, 0, 0), 1);
                //transform.localPosition.Set(transform.localPosition.x, 10, transform.localPosition.z);
            }
            if (!doHighlight && highlighted) {
                highlighted = false;
                transform.DORotate(Vector3.zero, 1);
                //transform.localPosition.Set(transform.localPosition.x, 0, transform.localPosition.z);
            }
        }
    }
}
