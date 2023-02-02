using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb.lanoria
{
    public class ScoreManager : GameplayComponent
    {
        public int CurrentScore { get; private set; }
        public delegate void ScoreEvent(int score, int points);
        public ScoreEvent OnScoreUpdate;

        [SerializeField] private int synergyScore;
        private List<AreaId> completedSubregion;

        protected override void Awake()
        {
            base.Awake();
            completedSubregion = new List<AreaId>();
            CurrentScore = 0;
        }

        public void UpdateScore(Tile placeable)
        {
            int placementPoints = CalculateBasicPoints(placeable);
            int transversalityPoints = CalculateTransversality(placeable);
            int totalPoints = placementPoints + synergyScore + transversalityPoints;
            CurrentScore += totalPoints;
            Debug.Log("basic: " + placementPoints + " sinergy: " + synergyScore + " transversality: " + transversalityPoints);

            OnScoreUpdate?.Invoke(CurrentScore, totalPoints);
        }

        public void PreviewSynergy()
        {

        }

        private int CalculateBasicPoints(Tile placeable)
        {
            return placeable.Size;
        }

        public List<Vector3> CalculateSynergy(Tile placeable)
        {
            int resultingScore = 0;
            var positionsToExclude = placeable.GetCellsHexPositions();
            var synergyPoints = new List<Vector3>();

            foreach (var cell in placeable.Cells) {
                var neighbours = GridManager.I.GetNeighboursByPos(cell.HexPosition);
                foreach (var neighbour in neighbours) {
                    if (positionsToExclude.Contains(neighbour.HexPosition)) {
                        continue;
                    }
                    if (neighbour.Category == cell.Category) {
                        resultingScore++;
                        Vector3 midPoint = cell.HexPosition + (neighbour.HexPosition - cell.HexPosition) / 2;
                        synergyPoints.Add(midPoint);
                    }
                }
            }

            synergyScore = resultingScore;

            return synergyPoints;
        }

        private int CalculateTransversality(Tile placeable)
        {
            int resultingScore = 0;
            var containedCategories = new List<ProjectCategories>();
            var visitedSubregion = new List<AreaId>();
            foreach (var placCell in placeable.Cells) {
                Vector3 cellPos = placCell.HexPosition;
                var subregionCells = GridManager.I.GetAllSubregionCellsByPos(cellPos);
                if (subregionCells.Count == 0) {
                    continue;
                }

                var subregion = subregionCells[0].Area;
                if (visitedSubregion.Contains(subregion) || IsSubregionComplete(subregion)) {
                    continue; // already visited!
                }

                visitedSubregion.Add(subregion);
                containedCategories.Clear();
                foreach (var subregionCell in subregionCells) {
                    var subCellCategory = subregionCell.Category;
                    if (subCellCategory > 0) {
                        if (!containedCategories.Contains(subCellCategory)) {
                            containedCategories.Add(subCellCategory);
                        }
                    }
                }
                var categoriesCount = System.Enum.GetNames(typeof(ProjectCategories)).Length;
                if (containedCategories.Count == categoriesCount) {
                    resultingScore += GameplayConfig.I.TransversalityBonus;
                    completedSubregion.Add(subregion);
                }
            }
            return resultingScore;
        }

        private bool IsSubregionComplete(AreaId area)
        {
            return completedSubregion.Contains(area);
        }
    }
}
