using vgwb.framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace vgwb.lanoria
{
    public class GridManager : SingletonMonoBehaviour<GridManager>
    {
        //[HideInInspector]
        public List<GridCell> Cells;

        Hex currentHex = new Hex(0, 0);

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            InitCells();
        }

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

        public void OccupyCellByPosition(Vector3 pos)
        {
            var cell = GetCellByPosition(pos);
            if (cell != null) {
                cell.Occupied = true;
            }
        }

        public GridCell GetCellByHex(Hex hex)
        {
            return Cells.Find(x => (x.hex.q == hex.q) && (x.hex.r == hex.r));
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

        public List<GridCell> GetAreaCellsFromSinglePos(Vector3 pos)
        {
            var cells = new List<GridCell>();
            var originCell = GetCellByPosition(pos);
            if (originCell != null) {
                cells = Cells.FindAll(x => x.Area == originCell.Area);
            }
            return cells;
        }

        public List<GridCell> GetNeighboursByPos(Vector3 pos)
        {
            var cells = new List<GridCell>();
            var hex = Hex.FromWorld(pos);
            foreach (var neighbour in hex.Neighbours()) {
                var neighbourPos = neighbour.ToWorld();
                if (!IsOutOfMap(neighbourPos)) {
                    cells.Add(GetCellByPosition(neighbourPos));
                }
            }
            return cells;
        }

        public HashSet<GridCell> GetNeighbours(List<TileCell> cells)
        {
            var neighboursSet = new HashSet<GridCell>();
            var cellsToExclude = new HashSet<GridCell>();
            foreach (var cell in cells) {
                cellsToExclude.Add(GetCellByPosition(cell.HexPosition));
                neighboursSet.UnionWith(GetNeighboursByPos(cell.HexPosition));
            }
            neighboursSet.ExceptWith(cellsToExclude);
            return neighboursSet;
        }

        public bool CanProjectBePlaced(Tile tile)
        {
            var foundLocation = new TileLocation();
            return GetGoodTileLocation(tile.ShapePath, out foundLocation);
        }

        public bool GetGoodTileLocation(List<int> shapePath, out TileLocation foundLocation)
        {
            foundLocation = new TileLocation();
            foreach (var cell in Cells) {
                if (cell.Occupied) {
                    continue;
                }

                foreach (HexDirection direction in Enum.GetValues(typeof(HexDirection))) {
                    if (canShapeBePlacedHere(cell.hex, shapePath, direction)) {
                        foundLocation.Position = cell.hex;
                        foundLocation.Direction = direction;
                        return true;
                    }
                }
            }
            return false;
        }

        public List<GridCell> GetCellsByArea(AreaId area)
        {
            var cells = new List<GridCell>();
            cells = Cells.FindAll(x => x.Area == area);

            return cells;
        }

        private Vector3 RetrieveHexPos(Vector3 pos)
        {
            return Hex.FromWorld(pos).ToWorld(0f);
        }

        private bool canShapeBePlacedHere(Hex startingHex, List<int> shape, HexDirection direction)
        {
            var neightbour = startingHex.GetNeighbour(direction);
            if (shape.Count == 1) {
                // size = 2
                if (GetCellByHex(neightbour) != null && !GetCellByHex(neightbour).Occupied) {
                    return true;
                }
            } else if (shape.Count == 2) {
                // size = 3
                var neightbour2 = neightbour.GetNeighbour(HexDirectionExtensions.GetByDelta(direction, shape[1]));
                if (GetCellByHex(neightbour) != null && !GetCellByHex(neightbour).Occupied
                && GetCellByHex(neightbour2) != null && !GetCellByHex(neightbour2).Occupied) {
                    return true;
                }
            } else if (shape.Count == 3) {
                //                Debug.Log("Shape size 4");
                // size = 4
                var neightbour2 = neightbour.GetNeighbour(HexDirectionExtensions.GetByDelta(direction, shape[1]));
                var neightbour3 = neightbour2.GetNeighbour(HexDirectionExtensions.GetByDelta(direction, shape[2]));
                // Debug.Log("startingHex: " + startingHex);
                // Debug.Log("neightbour1: " + neightbour);
                // Debug.Log("neightbour2: " + neightbour2);
                // Debug.Log("neightbour3: " + neightbour3);
                if (GetCellByHex(neightbour) != null && !GetCellByHex(neightbour).Occupied
                && GetCellByHex(neightbour2) != null && !GetCellByHex(neightbour2).Occupied
                && GetCellByHex(neightbour3) != null && !GetCellByHex(neightbour3).Occupied) {
                    return true;
                }
            }
            return false;
        }

        #region Debug and Editor Methods
        public void DebugSelectCell(int direction)
        {
            if (GetCellByHex(currentHex.GetNeighbour(direction)) is not null) {
                GetCellByHex(currentHex).Highlight(false);
                currentHex = currentHex.GetNeighbour(direction);
                GetCellByHex(currentHex).Highlight(true);
            }
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            if (AppConfig.I.ShowSubregionDebug != SubregionDebugType.None) {
                DrawSubregionInfo();
            }
        }

        private void DrawSubregionInfo()
        {
            var subregionWritten = new List<AreaId>();
            foreach (var cell in Cells) {
                bool drawText = false;
                bool drawColor = false;
                switch (AppConfig.I.ShowSubregionDebug) {
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
                var subregionColor = GameData.I.Areas.GetSubregionColorByEnum(cell.Area);
                if (drawText && !subregionWritten.Contains(cell.Area)) {
                    string subregionName = GameData.I.Areas.GetSubregionNameByEnum(cell.Area);
                    subregionName = subregionName.Replace(" ", "\n");
                    var style = new GUIStyle();
                    style.normal.textColor = subregionColor;
                    style.fontSize = GameplayConfig.I.LabelDebugFontSize;
                    Vector3 labelPos = pos + GameplayConfig.I.LabelDebugOffset;
                    Handles.Label(labelPos, subregionName, style);
                    subregionWritten.Add(cell.Area);
                }

                if (drawColor) {
                    Gizmos.color = subregionColor;
                    Vector3 gizmosPos = pos + Vector3.up * 0.2f;
                    Gizmos.DrawSphere(gizmosPos, 0.3f);
                }
            }
        }
#endif
        #endregion
    }
}
