using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb.lanoria
{
    public class PreviewManager : GameplayComponent
    {
        public GameObject PointPreviewPrefab;
        public bool UsePreview = true;
        private ScoreManager scorer;

        protected override void Awake()
        {
            base.Awake();

            scorer = manager.Scorer;

            if (PointPreviewPrefab == null) {
                Debug.LogError("ScoreManager - Awake(): no point preview prefab defined!");
            }
        }

        public void PreviewScore(Tile placeable)
        {
            CleanPreview();
            if (!UsePreview) {
                return;
            }

            var points = scorer.CalculateSynergy(placeable);
            foreach (var point in points) {
                InstantiatePointPreview(point, 1);
            }
        }

        public void CleanPreview()
        {
            var elements = UI_manager.I.PanelGameplay.GetPreviewElements();
            foreach (var element in elements) {
                Destroy(element);
            }
        }

        private GameObject InstantiatePointPreview(Vector3 worldPos, int points)
        {
            var parent = UI_manager.I.PanelGameplay.PanelPreview;
            var instance = Instantiate(PointPreviewPrefab, parent);
            var pointPreview = instance.GetComponent<PointPreview>();
            if (pointPreview != null) {
                pointPreview.Init(worldPos, points);
            }

            return instance;
        }
    }
}