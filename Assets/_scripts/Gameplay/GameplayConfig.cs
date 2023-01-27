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
        public struct EnumToDisplayedName<T>
        {
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
        [Header("Projects Setup")]
        public Color BlankColor = Color.white;
        [Header("Drawing Rules")]
        public int CardToDraw = 4;
        [Header("Region Definition")]
        public List<EnumToDisplayedName<Subregion>> SubregionNames;
        public List<CategoryToColor<Subregion>> SubregionColors;
        public Vector3 LabelDebugOffset;
        public int LabelDebugFontSize = 15;
        [Header("Score")]
        public int TransversalityBonus = 20;
        [Header("UI")]
        public float SlideProjectPanelTime = 0.8f;
        public float SlideProjectPanelPixelIn = 50.0f;
        public Vector3 UIProjectRotation = new Vector3(0.0f, -90.0f, 60.0f);
        [Header("UI transitions")]
        public float ResetCameraRotYOnPlay = 1.0f;
        public float FadeInGameCanvas = 1.0f;
        public float CardAppearsTime = 1.0f;
        #endregion

        #region Functions
        public GameObject GetProjectModelFromData(ProjectData projectData)
        {
            if (projectData == null) {
                return null;
            }

            GameObject tileModel = null;
            if (projectData.TileModel != TileType.Undefined) {
                tileModel = DataManager.I.Data.TilesData.GetProjectModelByKey(projectData.TileModel);
            } else {
                tileModel = DataManager.I.Data.TilesData.GetProjectModelByCellNum(projectData.Sequence.Length);
            }

            return tileModel;
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
