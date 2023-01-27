using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb.lanoria
{
    public class Tile : MonoBehaviour
    {
        public TileModel Model;
        public bool ShowPivot = false;
        public GameObject Pivot;
        public GameObject CellsContainer;
        public List<TileCell> Cells;
        public Outline OutlineHandler;

        public delegate void PlaceableEvent();
        public PlaceableEvent OnSelectMe;
        public PlaceableEvent OnValidPositionChange;
        public PlaceableEvent OnStopUsingMe;
        public PlaceableEvent OnHexPosChange;

        [Header("Lean components")]
        public LeanDragCamera LeanCameraComp;
        public LeanSelectableByFinger LeanSelectableComp;
        public LeanFingerTap LeanFingerTapComp;

        private bool isValidPosition;
        private Vector3 prevPos;

        public bool IsValidPosition
        {
            get {
                return isValidPosition;
            }
        }

        public int CellsNum
        {
            get { return Cells.Count; }
        }

        public Vector3 HexPos
        {
            get { // the first tile is considered the object center
                if (Cells.Count > 0) {
                    return Cells[0].HexPosition;
                }

                return transform.position;
            }
        }

        private void Awake()
        {
            prevPos = HexPos;
            SetupOutline();
        }

        private void Update()
        {
            CheckValidPosition();
            CheckHexPosChange();
        }

        private void OnDestroy()
        {
            StopUsingMe();
        }

        private void OnDrawGizmos()
        {
            if (ShowPivot) {
                Gizmos.color = Color.magenta;
                Gizmos.DrawSphere(Pivot.transform.position, 0.2f);
            }
        }

        /// <summary>
        /// Fired when the player release the project into the board.
        /// Snap the object into a valid position of the hexagonal field.
        /// </summary>
        public void OnRelease()
        {
            Vector3 newPos = HexPos;
            if (IsCompletelyOutOfMap()) {
                var grid = GridManager.I;
                newPos = grid.ClosestBorderPoint(newPos);
            }

            transform.position = newPos;

            LowerDownTilesHeight();
            CameraManager.I.EnableRotationWithFingers(true);
        }

        public void OnSelect()
        {
            RestoreOutline();
            RiseUpTilesHeight();
            if (OnSelectMe != null) {
                OnSelectMe();
            }
        }

        /// <summary>
        /// Fired when the player tap on the project object.
        /// </summary>
        public void OnTap()
        {
            var ParentInPivot = new GameObject(); // create an empty object
            // put it in the pivot position
            ParentInPivot.transform.position = Pivot.transform.position;
            // the new object is now the parent...
            transform.parent = ParentInPivot.transform;
            Vector3 rot = new Vector3(0.0f, 60.0f, 0.0f); // rotate it!
            ParentInPivot.transform.Rotate(rot);
            transform.parent = null; // restore the parent object to null
            Destroy(ParentInPivot);
        }

        public void OnProjectConfirmed()
        {
            EnableLeanComponents(false);
            OccupyGrid();
            DisableOutline();
            StopUsingMe();
        }

        public void EnablePivot(bool enable)
        {
            if (Pivot != null) {
                Pivot.SetActive(enable);
            }
        }

        public void EnableLeanComponents(bool enable)
        {
            if (LeanCameraComp != null) {
                LeanCameraComp.enabled = enable;
            }

            if (LeanFingerTapComp != null) {
                LeanFingerTapComp.enabled = enable;
            }

            if (LeanSelectableComp != null) {
                LeanSelectableComp.enabled = enable;
            }
        }

        public void OccupyGrid()
        {
            var grid = GridManager.I;
            foreach (var tile in Cells) {
                Vector3 tilePos = tile.transform.position;
                grid.OccupyCellByPosition(tilePos, tile.Category);
            }
        }

        /// <summary>
        /// Used from editor to init the placeable.
        /// </summary>
        public void InitPlaceable()
        {
            LeanCameraComp = GetComponent<LeanDragCamera>();
            LeanSelectableComp = GetComponent<LeanSelectableByFinger>();
            LeanFingerTapComp = GetComponent<LeanFingerTap>();
            Cells.Clear();

            int childs = CellsContainer.transform.childCount;
            for (int i = 0; i < childs; i++) {
                var child = CellsContainer.transform.GetChild(i).gameObject;
                string childName = child.name.ToLower();
                var tile = child.GetComponent<TileCell>();
                if (tile == null) {
                    tile = child.AddComponent<TileCell>();
                }
                tile.Init();
                Cells.Add(tile);
            }
        }

        public void RiseUpTilesHeight()
        {
            if (CellsContainer != null) {
                Vector3 delta = Vector3.up * GameplayConfig.I.DragYOffset;
                CellsContainer.transform.position += delta;
            }
        }

        public void LowerDownTilesHeight()
        {
            if (CellsContainer != null) {
                Vector3 delta = Vector3.up * GameplayConfig.I.DragYOffset;
                CellsContainer.transform.position -= delta;
            }
        }

        public void SetupCellsColor(ProjectData projectData)
        {
            if (projectData.Sequence.Length != Cells.Count) {
                Debug.LogError("Placeable - SetupCellsColor(): error in sequence definition!");
                return;
            }

            for (int i = 0; i < projectData.Sequence.Length; i++) {
                if (Cells[i] != null) {
                    Cells[i].SetupCategory(projectData.Sequence[i]);
                    Cells[i].ApplyColor();
                }
            }
        }

        public List<Vector3> GetCellsHexPositions()
        {
            List<Vector3> hexPositions = new List<Vector3>();
            foreach (var cell in Cells) {
                hexPositions.Add(cell.HexPosition);
            }

            return hexPositions;
        }

        public void SetupForUI()
        {
            EnableLeanComponents(false);
            DisableOutline();
            SetupCellsForUICamera(false);

        }

        private void SetupCellsForUICamera(bool enable)
        {
            foreach (var cell in Cells) {
                //               cell.EnableHexComponent(enable);
                cell.SetupLayerForUICamera();
            }
        }

        /// <summary>
        /// Check if the placeable is in a valid position.
        /// </summary>
        private void CheckValidPosition()
        {
            var grid = GridManager.I;
            bool validPosition = true;
            foreach (var tile in Cells) {
                Vector3 tilePos = tile.transform.position;
                if (grid.IsCellOccupiedByPos(tilePos)) {
                    validPosition = false;
                    HandleInvalidPosition();
                    break;
                }
            }

            if (validPosition) {
                RestoreOutline();
            }

            // raise the event, position validity has changed!
            if (validPosition != isValidPosition) {
                isValidPosition = validPosition; // update the value
                if (OnValidPositionChange != null) {
                    OnValidPositionChange(); // notify the new validity
                }
            }
        }

        /// <summary>
        /// Warning: this method check for real if the first tile has changed position!
        /// </summary>
        private void CheckHexPosChange()
        {
            var actualPos = HexPos;
            if (actualPos != prevPos) {
                prevPos = actualPos;
                if (OnHexPosChange != null) {
                    OnHexPosChange();
                }
            }
        }

        private bool IsCompletelyOutOfMap()
        {
            bool result = true;
            foreach (var tile in Cells) {
                var grid = GridManager.I;
                Vector3 tilePos = tile.transform.position;
                if (!grid.IsOutOfMap(tilePos)) {
                    result = false;
                    break;
                }
            }

            return result;
        }

        private void HandleInvalidPosition()
        {
            var overlapColor = GameplayConfig.I.OverlapColor;
            ChangeOutlineColor(overlapColor);
        }

        private void RestoreOutline()
        {
            var movingColor = GameplayConfig.I.MovingColor;
            ChangeOutlineColor(movingColor);
        }

        private void ChangeOutlineColor(Color color)
        {
            OutlineHandler.OutlineColor = color;
        }

        private void DisableOutline()
        {
            OutlineHandler.enabled = false;
        }

        private void StopUsingMe()
        {
            if (OnStopUsingMe != null) {
                OnStopUsingMe();
            }
        }

        private void SetupOutline()
        {
            OutlineHandler.OutlineColor = GameplayConfig.I.MovingColor;
            OutlineHandler.OutlineWidth = GameplayConfig.I.OutlineWidth;
            OutlineHandler.OutlineMode = GameplayConfig.I.PlaceableOutlineMode;
        }
    }
}
