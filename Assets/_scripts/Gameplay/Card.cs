using Lean.Touch;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

namespace vgwb.lanoria
{
    public class Card : MonoBehaviour
    {
        public TMP_Text CardTitle;
        public RawImage PrefabImg;
        public Image TutorialImg;
        public LeanFingerDownCanvas SpawnCanvas;
        public LeanSpawnWithFinger Spawner;
        private bool displayTutorial;
        private Vector2 originSizeDelta;
        public bool Playable { get; private set; }

        [HideInInspector]
        public ProjectData Project { get; private set; }
        public GameObject TilePrefab { get; private set; }
        public RectTransform Rect { get; private set; }

        public int CardIndex;
        public Vector3 originalLocalPos;

        private void Awake()
        {
            Rect = GetComponent<RectTransform>();
            originSizeDelta = TutorialImg.rectTransform.sizeDelta;
            displayTutorial = TutorialManager.I.IsPlayingStep(TutorialStep.Drag);
            TutorialImg.gameObject.SetActive(displayTutorial);
        }

        private void Update()
        {
            HandleTutorial();
        }

        private void OnDestroy()
        {
            Spawner.OnSpawnedClone = null;
            TutorialImg.DOKill();
        }

        public void Init(ProjectData projectData, Texture texture, GameObject prefabUsed, int cardIndex)
        {
            Project = projectData;
            TilePrefab = prefabUsed;
            Spawner.Prefab = TilePrefab.transform;
            Spawner.OnSpawnedClone += OnPrefabSpawned;
            CardIndex = cardIndex;
            SetTitle(Project.Title);
            SetImage(texture); // visualize the 3d prefab into the canvas
            originalLocalPos = transform.localPosition;
        }

        public void SetPlayable(bool isPlayable)
        {
            Playable = isPlayable;
            // if (Playable) {
            //     transform.localPosition = originalLocalPos;
            // } else {
            //     Debug.Log("NOT PLAYABLE card " + cardNumber);
            //     transform.DOLocalMoveY(-30, 1);
            //     //transform.localPosition = new Vector3(originalLocalPos.x, originalLocalPos.y - 30, originalLocalPos.z);
            // }
        }

        public bool IsPlayable()
        {
            return Playable;
        }

        public void DoSelect(bool status)
        {
            if (status) {
                Rect.DOAnchorPosY(-242, 0.5f);
            } else {
                Rect.DOAnchorPosY(0.0f, 0.5f);
            }
        }

        public void HideInBottomScreen()
        {
            if (Rect != null) {
                float height = Rect.sizeDelta.y;
                var pos = Rect.localPosition;
                pos.y = -height;
                Rect.localPosition = pos;
            }
        }

        private void SetTitle(string title)
        {
            if (CardTitle != null) {
                CardTitle.text = title;
            }
        }

        private void SetImage(Texture texture)
        {
            if (PrefabImg != null) {
                PrefabImg.texture = texture;
            }
        }

        private void OnPrefabSpawned(GameObject clone)
        {
            var tile = clone.GetComponent<Tile>();
            tile.SetupCellsColor(Project);
            tile.SetupForDrag();
            GameManager.I.GameFSM.OnClickCard(Project, CardIndex);
            GameManager.I.GameFSM.OnPrefabSpawned(tile);
        }

        private void HandleTutorial()
        {
            if (displayTutorial != TutorialManager.I.IsPlayingStep(TutorialStep.Drag)) {
                displayTutorial = TutorialManager.I.IsPlayingStep(TutorialStep.Drag);
                Debug.Log("Change");
                TutorialImg.gameObject.SetActive(displayTutorial);
                if (displayTutorial) {
                    Bounce(false);
                }
            }
        }

        private void Bounce(bool increment)
        {
            float from = 0.6f;
            float to = 1.0f;
            float duration = 0.6f;
            if (!increment) {
                from = 1.0f;
                to = 0.6f;
            }
            DOVirtual.Float(from, to, duration, ResizeRect).OnComplete(() => Bounce(!increment));
        }

        private void ResizeRect(float perc)
        {
            if (TutorialImg != null) {
                Debug.Log("size delta: "+ TutorialImg.rectTransform.sizeDelta+" - "+perc);
                TutorialImg.rectTransform.sizeDelta = originSizeDelta * perc;
            }
        }
    }
}
