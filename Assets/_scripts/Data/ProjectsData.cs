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
        [NonSerialized]
        public TileModel DefinedTileModel;
        public ProjectCategories[] Sequence;
        public string Title;
        public int Year;
        [TextArea]
        public string Description;
        [ShowAssetPreview(128, 128)]
        public Sprite Image;

        public void Init()
        {
            if (TileModel != TileModel.Random) {
                DefinedTileModel = TileModel;
            } else {
                //return GameData.I.Tiles.GetTileBySize(projectData.Sequence.Length);
                DefinedTileModel = TileUtils.GetRandomTileModelByLenght(Sequence.Length);
            }
            //Debug.Log("Init Project with " + DefinedTileModel);
        }

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

            if (projectData.DefinedTileModel != TileModel.Random) {
                return GameData.I.Tiles.GetTileByModel(projectData.DefinedTileModel);
            } else {
                return GameData.I.Tiles.GetTileBySize(projectData.Sequence.Length);
            }
        }

    }
}
