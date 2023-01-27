using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb.lanoria
{
    public class ScoreManager : GameplayComponent
    {
        #region Var
        public int ActualScore;
        public delegate void ScoreEvent(int score);
        public ScoreEvent OnScoreUpdate;

        [SerializeField] private int synergyScore;
        /// <summary>
        /// Subregions with all the colors.
        /// </summary>
        private List<Subregion> completedSubregion;
        #endregion

        #region MonoB
        protected override void Awake()
        {
            base.Awake();

            completedSubregion = new List<Subregion>();
            ActualScore = 0;
        }
        #endregion

        #region Functions
        public void UpdateScore(PlaceableProject placeable)
        {
            int basicScore = CalculateBasicPoints(placeable);
            int transversalityScore = CalculateTransversality(placeable);
            int totalScore = basicScore + synergyScore + transversalityScore;
            ActualScore += totalScore;
            Debug.Log("basic: " + basicScore + " sinergy: " + synergyScore + " transversality: " + transversalityScore);

            if (OnScoreUpdate != null) {
                OnScoreUpdate(ActualScore);
            }
        }

        public void PreviewSynergy()
        {

        }

        private int CalculateBasicPoints(PlaceableProject placeable)
        {
            return placeable.CellsNum;
        }

        public List<Vector3> CalculateSynergy(PlaceableProject placeable)
        {
            int resultingScore = 0;
            var grid = GridManager.I;
            var positionsToExclude = placeable.GetCellsHexPositions();
            var synergyPoints = new List<Vector3>();

            foreach (var cell in placeable.Cells) {
                var neighbours = grid.GetNeighboursByPos(cell.HexPosition);
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

        private int CalculateTransversality(PlaceableProject placeable)
        {
            int resultingScore = 0;
            var grid = GridManager.I;
            List<ProjectCategories> containedCategories = new List<ProjectCategories>();
            List<Subregion> visitedSubregion = new List<Subregion>();
            foreach (var placCell in placeable.Cells) {
                Vector3 cellPos = placCell.HexPosition;
                var subregionCells = grid.GetAllSubregionCellsByPos(cellPos);
                if (subregionCells.Count == 0) {
                    continue;
                }

                var subregion = subregionCells[0].MySubregion;
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

        private bool IsSubregionComplete(Subregion subregion)
        {
            return completedSubregion.Contains(subregion);
        }
        #endregion
    }
}
