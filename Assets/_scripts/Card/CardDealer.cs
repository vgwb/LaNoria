using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb.lanoria
{
    public class CardDealer : MonoBehaviour
    {
        #region Var
        [Header("Project Atlas")]
        public ProjectsData ProjectAtlas;
        #endregion

        #region MonoB
        void Awake()
        {
            if (ProjectAtlas == null) {
                Debug.LogError("CardDealer - Awake(): no project atlas defined!");
            }
        }
        #endregion

        #region Functions
        public IEnumerable<ProjectData> DrawProjects()
        {
            int cardsNum = GameplayConfig.I.CardToDraw;

            return ProjectAtlas.PickRandomElements(cardsNum);
        }

        
        #endregion
    }
}
