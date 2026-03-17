using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Data
{
    [CreateAssetMenu(fileName = "NewNeighbor", menuName = "CaudilloBay/NeighborState")]
    public class NeighborState : ScriptableObject
    {
        public string stateName;
        public float relations = 0f; // -100 to 100
        public float militaryStrength = 50f;
        public Politics.FactionType rulingFaction;

        [Header("Resources")]
        public List<ResourceType> exportResources;
        public List<ResourceType> importResources;

        [Header("Status")]
        public bool hasNonAggressionPact;
        public bool isAllied;
        public bool isSanctioned;
    }
}
