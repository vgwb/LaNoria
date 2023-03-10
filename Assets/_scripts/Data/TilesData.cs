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

    public static class TileUtils
    {
        public static TileModel GetRandomTileModelByLenght(int lenght)
        {
            var random = new System.Random();
            if (lenght == 2) {
                return TileModel.P2A;
            } else if (lenght == 3) {
                var models3 = new List<TileModel> { TileModel.P3A, TileModel.P3B, TileModel.P3B_alt, TileModel.P3C };
                return models3[random.Next(models3.Count)];
            } else if (lenght == 4) {
                var models4 = new List<TileModel> { TileModel.P4A, TileModel.P4B, TileModel.P4B_alt,
                TileModel.P4C, TileModel.P4D, TileModel.P4D_alt, TileModel.P4E,
                TileModel.P4F, TileModel.P4F_alt };
                return models4[random.Next(models4.Count)];
            } else {
                return TileModel.P2A;
            }
        }
    }


    [CreateAssetMenu(menuName = "VGWB/Tiles Data")]
    public class TilesData : ScriptableObject
    {
        public List<Tile> TilesPrefabs;

        public GameObject GetTileByModel(TileModel model)
        {
            Debug.Log("GetTileByModel + " + model);
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
