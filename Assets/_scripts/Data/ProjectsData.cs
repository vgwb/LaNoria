using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace vgwb.lanoria
{
    [Serializable]
    public class ProjectData
    {
        public string Id;
        public bool Active = true;
        public TileModel TileModel;
        public ProjectCategories[] Sequence;
        public string Title;
        [TextArea]
        public string Description;
        public Sprite Image;
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

            GameObject tilePrefab;
            if (projectData.TileModel != TileModel.Random) {
                tilePrefab = GameData.I.Tiles.GetTileByModel(projectData.TileModel);
            } else {
                tilePrefab = GameData.I.Tiles.GetTileByLength(projectData.Sequence.Length);
            }

            return tilePrefab;
        }
    }
}
