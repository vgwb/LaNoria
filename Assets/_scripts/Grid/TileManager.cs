using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using vgwb.framework;

namespace vgwb.lanoria
{
    public class TileManager : SingletonMonoBehaviour<TileManager>
    {
        #region Var
        private List<TileCell> placedTileCells;
        #endregion

        #region MonoB
        private void Start()
        {
            placedTileCells = new List<TileCell>();
        }
        #endregion

        #region Functions
        public void AddTilesToPlaced(List<TileCell> cells)
        {
            placedTileCells.AddRange(cells);
        }

        public TileCell GetPlacedTileByPosition(Vector3 hexPos)
        {
            return placedTileCells.Find(x => x.HexPosition == hexPos);
        }

        public void Clean()
        {
            placedTileCells.Clear();
        }
        #endregion
    }
}
