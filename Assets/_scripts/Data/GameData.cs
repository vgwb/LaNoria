using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using vgwb.framework;

namespace vgwb.lanoria
{

    [CreateAssetMenu(menuName = "VGWB/Game Data")]
    public class GameData : SingletonScriptableObject<GameData>
    {
        public ProjectsData Projects;
        public CategoriesData Categories;
        public TilesData Tiles;
        public AreasData Areas;
    }
}
