using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb.lanoria
{
    public class ScoreManager : MonoBehaviour
    {
        #region Var
        public int ActualScore;
        /// <summary>
        /// Subregions with all the colors.
        /// </summary>
        private List<Subregion> completedSubregion;
        #endregion

        #region MonoB
        private void Awake()
        {
            completedSubregion = new List<Subregion>();
        }
        #endregion

        #region Functions
        public void UpdateScore(Placeable placeable)
        {
            int basicPoints = CalculateBasicPoints(placeable);
            int transversalityPoints = CalculateTransversality(placeable);

            Debug.Log("basic points: "+basicPoints+" transversality: "+transversalityPoints);
        }

        private int CalculateBasicPoints(Placeable placeable)
        {
            return placeable.CellsNum;
        }

        private int CalculateTransversality(Placeable placeable)
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
