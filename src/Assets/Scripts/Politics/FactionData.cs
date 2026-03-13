using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Politics
{
    [System.Serializable]
    public class FactionData
    {
        public FactionType type;
        public string displayName;
        public Color factionColor = Color.white;

        [Range(0, 100)]
        public float loyalty = 50f;
        public float supportBase = 0f;

        public FactionLeader leader;
        public List<Demand> activeDemands = new List<Demand>();

        // Key: FactionType, Value: Relation value (-100 to 100)
        public Dictionary<FactionType, float> relations = new Dictionary<FactionType, float>();

        // Satisfaction from economic needs (0-100)
        public float needsSatisfaction = 50f;
    }
}
