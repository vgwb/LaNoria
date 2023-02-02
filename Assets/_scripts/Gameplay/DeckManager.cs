using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using vgwb.framework;

namespace vgwb.lanoria
{
    public class DeckManager : SingletonMonoBehaviour<DeckManager>
    {
        private List<ProjectData> deck;
        private int DeckSize => deck.Count;

        void Start()
        {
        }

        public void PrepareNewDeck()
        {
            deck = new List<ProjectData>();
            foreach (var project in GameData.I.Projects.Projects) {
                if (project.Active) {
                    deck.Add(project);
                }
            }
            Debug.Log("New Deck: " + DeckSize + " cards");
        }

        public IEnumerable<ProjectData> GetNewHand()
        {
            int handSize = GameplayConfig.I.HandSize;

            return GameData.I.Projects.PickRandomProjects(handSize);
        }
    }
}
