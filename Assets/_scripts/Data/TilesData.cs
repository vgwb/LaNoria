using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;
using NaughtyAttributes;

namespace vgwb.lanoria
{
    public enum TileModel
    {
        Random = 0,
        P2A = 21,
        P3A = 31,
        P3B = 32,
        P3B_alt = 34,
        P3C = 33,
        P4A = 41,
        P4B = 42,
        P4B_alt = 47,
        P4C = 43,
        P4D = 44,
        P4D_alt = 48,
        P4E = 45,
        P4F = 46,
        P4F_alt = 49
    }

    [CreateAssetMenu(menuName = "VGWB/Tiles Data")]
    public class TilesData : ScriptableObject
    {
        public List<Tile> TilesPrefabs;

        public GameObject GetTileByModel(TileModel model)
        {
            return TilesPrefabs.Find(x => x.Model == model).gameObject;
        }

        public GameObject GetTileBySize(int size)
        {
            var tuples = TilesPrefabs.FindAll(x => x.Size == size);
            int randomIndex = UnityEngine.Random.Range(0, tuples.Count);
            return tuples[randomIndex].gameObject;
        }
    }
}
