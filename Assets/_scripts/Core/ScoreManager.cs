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

        [SerializeField] private int adjacencyBonus;
        [SerializeField] private int areaScore;
        [SerializeField] private List<AreaId> completedAreas;
        [SerializeField] private List<AreaId> areasToConfirm;

        void Start()
        {
            completedAreas = new List<AreaId>();
            areasToConfirm = new List<AreaId>();
            Score = 0;
        }

        public void UpdateScore(Tile tile)
        {
            int placementBonus = CalculatePlacementBonus(tile);
            int newPoints = placementBonus + adjacencyBonus + areaScore;
            Score += newPoints;
            Debug.Log("basic: " + placementBonus + " adjacency: " + adjacencyBonus + " area: " + areaScore);
            confirmAreas();

            UI_manager.I.PanelGameplay.SetScoreUI(Score, newPoints);
        }

        public void ResetScore()
        {
            Score = 0;
            adjacencyBonus = 0;
            completedAreas.Clear();
            areasToConfirm.Clear();
        }

        public List<CellScoreToDisplay> CalculateAdjacencyBonus(Tile tile)
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

                        if (existingCell.Category == cell.Category) {
                            resultingScore++;
                            cellScore.Score += GameplayConfig.I.AdjacencyBonus;
                        }
                    }
                }
            }

            adjacencyBonus = resultingScore;
            return synergyCells;
        }

        public List<AreaId> CalculateAreaBonus(Tile tile)
        {
            int resultingScore = 0;
            var containedCategories = new List<ProjectCategories>();
            var visitedArea = new List<AreaId>(); // temp visited
            areasToConfirm.Clear(); // the new areas will be completed
            foreach (var cell in tile.Cells) { // browse the tile cells
                var areaCells = GridManager.I.GetAreaCellsFromSinglePos(cell.HexPosition);
                if (areaCells.Count == 0) {
                    continue;
                }

                var area = areaCells[0].Area;
                if (visitedArea.Contains(area) || isAreaComplete(area)) {
                    continue; // already visited!
                }

                // get the tile's cells inside the area
                visitedArea.Add(area);
                containedCategories.Clear();
                containedCategories.AddRange(tile.GetTileCategoriesInArea(area));

                foreach (var areaCell in areaCells) {
                    var existingTile = TileManager.I.GetPlacedTileByPosition(areaCell.HexPosition);
                    if (existingTile != null) {
                        if (!containedCategories.Contains(existingTile.Category)) {
                            containedCategories.Add(existingTile.Category);
                        }
                    }
                }

                var categoriesCount = System.Enum.GetNames(typeof(ProjectCategories)).Length;
                if (containedCategories.Count == categoriesCount) {
                    resultingScore += GameplayConfig.I.AreaBonus;
                    areasToConfirm.Add(area);
                }
            }

            areaScore = resultingScore;
            return areasToConfirm;
        }

        private int CalculatePlacementBonus(Tile tile)
        {
            var points = tile.Size switch {
                2 => GameplayConfig.I.Place2Bonus,
                3 => GameplayConfig.I.Place3Bonus,
                4 => GameplayConfig.I.Place4Bonus,
                _ => 0,
            };
            return points;
        }

        private bool isAreaComplete(AreaId area)
        {
            return completedAreas.Contains(area);
        }

        private void confirmAreas()
        {
            foreach (var area in areasToConfirm) {
                if (!completedAreas.Contains(area)) {
                    completedAreas.Add(area);
                }
            }
            areasToConfirm.Clear();
        }
    }
}
