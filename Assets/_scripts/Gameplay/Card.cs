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
        public CanvasGroup MyCanvas;
        public TMP_Text CardTitle;
        public RawImage PrefabImg;
        public LeanFingerDownCanvas SpawnCanvas;
        public LeanSpawnWithFinger Spawner;
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
        }

        private void OnDestroy()
        {
            Spawner.OnSpawnedClone = null;
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
            if (!Playable) {
                MyCanvas.alpha = 0.5f;
                MyCanvas.blocksRaycasts = false;
            }
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
    }
}
