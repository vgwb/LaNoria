using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb
{
    public class Project : MonoBehaviour
    {
        #region Var
        public bool ShowPivot = false;
        public GameObject Pivot;
        public List<HexSnap> Tiles;
        [Header("Lean components")]
        public LeanDragCamera LeanCameraComp;
        public LeanSelectableByFinger LeanSelectableComp;
        public LeanFingerTap LeanFingerTapComp;
        #endregion

        #region MonoB
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
            if (Tiles.Count > 0) { // the first tile is considered the object center
                Vector3 newPos = Tiles[0].hex.ToWorld(0f);
                transform.position = newPos;
            }
            CheckOverlap();
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

        public void OccupatyGrid()
        {
            var grid = FindObjectOfType<GridManager>(); // TODO: remove this!
            foreach (var tile in Tiles) {
                Vector3 tilePos = tile.transform.position;
                grid.SetCellAsOccupiedByPosition(tilePos);
            }
        }

        private void CheckOverlap()
        {
            var grid = FindObjectOfType<GridManager>(); // TODO: remove this!
            foreach (var tile in Tiles) {
                Vector3 tilePos = tile.transform.position;
                bool posOccupied = grid.IsCellOccupiedByPos(tilePos);
                if (posOccupied) {
                    Debug.Log("Occupied!!!");
                    break;
                }
            }
        }
        #endregion
    }
}
