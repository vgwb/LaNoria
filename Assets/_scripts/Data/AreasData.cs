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
        Subregion_1 = 1, // Costa Del Sol
        Subregion_2 = 2, // Serrania de Ronda
        Subregion_3 = 3, // Sierra de las nieves
        Subregion_4 = 4, // Guadallelba
        Subregion_5 = 5, // Comarqua de Antequera
        Subregion_6 = 6, // Valle del Guadal Horce
        Subregion_7 = 7, // Malaga
        Subregion_8 = 8, // Noroma
        Subregion_9 = 9 // Axarquia
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
