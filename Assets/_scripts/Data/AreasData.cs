using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;

namespace vgwb.lanoria
{
    public enum Region
    {
        undefined = 0,
        Region_1 = 1
    }

    public enum AreaId
    {
        undefined = 0,
        Area_1 = 1, // Costa Del Sol
        Area_2 = 2, // Serrania de Ronda
        Area_3 = 3, // Sierra de las nieves
        Area_4 = 4, // Guadallelba
        Area_5 = 5, // Comarqua de Antequera
        Area_6 = 6, // Valle del Guadal Horce
        Area_7 = 7, // Malaga
        Area_8 = 8, // Noroma
        Area_9 = 9 // Axarquia
    }

    [Serializable]
    public class AreaData
    {
        public AreaId Id;
        public string Title;
        public Color Color;
    }

    [CreateAssetMenu(menuName = "VGWB/Areas Data")]
    public class AreasData : ScriptableObject
    {
        public List<AreaData> Areas;

        public string GetSubregionNameByEnum(AreaId subregion)
        {
            return Areas.Find(x => x.Id == subregion).Title;
        }

        public Color GetSubregionColorByEnum(AreaId subregion)
        {
            return Areas.Find(x => x.Id == subregion).Color;
        }
    }
}
