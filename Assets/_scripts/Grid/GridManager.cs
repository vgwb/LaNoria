using vgwb.framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace vgwb.lanoria
{
    [System.Serializable]
    public class SubregionColorDebug
    {
        public Subregion SubregionRef;
        public Color ColorDebug;
    }

    public class GridManager : SingletonMonoBehaviour<GridManager>
    {
        #region Var
        public List<GridCell> Cells;
        [Header("Subregion Debug")]
        public bool ShowSubregionColor = false;
        public List<SubregionColorDebug> SubRegionDebug;
        #endregion

        #region MonoB
        protected override void Awake()
        {
            base.Awake();

            ShowSubregionColor = false;
        }

        private void Start()
        {
            Cells.Clear();
            InitCells();
        }

        private void OnDrawGizmos()
        {
            if (ShowSubregionColor) {
                foreach (var cell in Cells) {
                    var tuple = SubRegionDebug.Find(x => x.SubregionRef == cell.MySubregion);
                    var pos = cell.transform.position;
                    if (tuple != null) {
                        Gizmos.color = tuple.ColorDebug;
                        Gizmos.DrawSphere(pos, 0.3f);
                    }
                }
            }
        }
        #endregion

        #region Functions

        public void InitCells()
        {
            Cells.Clear();
            int childs = transform.childCount;
            for (int i = 0; i < childs; i++) {
                var cellObj = transform.GetChild(i).gameObject;
                var gridCell = cellObj.GetComponent<GridCell>();
                gridCell.Init(gridCell.IsCapital() ? true : false);
                Cells.Add(gridCell);
            }
        }

        public bool IsCellOccupiedByPos(Vector3 pos)
        {
            Vector3 hexPos = RetrieveHexPos(pos);
            var cell = Cells.Find(x => x.HexPosition == hexPos);
            if (cell != null) {
                return cell.Occupied;
            }

            return true;
        }

        public bool IsOutOfMap(Vector3 pos)
        {
            Vector3 hexPos = RetrieveHexPos(pos);
            var cell = Cells.Find(x => x.HexPosition == hexPos);

            return cell == null;
        }

        public void OccupyCellByPosition(Vector3 pos, ProjectCategories category)
        {
            Vector3 hexPos = RetrieveHexPos(pos);
            var cell = Cells.Find(x => x.HexPosition == hexPos);
            if (cell != null) {
                cell.Occupied = true;
                cell.SetupCategory(category);
            }
        }

        public Vector3 ClosestBorderPoint(Vector3 pos)
        {
            Vector3 result = Vector3.negativeInfinity;
            float distance = float.PositiveInfinity;
            foreach (var cell in Cells) {
                float d = Vector3.Distance(pos, cell.HexPosition);
                if (d < distance) {
                    distance = d;
                    result = cell.HexPosition;
                }
            }

            return result;
        }

        private Vector3 RetrieveHexPos(Vector3 pos)
        {
            var hex = HexUtils.FromWorld(pos);
            return hex.ToWorld(0f);
        }
        #endregion
    }
}
