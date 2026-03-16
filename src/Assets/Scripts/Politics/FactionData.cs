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

        [System.Serializable]
        public struct RelationEntry
        {
            public FactionType faction;
            public float value;
        }
        public List<RelationEntry> relationsList = new List<RelationEntry>();

        // Satisfaction from economic needs (0-100)
        public float needsSatisfaction = 50f;
    }
}
