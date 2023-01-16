using vgwb.framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace vgwb.lanoria
{
    public class GridManager : SingletonMonoBehaviour<GridManager>
    {
        #region Var
        public List<GridCell> Cells;
        #endregion

        #region MonoB
        protected override void Awake()
        {
            base.Awake();

            if (Cells.Count == 0) {
                InitCells();
            }
        }

        void Update()
        {

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
                gridCell.Init(i == 0 ? true : false);  // the first one is Malaga
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

            return true; // outside of the map!!!
        }

        public bool IsOutOfMap(Vector3 pos)
        {
            Vector3 hexPos = RetrieveHexPos(pos);
            var cell = Cells.Find(x => x.HexPosition == hexPos);

            return cell == null;
        }

        public void SetCellAsOccupiedByPosition(Vector3 pos)
        {
            Vector3 hexPos = RetrieveHexPos(pos);
            var cell = Cells.Find(x => x.HexPosition == hexPos);
            if (cell != null) {
                cell.Occupied = true;
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
