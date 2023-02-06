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
        private List<ProjectData> discardedCards;
        private List<ProjectData> currentHand;

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
            currentHand = new List<ProjectData>();
            discardedCards = new List<ProjectData>();
            Debug.Log("New Deck size: " + DeckSize);
        }

        public List<ProjectData> DiscardAndGetNewHand(ProjectData playedProjectData)
        {

            foreach (var projectInHand in currentHand) {
                if (projectInHand != playedProjectData) {
                    discardedCards.Add(projectInHand);
                }
            }
            if (DeckSize < 4) {
                reshuffleDeck();
            }
            return GetNewHand();
        }

        private List<ProjectData> GetNewHand()
        {
            int handSize = GameplayConfig.I.HandSize;
            currentHand = new List<ProjectData>();
            for (int i = 0; i < handSize; i++) {
                if (DeckSize > 0) {
                    int randomIndex = Random.Range(0, DeckSize - 1);
                    currentHand.Add(deck[randomIndex]);
                    deck.Remove(deck[randomIndex]);
                }
            }
            //            Debug.Log("GetNewHand Deck size: " + DeckSize);
            return currentHand;
        }

        private void reshuffleDeck()
        {
            deck = discardedCards;
        }
    }
}
