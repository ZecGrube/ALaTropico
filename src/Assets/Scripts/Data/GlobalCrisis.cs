using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Data
{
    public enum CrisisType { Economic, Ecological, Health, Conflict }

    [CreateAssetMenu(fileName = "New Global Crisis", menuName = "Caudillo Bay/Diplomacy/Global Crisis")]
    public class GlobalCrisisTemplate : ScriptableObject
    {
        public string crisisId;
        public string crisisName;
        [TextArea]
        public string description;
        public CrisisType type;

        [Header("Requirements to Solve")]
        public float requiredFunds = 0f;
        public List<ResourceCost> requiredResources = new List<ResourceCost>();
        public int requiredTechLevel = 0;
        public float requiredInfluence = 0f;

        [Header("Rewards")]
        public float prestigeReward = 10f;
        public float influenceReward = 50f;
        public List<ModifierData> resolutionModifiers = new List<ModifierData>();
    }

    [System.Serializable]
    public class GlobalCrisisInstance
    {
        public GlobalCrisisTemplate template;
        public float currentProgress = 0f;
        public bool isSolved = false;
        public float remainingDuration = 12f; // Months
    }
}
