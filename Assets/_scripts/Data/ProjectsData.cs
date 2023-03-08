using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NaughtyAttributes;

namespace vgwb.lanoria
{
    [Serializable]
    public class ProjectData
    {
        public string Id;
        public bool Active = true;
        // [ShowAssetPreview(128, 128)]
        // public GameObject TilePrefab;
        public TileModel TileModel;
        public ProjectCategories[] Sequence;
        public string Title;
        [TextArea]
        public string Description;
        [ShowAssetPreview(128, 128)]
        public Sprite Image;

        public int Size()
        {
            return Sequence.Count();
        }

        public void AddCategories(HashSet<ProjectCategories> categories)
        {
            foreach (var cat in Sequence) {
                categories.Add(cat);
            }
        }

        public override string ToString()
        {
            return Title + " - " + Sequence.Length;
        }
    }

    [CreateAssetMenu(menuName = "VGWB/Projects Data")]
    public class ProjectsData : ScriptableObject
    {
        public List<ProjectData> Projects;

        public IEnumerable<ProjectData> PickRandomProjects(int howMany)
        {
            var rnd = new System.Random();
            return Projects.OrderBy(x => rnd.Next()).Take(howMany);
        }

        public GameObject GetTile(ProjectData projectData)
        {
            if (projectData == null) {
                return null;
            }

            if (projectData.TileModel != TileModel.Random) {
                return GameData.I.Tiles.GetTileByModel(projectData.TileModel);
            } else {
                return GameData.I.Tiles.GetTileBySize(projectData.Sequence.Length);
            }
        }

    }
}
