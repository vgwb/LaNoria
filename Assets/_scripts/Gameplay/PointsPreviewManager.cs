using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using vgwb.framework;

namespace vgwb.lanoria
{
    public class PointsPreviewManager : SingletonMonoBehaviour<PointsPreviewManager>
    {
        public bool UsePreview = true;
        private List<CellScoreToDisplay> cellsScore;

        private void Start()
        {
            cellsScore = new List<CellScoreToDisplay>();
        }

        public void PreviewScore(Tile placeable)
        {
            CleanPreview();
            if (!UsePreview) {
                return;
            }

            cellsScore = ScoreManager.I.CalculateSynergy(placeable);
            foreach (var cellScore in cellsScore) {
                DrawPreviewScore(cellScore.Cell, cellScore.Score);
            }
        }

        public void CleanPreview()
        {
            foreach (var cellScore in cellsScore) {
                cellScore.Cell.SetLabel("");
            }

            cellsScore.Clear();
        }

        private void DrawPreviewScore(TileCell cell, float score)
        {
            if (cell != null) {
                cell.SetLabel(score.ToString());
            }
        }
    }
}
