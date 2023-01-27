using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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

        public IEnumerable<ProjectData> PickRandomElements(int num)
        {
            var rnd = new System.Random();
            return Projects.OrderBy(x => rnd.Next()).Take(num);
        }

        public GameObject GetTile(ProjectData projectData)
        {
            if (projectData == null) {
                return null;
            }

            GameObject tileModel = null;
            if (projectData.TileModel != TileModel.Undefined) {
                tileModel = GameData.I.Tiles.GetProjectModelByKey(projectData.TileModel);
            } else {
                tileModel = GameData.I.Tiles.GetProjectModelByCellNum(projectData.Sequence.Length);
            }

            return tileModel;
        }
    }
}
