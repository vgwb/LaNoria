using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb.lanoria
{
    public class ScoreManager : MonoBehaviour
    {
        public int CurrentScore { get; private set; }
        public delegate void ScoreEvent(int score, int points);
        public ScoreEvent OnScoreUpdate;

        [SerializeField] private int synergyScore;
        private List<AreaId> completedAreas;

        void Awake()
        {
            completedAreas = new List<AreaId>();
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
