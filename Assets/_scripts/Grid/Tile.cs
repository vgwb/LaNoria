using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuickOutline;

namespace vgwb.lanoria
{

    public struct TileLocation
    {
        public Hex Position;
        public HexDirection Direction;

        public TileLocation(Hex hexPosition, HexDirection hexDirection)
        {
            Position = hexPosition;
            Direction = hexDirection;
        }
    }

    public class Tile : MonoBehaviour
    {
        [HideInInspector]
        public ProjectData Project;
        public TileModel Model;
        public List<TileCell> Cells;
        public List<int> ShapePath;

        [Header("Ref")]
        public GameObject Pivot;
        public GameObject CellsContainer;
        public Outline OutlineHandler;

        public delegate void PlaceableEvent();
        public PlaceableEvent OnSelectMe;
        public PlaceableEvent OnReleaseMe;
        public PlaceableEvent OnRotateMe;
        public PlaceableEvent OnValidPositionChange;
        public PlaceableEvent OnStopUsingMe;
        public PlaceableEvent OnHexPosChange;

        public LeanDragCamera LeanCameraComp;
        public LeanSelectableByFinger LeanSelectableComp;
        public LeanFingerTap LeanFingerTapComp;

        [Header("Debug")]
        public bool ShowPivot = false;

        private bool isUsed;
        private bool isMoving;
        private Vector3 prevPos;

        public bool IsValidPosition { get; private set; }
        public int Size => Cells.Count;

        public Vector3 HexPos
        {
            get { // the first tile is considered the object center
                if (Size > 0) {
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
            if (isUsed) {
                SetY(); // need to force the Y because of conflict with CW
            }

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
            isMoving = false;
            SoundManager.I.PlaySfx(AudioEnum.tile_released);
            Vector3 newPos = HexPos;
            if (IsCompletelyOutOfMap()) {
                var grid = GridManager.I;
                newPos = grid.ClosestBorderPoint(newPos);
            }

            transform.position = newPos;
            SetY();

            LowerDownTilesHeight(false);
            CameraManager.I.EnableCameraMove(true);

            if (OnReleaseMe != null) {
                OnReleaseMe();
            }
        }

        public void OnSelect()
        {
            //SoundManager.I.PlaySfx(AudioEnum.tile_released);
            isMoving = true;
            RestoreOutline();
            RiseUpTilesHeight();
            OnSelectMe?.Invoke();
        }

        /// <summary>
        /// Fired when the player tap on the project object.
        /// </summary>
        public void OnTap(LeanFinger finger)
        {
            var screenPos = finger.ScreenPosition;
            RaycastHit hit;
            Ray ray = CameraManager.I.MyCamera.ScreenPointToRay(screenPos);

            if (Physics.Raycast(ray, out hit)) {
                Transform objectHit = hit.transform;
                SoundManager.I.PlaySfx(AudioEnum.tile_rotate);
                Rotate(objectHit);
            }
        }

        private void Rotate(Transform turnPoint)
        {
            var ParentInPivot = new GameObject(); // create an empty object
            // put it in the pivot position
            ParentInPivot.transform.position = turnPoint.position;
            // the new object is now the parent...
            transform.parent = ParentInPivot.transform;
            var rot = new Vector3(0.0f, 60.0f, 0.0f); // rotate it!
            ParentInPivot.transform.Rotate(rot);
            transform.parent = null; // restore the parent object to null
            Destroy(ParentInPivot);

            if (OnRotateMe != null) {
                OnRotateMe();
            }
        }

        public void OnTileConfirmed()
        {
            EnableLeanComponents(false);
            EnablePivot(false); // deactivate pivot and colliders
            DisableCategoryIcons();

            OccupyGrid();
            DisableOutline();
            StopUsingMe();
            LowerDownTilesHeight(true);
            TileManager.I.AddTilesToPlaced(Cells);
        }

        public void EnablePivot(bool enable)
        {
            Pivot?.SetActive(enable);
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
                grid.OccupyCellByPosition(tilePos);
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
                var tileCell = child.GetComponent<TileCell>();
                if (tileCell == null) {
                    tileCell = child.AddComponent<TileCell>();
                }
                tileCell.Init();
                Cells.Add(tileCell);
            }
        }

        public void RiseUpTilesHeight()
        {
            if (CellsContainer != null) {
                Vector3 delta = Vector3.up * GameplayConfig.I.DragYOffset;
                CellsContainer.transform.localPosition = delta;
            }
        }

        public void LowerDownTilesHeight(bool toZero)
        {
            if (CellsContainer != null) {
                Vector3 delta = toZero ? Vector3.zero : Vector3.up * GameplayConfig.I.OnConfirmYOffset;
                CellsContainer.transform.localPosition = delta;
            }
        }

        public void SetupCellsColor(ProjectData projectData)
        {
            if (projectData.Sequence.Length != Size) {
                Debug.LogError("Placeable - SetupCellsColor(): error in sequence definition!");
                return;
            }

            for (int i = 0; i < projectData.Sequence.Length; i++) {
                if (Cells[i] != null) {
                    Cells[i].SetupCategory(projectData.Sequence[i]);
                    Cells[i].ApplyColor();
                    Cells[i].SetLabel("");
                }
            }
        }

        private void DisableCategoryIcons()
        {
            foreach (var cell in Cells) {
                cell.RemoveCategory();
            }
        }

        public List<Vector3> GetCellsHexPositions()
        {
            var hexPositions = new List<Vector3>();
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
            enabled = false;
        }

        public void SetupForDrag()
        {
            isUsed = true;
            var container = BoardManager.I.ProjectsContainer;
            transform.parent = container.transform;
        }

        public void ManualSetPosition(Vector3 hexPos, HexDirection dir)
        {
            transform.position = hexPos;
            transform.SetPositionAndRotation(hexPos, Quaternion.Euler(GetHexRotation(dir)));
            var container = BoardManager.I.ProjectsContainer;
            transform.parent = container.transform;
        }

        public List<ProjectCategories> GetTileCategoriesInArea(AreaId area)
        {
            var result = new List<ProjectCategories>();
            foreach (var cell in Cells) {
                if (!result.Contains(cell.Category)) {
                    var gridCell = GridManager.I.GetCellByPosition(cell.HexPosition);
                    bool isPieceInArea = gridCell.Area == area;
                    if (isPieceInArea) {
                        result.Add(cell.Category);
                    }
                }
            }

            return result;
        }

        public bool IsMoving()
        {
            return isMoving;
        }

        private Vector3 GetHexRotation(HexDirection dir)
        {
            //            Debug.Log("HexDirection + " + dir + " int:" + (int)dir);
            dir--;
            return new Vector3(0, 60 * (int)dir, 0);
        }

        private void SetY()
        {
            var pos = transform.position;
            pos.y = BoardManager.I.GetProjectsHeight();
            transform.position = pos;
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
            if (validPosition != IsValidPosition) {
                IsValidPosition = validPosition; // update the value
                OnValidPositionChange?.Invoke(); // notify the new validity
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
                OnHexPosChange?.Invoke();
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
            isUsed = false;
            OnStopUsingMe?.Invoke();
        }

        private void SetupOutline()
        {
            OutlineHandler.OutlineColor = GameplayConfig.I.MovingColor;
            OutlineHandler.OutlineWidth = GameplayConfig.I.OutlineWidth;
            OutlineHandler.OutlineMode = GameplayConfig.I.PlaceableOutlineMode;
        }
    }
}
