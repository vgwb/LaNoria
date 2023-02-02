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
            Debug.Log("New Deck size: " + DeckSize);
        }

        public List<ProjectData> GetNewHand()
        {
            int handSize = GameplayConfig.I.HandSize;
            var hand = new List<ProjectData>();
            for (int i = 0; i < handSize; i++) {
                if (DeckSize > 0) {
                    int randomIndex = Random.Range(0, handSize);
                    hand.Add(deck[randomIndex]);
                    deck.Remove(deck[randomIndex]);
                }
            }
            Debug.Log("GetNewHand Deck size: " + DeckSize);
            return hand;
        }
    }
}
