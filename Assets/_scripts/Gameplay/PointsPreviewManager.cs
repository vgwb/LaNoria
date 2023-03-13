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
        private List<AreaId> highlightAreas;
        private List<CellEfx> areaEfx;

        private void Start()
        {
            cellsScore = new List<CellScoreToDisplay>();
            highlightAreas = new List<AreaId>();
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
                //cell.EnableBridge(true);
            }
        }

        private void DisplaySynergyPreview(Tile placeable)
        {
            cellsScore = ScoreManager.I.CalculateAdjacencyBonus(placeable);
            if (ScoreManager.I.FindSinergy()) {
                if (TutorialManager.I.IsExplanationCompleted(TutorialStep.PointsSynergy)) {
                    TutorialManager.I.CloseTutorialStep(TutorialStep.PointsArea, 0, false);
                }
                TutorialManager.I.ShowTutorialStep(TutorialStep.PointsSynergy, 0);
            }

            foreach (var cellScore in cellsScore) {
                DrawPreviewScore(cellScore.Cell, cellScore.Score);
            }
        }

        private void CleanSynergyPreview()
        {
            foreach (var cellScore in cellsScore) {
                cellScore.Cell.SetLabel("");
                //cellScore.Cell.EnableBridge(false);
            }

            cellsScore.Clear();
        }

        private void DisplayTransversalityPreview(Tile placeable)
        {
            highlightAreas = ScoreManager.I.CalculateAreaBonus(placeable);
            if (highlightAreas.Count > 0) {
                TutorialManager.I.CloseTutorialStep(TutorialStep.PointsSynergy, 0, false);
                TutorialManager.I.ShowTutorialStep(TutorialStep.PointsArea, 0);
            }

            foreach (var area in highlightAreas) {
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
            foreach (var area in highlightAreas) {
                WallManager.I.ResetWallArea(area);
            }

            highlightAreas.Clear();

            foreach (var efx in areaEfx) {
                Destroy(efx.gameObject);
            }
            areaEfx.Clear();
        }
    }
}
