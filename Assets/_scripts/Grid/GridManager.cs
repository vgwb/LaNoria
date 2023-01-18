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

            ShowSubregionDebug = SubregionDebugType.None;
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
                    Gizmos.DrawSphere(pos, 0.3f);
                }
            }
        }
        #endregion
    }
}
