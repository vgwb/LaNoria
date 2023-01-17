using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb.lanoria
{
    public class Placeable : MonoBehaviour
    {
        #region Var
        public bool ShowPivot = false;
        public GameObject Pivot;
        public GameObject TilesContainer;
        public List<PlaceableCell> Tiles;
        public Outline OutlineHandler;
        [Header("Lean components")]
        public LeanDragCamera LeanCameraComp;
        public LeanSelectableByFinger LeanSelectableComp;
        public LeanFingerTap LeanFingerTapComp;
        public delegate void PlaceableEvent();
        public PlaceableEvent OnValidPositionChange;
        public PlaceableEvent OnStopUsingMe;

        private bool isValidPosition;
        #endregion

        #region Attributes
        public bool IsValidPosition
        {
            get {
                return isValidPosition;
            }
        }
        #endregion


        #region MonoB

        private void Awake()
        {
            SetupOutline();
        }

        private void Update()
        {
            CheckValidPosition();
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
        #endregion

        #region Functions

        /// <summary>
        /// Fired when the player release the project into the board.
        /// Snap the object into a valid position of the hexagonal field.
        /// </summary>
        public void OnRelease()
        {
            // the first tile is considered the object center
            if (Tiles.Count > 0) {
                Vector3 newPos = Tiles[0].HexPosition;

                if (IsCompletelyOutOfMap()) {
                    var grid = GridManager.I;
                    newPos = grid.ClosestBorderPoint(newPos);
                }

                transform.position = newPos;
            }

            LowerDownTilesHeight();
            CameraManager.I.EnableRotationWithFingers(true);
        }

        public void OnSelect()
        {
            RestoreOutline();
            CameraManager.I.EnableRotationWithFingers(false);
            RiseUpTilesHeight();
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
            foreach (var tile in Tiles) {
                Vector3 tilePos = tile.transform.position;
                grid.SetCellAsOccupiedByPosition(tilePos);
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
            Tiles.Clear();

            int childs = TilesContainer.transform.childCount;
            for (int i = 0; i < childs; i++) {
                var child = TilesContainer.transform.GetChild(i).gameObject;
                string childName = child.name.ToLower();
                var tile = child.GetComponent<PlaceableCell>();
                if (tile == null) {
                    tile = child.AddComponent<PlaceableCell>();
                }
                tile.Init();
                Tiles.Add(tile);
            }
        }

        public void RiseUpTilesHeight()
        {
            if (TilesContainer != null) {
                Vector3 delta = Vector3.up * GameplayConfig.I.DragYOffset;
                TilesContainer.transform.position += delta;
            }
        }

        public void LowerDownTilesHeight()
        {
            if (TilesContainer != null) {
                Vector3 delta = Vector3.up * GameplayConfig.I.DragYOffset;
                TilesContainer.transform.position -= delta;
            }
        }

        /// <summary>
        /// Check if the placeable is in a valid position.
        /// </summary>
        private void CheckValidPosition()
        {
            var grid = GridManager.I;
            bool validPosition = true;
            foreach (var tile in Tiles) {
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

        private bool IsCompletelyOutOfMap()
        {
            bool result = true;
            foreach (var tile in Tiles) {
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
        #endregion
    }
}
