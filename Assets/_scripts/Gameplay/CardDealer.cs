using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb.lanoria
{
    public class CardDealer : GameplayComponent
    {
        private ProjectsData projectAtlas;

        protected override void Awake()
        {
            base.Awake();

            projectAtlas = GameData.I.Projects;
            if (projectAtlas == null) {
                Debug.LogError("CardDealer - Awake(): no project atlas defined!");
            }
        }

        public IEnumerable<ProjectData> DrawProjects()
        {
            int cardsNum = GameplayConfig.I.CardToDraw;

            return projectAtlas.PickRandomElements(cardsNum);
        }
    }
}
