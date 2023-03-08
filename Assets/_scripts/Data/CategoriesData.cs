using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace vgwb.lanoria
{
    public enum ProjectCategories
    {
        Environment = 1,
        Equality = 2,
        Tech = 3,
        People = 4
    }

    [Serializable]
    public struct CategoryData
    {
        public ProjectCategories Category;
        public string Title;
        public Color Color;
        public Material Material;
        public Sprite Icon;
    }

    [CreateAssetMenu(menuName = "VGWB/Categories Data")]
    public class CategoriesData : ScriptableObject
    {
        public List<CategoryData> Categories;

        public Color GetColor(ProjectCategories category)
        {
            return Categories.Find(x => x.Category == category).Color;
        }

        public Sprite GetIcon(ProjectCategories category)
        {
            return Categories.Find(x => x.Category == category).Icon;
        }


        public Material GetMaterial(ProjectCategories category)
        {
            return Categories.Find(x => x.Category == category).Material;
        }

    }
}
