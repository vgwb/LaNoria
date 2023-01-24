using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace vgwb.lanoria
{

    [CreateAssetMenu(menuName = "VGWB/Game Data")]
    public class GameData : ScriptableObject
    {
        public ProjectsData ProjectsData;
        public CategoriesData CategoriesData;
    }
}
