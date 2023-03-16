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
                    project.Init();
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
            bool shapes_ok = !(currentHand[0].DefinedTileModel == currentHand[1].DefinedTileModel &&
            currentHand[0].DefinedTileModel == cardToCheck.DefinedTileModel);

            if (shapes_ok) {
                return shapes_ok;
            } else {
                // Debug.LogWarning("check_Shape");
                // Debug.Log("Til 0 Model = " + currentHand[0].DefinedTileModel);
                // Debug.Log("Tile 1 Model = " + currentHand[1].DefinedTileModel);
                // Debug.Log("Tile 2 Model = " + cardToCheck.DefinedTileModel);
                return shapes_ok;
            }
        }

        private bool check_Size(ProjectData cardToCheck)
        {
            // no same number of tiles
            bool sizes_ok = !(currentHand[0].Size() == currentHand[1].Size() &&
            currentHand[0].Size() == cardToCheck.Size());

            if (sizes_ok) {
                return sizes_ok;
            } else {
                // Debug.LogWarning("check_Shape");
                // Debug.Log("Tile 0 Size = " + currentHand[0].Size());
                // Debug.Log("Tile 1 Size = " + currentHand[1].Size());
                // Debug.Log("Tile 2 Size = " + cardToCheck.Size());
                return sizes_ok;
            }
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
