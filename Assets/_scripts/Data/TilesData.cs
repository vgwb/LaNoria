using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;

namespace vgwb.lanoria
{
    [Serializable]
    public class TileModel
    {
        public TileType Model;
        public int Length;
        public GameObject Prefab;
    }

    [CreateAssetMenu(menuName = "VGWB/Tiles Data")]
    public class TilesData : ScriptableObject
    {
        public List<TileModel> Tiles;


        public GameObject GetProjectModelByKey(TileType modelType)
        {
            return Tiles.Find(x => x.Model == modelType).Prefab;
        }

        public GameObject GetProjectModelByCellNum(int cellNum)
        {
            var tuples = Tiles.FindAll(x => x.Length == cellNum);
            int randomIndex = UnityEngine.Random.Range(0, tuples.Count);
            return tuples[randomIndex].Prefab;
        }
    }
}
