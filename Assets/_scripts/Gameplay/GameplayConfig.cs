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

        [System.Serializable]
        public struct CategoryToColor<T>
        {
            public T Category;
            public Color ColorToUse;
            public Material MaterialToUse;
        }

        [System.Serializable]
        public struct EnumToDisplayedName<T> {
            public T MyEnum;
            public string DisplayedName;
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
        public List<CategoryToColor<ProjectCategories>> CategoryColorsMap;
        public Color BlankColor = Color.white;
        [Header("Drawing Rules")]
        public int CardToDraw = 4;
        [Header("Region Definition")]
        public List<EnumToDisplayedName<Subregion>> SubregionNames;
        public List<CategoryToColor<Subregion>> SubregionColors;
        [Header("Region Definition")]
        public Vector3 LabelDebugOffset;
        public int LabelDebugFontSize = 15;
        #endregion

        #region Functions
        public GameObject GetProjectModelByKey(string key)
        {
            var tuple = ProjectPrefabsMap.Find(x => x.Key == key);

            return tuple.Prefab;
        }

        public Color GetCategoryColorsByType(ProjectCategories category)
        {
            var tuple = CategoryColorsMap.Find(x => x.Category == category);

            return tuple.ColorToUse;
        }

        public Material GetCategoryMaterialByType(ProjectCategories category)
        {
            var tuple = CategoryColorsMap.Find(x => x.Category == category);

            return tuple.MaterialToUse;
        }

        public string GetSubregionNameByEnum(Subregion subregion)
        {
            var tuple = SubregionNames.Find(x => x.MyEnum == subregion);

            return tuple.DisplayedName;
        }

        public Color GetSubregionColorByEnum(Subregion subregion)
        {
            var tuple = SubregionColors.Find(x => x.Category == subregion);

            return tuple.ColorToUse;
        }
        #endregion
    }
}