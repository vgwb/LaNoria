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
    public class ProjectData
    {
        public string Id;
        public bool Active = true;
        public string Model;
        public ProjectCategories[] Sequence;
        public string Title;
        [TextArea]
        public string Description;
        public Texture Image;
    }

    [CreateAssetMenu(menuName = "VGWB/Projects Data")]
    public class ProjectsData : ScriptableObject
    {
        #region Var
        public List<ProjectData> Projects;
        #endregion

        #region Functions
        public IEnumerable<ProjectData> PickRandomElements(int num)
        {
            var rnd = new System.Random();
            return Projects.OrderBy(x => rnd.Next()).Take(num);
        }
        #endregion
    }
}
