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
        private List<AreaId> highligtAreas;

        private void Start()
        {
            cellsScore = new List<CellScoreToDisplay>();
            highligtAreas = new List<AreaId>();
        }

        public void PreviewScore(Tile placeable)
        {
            CleanPreview();
            if (!UsePreview) {
                return;
            }

            DisplaySynergyPreview(placeable);
            DisplayTransversalityPreview(placeable);
        }

        public void CleanPreview()
        {
            CleanSynergyPreview();
            CleanTransversalityPreview();
        }

        private void DrawPreviewScore(TileCell cell, float score)
        {
            if (cell != null) {
                cell.SetLabel(score.ToString());
            }
        }

        private void DisplaySynergyPreview(Tile placeable)
        {
            cellsScore = ScoreManager.I.CalculateSynergy(placeable);
            foreach (var cellScore in cellsScore) {
                DrawPreviewScore(cellScore.Cell, cellScore.Score);
            }
        }

        private void CleanSynergyPreview()
        {
            foreach (var cellScore in cellsScore) {
                cellScore.Cell.SetLabel("");
            }

            cellsScore.Clear();
        }

        private void DisplayTransversalityPreview(Tile placeable)
        {
            highligtAreas = ScoreManager.I.CalculateTransversality(placeable);
            foreach (var area in highligtAreas) {
                WallManager.I.HighlightWallArea(area);
            }
        }

        private void CleanTransversalityPreview()
        {
            foreach (var area in highligtAreas) {
                WallManager.I.ResetWallArea(area);
            }

            highligtAreas.Clear();
        }
    }
}
