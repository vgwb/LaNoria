using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb.lanoria
{
    public class CardDealer : MonoBehaviour
    {
        #region Var
        private ProjectsData projectAtlas;
        #endregion

        #region MonoB
        void Awake()
        {
            projectAtlas = DataManager.I.Data.ProjectsData;
            if (projectAtlas == null) {
                Debug.LogError("CardDealer - Awake(): no project atlas defined!");
            }
        }
        #endregion

        #region Functions
        public IEnumerable<ProjectData> DrawProjects()
        {
            int cardsNum = GameplayConfig.I.CardToDraw;

            return projectAtlas.PickRandomElements(cardsNum);
        }

        
        #endregion
    }
}
