using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using vgwb.framework;

namespace vgwb.lanoria
{
    public class PointsPreviewManager : SingletonMonoBehaviour<PointsPreviewManager>
    {
        public bool UsePreview = true;
        public GameObject CellAreaEfx;
        private List<CellScoreToDisplay> cellsScore;
        private List<AreaId> highligtAreas;
        private List<CellEfx> areaEfx;

        private void Start()
        {
            cellsScore = new List<CellScoreToDisplay>();
            highligtAreas = new List<AreaId>();
            areaEfx = new List<CellEfx>();
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
            cellsScore = ScoreManager.I.CalculateAdjacencyBonus(placeable);
            if (ScoreManager.I.FindSinergy()) {
                TutorialManager.I.ShowTutorialStep(TutorialStep.Points, 0);
            }

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
            highligtAreas = ScoreManager.I.CalculateAreaBonus(placeable);
            foreach (var area in highligtAreas) {
                WallManager.I.HighlightWallArea(area);
                var cells = GridManager.I.GetCellsByArea(area);
                foreach (var cell in cells) {
                    var pos = Vector3.up;
                    pos.y += GameplayConfig.I.CellEfxHeight;
                    var cellEfx = Instantiate(CellAreaEfx, cell.transform);
                    cellEfx.transform.localPosition += pos;
                    var efxComp = cellEfx.GetComponent<CellEfx>();
                    Color color = GameplayConfig.I.HighlightMat.GetColor("_BaseColor");
                    color.a = GameplayConfig.I.CellEfxAlpha;
                    efxComp.SetColor(color);
                    areaEfx.Add(efxComp);
                }
            }
        }

        private void CleanTransversalityPreview()
        {
            foreach (var area in highligtAreas) {
                WallManager.I.ResetWallArea(area);
            }

            highligtAreas.Clear();

            foreach (var efx in areaEfx) {
                Destroy(efx.gameObject);
            }
            areaEfx.Clear();
        }
    }
}
