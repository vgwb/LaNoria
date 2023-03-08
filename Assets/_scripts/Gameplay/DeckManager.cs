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
            currentHand = new List<ProjectData>();
            discardedCards = new List<ProjectData>();

            foreach (var project in GameData.I.Projects.Projects) {
                if (project.Active) {
                    deck.Add(project);
                }
            }
            deck.Shuffle();
            // Debug.Log("New Deck size: " + DeckSize);
        }

        public List<ProjectData> DiscardAndGetNewHand(ProjectData playedProjectData)
        {
            foreach (var projectInHand in currentHand) {
                if (projectInHand != playedProjectData) {
                    discardedCards.Add(projectInHand);
                }
            }
            if (DeckSize < GameplayConfig.I.HandSize) {
                reshuffleDeck();
            }
            return GetNewHand();
        }

        private List<ProjectData> GetNewHand()
        {
            int handSize = GameplayConfig.I.HandSize;
            currentHand = new List<ProjectData>();
            ProjectData pickedCard;
            if (DeckSize < handSize) {
                reshuffleDeck();
            }

            for (int i = 1; i <= handSize; i++) {
                // check only last card
                pickedCard = deck[0];
                if (i == handSize) {
                    if (check1() && check2() && check3()) {
                        pickedCard = deck[0];
                    }
                }

                currentHand.Add(pickedCard);
                deck.Remove(pickedCard);
            }
            //            Debug.Log("GetNewHand Deck size: " + DeckSize);
            return currentHand;
        }

        private void reshuffleDeck()
        {
            deck = discardedCards;
            deck.Shuffle();
        }

        private bool check1()
        {
            // no 3 identical shapes
            return true;
        }

        private bool check2()
        {
            // no same number of tiles
            return true;
        }

        private bool check3()
        {
            // at least 3 different colors
            return true;
        }



    }
}
