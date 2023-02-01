using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;

namespace vgwb.lanoria
{
    public enum TileModel
    {
        Random = 0,
        P2A = 21,
        P3A = 31,
        P3B = 32,
        P3C = 33,
        P4A = 41,
        P4B = 42,
        P4C = 43,
        P4D = 44,
        P4E = 45,
        P4F = 46
    }

    [Serializable]
    public class TileInfo
    {
        public TileModel Model;
        public int Length;
        public GameObject Prefab;
    }

    [CreateAssetMenu(menuName = "VGWB/Tiles Data")]
    public class TilesData : ScriptableObject
    {
        public List<TileInfo> Tiles;

        public GameObject GetTileByModel(TileModel model)
        {
            return Tiles.Find(x => x.Model == model).Prefab;
        }

        public GameObject GetTileByLength(int cellNum)
        {
            var tuples = Tiles.FindAll(x => x.Length == cellNum);
            int randomIndex = UnityEngine.Random.Range(0, tuples.Count);
            return tuples[randomIndex].Prefab;
        }
    }
}
