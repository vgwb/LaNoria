using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb.lanoria
{
    [RequireComponent(typeof(Tile))]
    public class HighlighterTutorial : MonoBehaviour
    {
        public TutorialStep DisplayStep = TutorialStep.Rotate;
        public GameObject HighlighterPrefab;
        private bool displayTutorial;
        private Vector3 originScale;
        private Tile tile;
        private GameObject highlighterInstance;

        void Awake()
        {
            displayTutorial = false;
            highlighterInstance = null;
            tile = GetComponent<Tile>();
        }


        void Update()
        {
            HandleTutorial();
        }

        private void HandleTutorial()
        {
            bool stepIsPlaying = TutorialManager.I.IsPlayingStep(DisplayStep);
            if (displayTutorial != stepIsPlaying) {
                displayTutorial = stepIsPlaying;
                
                if (displayTutorial) {
                    highlighterInstance = Instantiate(HighlighterPrefab, tile.Pivot.transform);
                    originScale = highlighterInstance.transform.localScale;
                    Bounce(false);
                } else {
                    Destroy(highlighterInstance);
                    highlighterInstance = null;
                }
            }

            if (highlighterInstance != null) {
                highlighterInstance.SetActive(!tile.IsMoving());
            }
        }

        private void Bounce(bool increment)
        {
            float from = GameplayConfig.I.BounceMinDim;
            float to = 1.0f;
            float duration = GameplayConfig.I.BounceDuration;
            if (!increment) {
                from = 1.0f;
                to = GameplayConfig.I.BounceMinDim;
            }
            DOVirtual.Float(from, to, duration, ResizeRect).OnComplete(() => Bounce(!increment));
        }

        private void ResizeRect(float perc)
        {
            if (highlighterInstance != null) {
                highlighterInstance.transform.localScale = originScale * perc;
            }
        }
    } 
}
