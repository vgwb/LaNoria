using Lean.Touch;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace vgwb.lanoria
{
    public class Card : MonoBehaviour
    {
        public TMP_Text CardTitle;
        public RawImage PrefabImg;
        public LeanFingerDownCanvas SpawnCanvas;
        public LeanSpawnWithFinger Spawner;
        public bool Playable;

        [HideInInspector]
        public ProjectData Project { get; private set; }
        public GameObject TilePrefab { get; private set; }
        public RectTransform Rect { get; private set; }

        private int cardNumber;

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
            cardNumber = cardIndex;
            SetTitle(Project.Title);
            SetImage(texture); // visualize the 3d prefab into the canvas
        }

        public void SetPlayable(bool isPlayable)
        {
            Playable = isPlayable;
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

        public bool IsPlayable()
        {
            return Playable;
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
            GameManager.I.GameFSM.OnClickCard(Project, cardNumber);
            GameManager.I.GameFSM.OnPrefabSpawned(tile);
        }
    }
}
