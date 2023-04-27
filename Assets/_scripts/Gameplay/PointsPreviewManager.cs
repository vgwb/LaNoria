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
        [SerializeField] private List<AreaId> highlightedAreas;
        private Dictionary<AreaId, List<CellEfx>> areaEfx;
        private List<GameObject> bridges;
        private List<CellEfx> efxConfirmed;

        private void Start()
        {
            cellsScore = new List<CellScoreToDisplay>();
            highlightedAreas = new List<AreaId>();
            areaEfx = new Dictionary<AreaId, List<CellEfx>>();
            bridges = new List<GameObject>();
            efxConfirmed = new List<CellEfx>();
        }

        public void PreviewScore(Tile placeable)
        {
            CleanSynergyPreview();            
            if (!UsePreview) {
                return;
            }

            DisplaySynergyPreview(placeable);
            DisplayTransversalityPreview(placeable);
        }

        public void CleanPreview()
        {
            CleanSynergyPreview();
            //CleanTransversalityPreview();
        }

        public void CleanInvalidPosition()
        {
            CleanSynergyPreview();
            CleanTransversalityPreview();
        }

        public void ConfirmEfx()
        {
            foreach (var area in highlightedAreas) {
                if (areaEfx.ContainsKey(area)) {
                    efxConfirmed.AddRange(areaEfx[area]);
                    foreach (var efx in areaEfx[area]) {
                        efx.StopBlink();
                    }
                    areaEfx[area].Clear();
                }
            }
        }

        public void ResetAll()
        {
            highlightedAreas.Clear();
            var areaKeys = areaEfx.Keys;
            foreach (var area in areaKeys) {
                CleanEfxArea(area);
            }

            foreach (var efx in efxConfirmed) {
                Destroy(efx.gameObject);
            }
            efxConfirmed.Clear();
        }

        private void DrawPreviewScore(TileCell cell, List<TileCell> adjacents)
        {
            if (cell != null) {
                foreach (var adjacent in adjacents) {
                    var bridge = cell.SpawnBridgeBetween(cell.HexPosition, adjacent.HexPosition);
                    bridges.Add(bridge);
                }
            }
        }

        private void DisplaySynergyPreview(Tile placeable)
        {
            cellsScore = ScoreManager.I.CalculateAdjacencyBonus(placeable);
            bool displayTutorial = AppManager.I.AppSettings.TutorialEnabled;
            if (displayTutorial && ScoreManager.I.FindSinergy()) {
                if (TutorialManager.I.IsExplanationCompleted(TutorialStep.PointsSynergy)) {
                    TutorialManager.I.CloseTutorialStep(TutorialStep.PointsArea, 0, false);
                }
                TutorialManager.I.ShowTutorialStep(TutorialStep.PointsSynergy, 0);
            }

            foreach (var cellScore in cellsScore) {
                DrawPreviewScore(cellScore.Cell, cellScore.Adjacents);
            }
        }

        private void CleanSynergyPreview()
        {
            foreach (var cellScore in cellsScore) {
                //cellScore.Cell.SetLabel("");
                //cellScore.Cell.EnableBridge(false);
            }

            foreach (var bridge in bridges) {
                Destroy(bridge);
            }

            bridges.Clear();
            cellsScore.Clear();
        }

        private IEnumerable<AreaId> GetAreaToHighligth(Tile placeable)
        {
            var newAreas = ScoreManager.I.CalculateAreaBonus(placeable);
            List<AreaId> areaToHighlit = new List<AreaId>();
            foreach (var highArea in highlightedAreas) {
                WallManager.I.ResetWallArea(highArea);
                CleanEfxArea(highArea);
            }

            highlightedAreas.Clear();

            foreach (var area in newAreas) {
                areaToHighlit.Add(area);
                highlightedAreas.Add(area);
            }

            return areaToHighlit;
        }

        private void DisplayTransversalityPreview(Tile placeable)
        {
            var areaToHighlight = GetAreaToHighligth(placeable);
            bool displayTutorial = AppManager.I.AppSettings.TutorialEnabled;
            if (displayTutorial && highlightedAreas.Count > 0) {
                TutorialManager.I.CloseTutorialStep(TutorialStep.PointsSynergy, 0, false);
                TutorialManager.I.ShowTutorialStep(TutorialStep.PointsArea, 0);
            }

            foreach (var area in areaToHighlight) {
                WallManager.I.HighlightWallArea(area);
                var cells = GridManager.I.GetCellsByArea(area);
                List<CellEfx> efxList = new List<CellEfx>();
                foreach (var cell in cells) {
                    var pos = Vector3.up;
                    pos.y += GameplayConfig.I.CellEfxHeight;
                    var cellEfx = Instantiate(CellAreaEfx, cell.transform);
                    cellEfx.transform.localPosition += pos;
                    var efxComp = cellEfx.GetComponent<CellEfx>();
                    Color color = GameplayConfig.I.HighlightMat.GetColor("_BaseColor");
                    color.a = GameplayConfig.I.CellEfxAlpha;
                    efxComp.SetColor(color);
                    efxList.Add(efxComp);
                }
                areaEfx[area] = efxList;
            }
        }

        private void CleanEfxArea(AreaId area)
        {
            if (areaEfx.ContainsKey(area)) {
                foreach (var efx in areaEfx[area]) {
                    Destroy(efx.gameObject);
                }
                areaEfx[area].Clear();
            }
        }

        private void CleanTransversalityPreview()
        {
            foreach (var area in highlightedAreas) {
                WallManager.I.ResetWallArea(area);
            }

            highlightedAreas.Clear();

            var areaKeys = areaEfx.Keys;
            foreach (var area in areaKeys) {
                CleanEfxArea(area);
            }
            areaEfx.Clear();
        }
    }
}
