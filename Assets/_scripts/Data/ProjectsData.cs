using System;
using System.Collections;
using System.Collections.Generic;
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
        public string Model;
        public ProjectCategories[] Sequence;
        public string Title;
        public string Description;
        public Texture Image;
    }

    [CreateAssetMenu(menuName = "VGWB/Projects Data")]
    public class ProjectsData : ScriptableObject
    {
        public List<ProjectData> Projects;
    }
}