using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb.lanoria
{
    public class WallManager : MonoBehaviour
    {
        public List<Wall> Walls;

        private void Start()
        {
            PopulateWalls();
        }

        public void PopulateWalls()
        {
            Walls.Clear();
            var walls = GetComponentsInChildren<Wall>();
            foreach (var wall in walls) {
                wall.GetWallMesh();
                Vector3 forward = wall.GetWallForward() * 1;
                Vector3 back = wall.GetWallForward() * -1;
                Vector3 pos1 = wall.GetWallPosition() + forward;
                Vector3 pos2 = wall.GetWallPosition() + back;
                var grid = GridManager.I;
                var forwardCell = grid.GetCellByPosition(pos1);
                var backCell = grid.GetCellByPosition(pos2);
                wall.CleanAreas();
                wall.AddArea(forwardCell.Area);
                wall.AddArea(backCell.Area);

                Walls.Add(wall);
            }
        }

        public List<Wall> GetWallsByArea(AreaId area)
        {
            return Walls.FindAll(x => x.Areas.Contains(area));
        }
    } 
}
