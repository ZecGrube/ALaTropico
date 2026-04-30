using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.Data;

namespace CaudilloBay.Colonization
{
    [System.Serializable]
    public class Colony
    {
        public string islandId;
        public string name;
        public int population;
        public float loyalty = 75f;
        public Economy.Inventory resourceStocks = new Economy.Inventory { maxWeight = 1000f };
        public List<string> builtBuildingIds = new List<string>();
        public ColonyPolicy autoPolicy = ColonyPolicy.Balanced;

        public bool isClaimed = false;
        public string ownerName = "Neutral";
    }

    public enum ColonyPolicy
    {
        Balanced,
        ResourceExtraction,
        AgriculturalFocus,
        RapidGrowth
    }
}
