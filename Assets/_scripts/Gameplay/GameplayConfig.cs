using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using vgwb.framework;

namespace vgwb.lanoria
{
    [CreateAssetMenu(fileName = "GameplayConfig", menuName = "Scriptable Object/General/GameplayConfig", order = 2)]
    public class GameplayConfig : SingletonScriptableObject<GameplayConfig>
    {
        #region Struct
        [System.Serializable]
        public struct ModelKeyToPrefab
        {
            public string Key;
            public GameObject Prefab;
        }
        #endregion

        #region Var
        [Header("Placeable Settings")]
        public Color MovingColor;
        public Color OverlapColor;
        public Outline.Mode PlaceableOutlineMode = Outline.Mode.OutlineVisible;
        public float OutlineWidth = 7.0f;
        public float DragYOffset = 0.2f;
        public List<ModelKeyToPrefab> ProjectPrefabsMap;
        [Header("Project Setup")]
        public Color ClimateColor;
        public Color EqualityColor;
        public Color TechColor;
        public Color DepopulationColor;
        #endregion
    }
}
