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
        #region enum
        public enum SubregionDebugType
        {
            None,
            Color,
            Name,
            ColorAndName
        }
        #endregion
        #region Var
        public List<GridCell> Cells;
        [Header("Subregion Debug")]
        public SubregionDebugType ShowSubregionDebug = SubregionDebugType.None;
        #endregion

        #region MonoB
        protected override void Awake()
        {
            base.Awake();

            if (!Application.isEditor) {
                ShowSubregionDebug = SubregionDebugType.None;
            }
        }

        private void Start()
        {
            Cells.Clear();
            InitCells();
        }

        private void OnDrawGizmos()
        {
            if (ShowSubregionDebug != SubregionDebugType.None) {
                DrawSubregionInfo();
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
            var cell = GetCellByPosition(pos);
            if (cell != null) {
                return cell.Occupied;
            }

            return true;
        }

        public bool IsOutOfMap(Vector3 pos)
        {
            var cell = GetCellByPosition(pos);

            return cell == null;
        }

        public void OccupyCellByPosition(Vector3 pos, ProjectCategories category)
        {
            var cell = GetCellByPosition(pos);
            if (cell != null) {
                cell.Occupied = true;
                cell.SetupCategory(category);
            }
        }

        public GridCell GetCellByPosition(Vector3 pos)
        {
            Vector3 hexPos = RetrieveHexPos(pos);

            return Cells.Find(x => x.HexPosition == hexPos);
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

        public List<GridCell> GetAllSubregionCellsByPos(Vector3 pos)
        {
            List<GridCell> subregionCells = new List<GridCell>();
            var originCell = GetCellByPosition(pos);
            if (originCell != null) {
                subregionCells = Cells.FindAll(x => x.MySubregion == originCell.MySubregion);
            }

            return subregionCells;
        }

        public List<GridCell> GetNeighboursByPos(Vector3 pos)
        {
            List<GridCell> subregionCells = new List<GridCell>();
            var hex = HexUtils.FromWorld(pos);
            foreach (var neighbour in hex.Neighbours()) {
                var neighbourPos = neighbour.ToWorld();
                if (!IsOutOfMap(neighbourPos)) {
                    subregionCells.Add(GetCellByPosition(neighbourPos));
                }
            }

            return subregionCells;
        }

        public HashSet<GridCell> GetNeighboursOfPlaceable(List<PlaceableCell> cells)
        {
            HashSet<GridCell> neighboursSet = new HashSet<GridCell>();
            HashSet<GridCell> cellsToExclude = new HashSet<GridCell>();
            foreach (var cell in cells) {
                cellsToExclude.Add(GetCellByPosition(cell.HexPosition));
                neighboursSet.UnionWith(GetNeighboursByPos(cell.HexPosition));
            }

            neighboursSet.ExceptWith(cellsToExclude);

            return neighboursSet;
        }

        private Vector3 RetrieveHexPos(Vector3 pos)
        {
            var hex = HexUtils.FromWorld(pos);

            return hex.ToWorld(0f);
        }

        private void DrawSubregionInfo()
        {
            foreach (var cell in Cells) {
                bool drawText = false;
                bool drawColor = false;
                switch (ShowSubregionDebug) {
                    case SubregionDebugType.Color:
                        drawColor = true;
                        break;

                    case SubregionDebugType.Name:
                        drawText = true;
                        break;

                    case SubregionDebugType.ColorAndName:
                        drawColor = true;
                        drawText = true;
                        break;

                    default:
                    case SubregionDebugType.None:
                        break;
                }

                var pos = cell.transform.position;
                var subregionColor = GameplayConfig.I.GetSubregionColorByEnum(cell.MySubregion);
                if (drawText) {
                    string subregionName = GameplayConfig.I.GetSubregionNameByEnum(cell.MySubregion);
                    subregionName = subregionName.Replace(" ", "\n");
                    var style = new GUIStyle();
                    style.normal.textColor = subregionColor;
                    style.fontSize = GameplayConfig.I.LabelDebugFontSize;
                    Vector3 labelPos = pos + GameplayConfig.I.LabelDebugOffset;
                    Handles.Label(labelPos, subregionName, style);
                }

                if (drawColor) {
                    Gizmos.color = subregionColor;
                    Vector3 gizmosPos = pos + Vector3.up * 0.2f;
                    Gizmos.DrawSphere(gizmosPos, 0.3f);
                }
            }
        }
        #endregion
    }
}
