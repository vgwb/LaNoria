using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using vgwb.framework;

namespace vgwb.lanoria
{
    [System.Serializable]
    public class CellScoreToDisplay
    {
        public TileCell Cell;
        public float Score;

        public CellScoreToDisplay(TileCell cell, float score)
        {
            Cell = cell;
            Score = score;
        }
    }
    public class ScoreManager : SingletonMonoBehaviour<ScoreManager>
    {
        public int Score { get; private set; }

        [SerializeField] private int synergyScore;
        private List<AreaId> completedAreas;

        void Start()
        {
            completedAreas = new List<AreaId>();
            Score = 0;
        }

        public void UpdateScore(Tile tile)
        {
            int placementPoints = CalculateBasicPoints(tile);
            int transversalityPoints = CalculateTransversality(tile);
            int newPoints = placementPoints + synergyScore + transversalityPoints;
            Score += newPoints;
            Debug.Log("basic: " + placementPoints + " sinergy: " + synergyScore + " transversality: " + transversalityPoints);

            UI_manager.I.PanelGameplay.SetScoreUI(Score, newPoints);
        }

        public void ResetScore()
        {
            Score = 0;
            synergyScore = 0;
        }

        private int CalculateBasicPoints(Tile tile)
        {
            int points = 0;
            switch (tile.Size) {
                default:
                case 2:
                    points = GameplayConfig.I.Hex2Points;
                    break;
                case 3:
                    points = GameplayConfig.I.Hex3Points;
                    break;
                case 4:
                    points = GameplayConfig.I.Hex4Points;
                    break;
            }
            return points;
        }

        public List<CellScoreToDisplay> CalculateSynergy(Tile tile)
        {
            int resultingScore = 0;
            var positionsToExclude = tile.GetCellsHexPositions();
            var synergyCells = new List<CellScoreToDisplay>();

            foreach (var cell in tile.Cells) {
                var neighbours = GridManager.I.GetNeighboursByPos(cell.HexPosition);
                foreach (var neighbour in neighbours) {
                    if (positionsToExclude.Contains(neighbour.HexPosition)) {
                        continue;
                    }
                    var existingCell = TileManager.I.GetPlacedTileByPosition(neighbour.HexPosition);
                    if (existingCell != null) {
                        CellScoreToDisplay cellScore = synergyCells.Find(x => x.Cell == existingCell);
                        if (cellScore == null) {
                            cellScore = new CellScoreToDisplay(existingCell, 0);
                            synergyCells.Add(cellScore);
                        }

                        if (neighbour.Category == cell.Category) {
                            resultingScore++;
                            cellScore.Score += GameplayConfig.I.SynergyBonus;
                        }
                    }
                }
            }

            synergyScore = resultingScore;
            return synergyCells;
        }

        private int CalculateTransversality(Tile tile)
        {
            int resultingScore = 0;
            var containedCategories = new List<ProjectCategories>();
            var visitedArea = new List<AreaId>();
            foreach (var cell in tile.Cells) {
                var areaCells = GridManager.I.GetAreaCellsByPos(cell.HexPosition);
                if (areaCells.Count == 0) {
                    continue;
                }

                var area = areaCells[0].Area;
                if (visitedArea.Contains(area) || IsAreaComplete(area)) {
                    continue; // already visited!
                }

                visitedArea.Add(area);
                containedCategories.Clear();
                foreach (var areaCell in areaCells) {
                    var cellCategory = areaCell.Category;
                    if (cellCategory > 0) {
                        if (!containedCategories.Contains(cellCategory)) {
                            containedCategories.Add(cellCategory);
                        }
                    }
                }
                var categoriesCount = System.Enum.GetNames(typeof(ProjectCategories)).Length;
                if (containedCategories.Count == categoriesCount) {
                    resultingScore += GameplayConfig.I.TransversalityBonus;
                    completedAreas.Add(area);
                }
            }
            return resultingScore;
        }

        private bool IsAreaComplete(AreaId area)
        {
            return completedAreas.Contains(area);
        }
    }
}
