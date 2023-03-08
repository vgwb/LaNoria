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
        private int handSize;

        void Start()
        {
            handSize = GameplayConfig.I.HandSize;
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
            currentHand = new List<ProjectData>();
            ProjectData validatedCard;
            if (DeckSize < handSize) {
                reshuffleDeck();
            }

            for (int i = 1; i <= handSize; i++) {
                validatedCard = deck[0];
                foreach (var pickedCard in deck) {
                    if (checkCard(pickedCard)) {
                        validatedCard = pickedCard;
                        break;
                    }
                    //                    Debug.Log("FOUND NO GOOD CARD");
                }
                currentHand.Add(validatedCard);
                deck.Remove(validatedCard);
            }
            //            Debug.Log("GetNewHand Deck size: " + DeckSize);
            return currentHand;
        }

        private bool checkCard(ProjectData cardToCheck)
        {
            //            Debug.Log("checkCard " + currentHand.Count + " / " + handSize);
            if (currentHand.Count + 1 < handSize) {
                return true;
            } else {
                return check_Shape(cardToCheck) && check_Size(cardToCheck) && check_Colors(cardToCheck);
            }
        }

        private bool check_Shape(ProjectData cardToCheck)
        {
            // no 3 identical shapes
            return !(currentHand[0].TileModel == currentHand[1].TileModel &&
            currentHand[0].TileModel == cardToCheck.TileModel);
        }

        private bool check_Size(ProjectData cardToCheck)
        {
            // no same number of tiles
            return !(currentHand[0].Size() == currentHand[1].Size() &&
            currentHand[0].Size() == cardToCheck.Size());
        }

        private bool check_Colors(ProjectData cardToCheck)
        {
            var categoriesUsed = new HashSet<ProjectCategories>();
            currentHand[0].AddCategories(categoriesUsed);
            //            Debug.Log("Categories 0 used Count: " + categoriesUsed.Count);
            currentHand[1].AddCategories(categoriesUsed);
            //            Debug.Log("Categories 1 used Count: " + categoriesUsed.Count);
            cardToCheck.AddCategories(categoriesUsed);
            //            Debug.Log("Categories 2 used Count: " + categoriesUsed.Count);

            // at least 3 different colors
            return categoriesUsed.Count >= 3;
        }

        private void reshuffleDeck()
        {
            deck = discardedCards;
            deck.Shuffle();
        }

    }
}
