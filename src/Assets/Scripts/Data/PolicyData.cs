using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Data
{
    public enum PolicyCategory { Economic, Cultural, Environmental, Security }

    [CreateAssetMenu(fileName = "NewPolicy", menuName = "CaudilloBay/PolicyData")]
    public class PolicyData : ScriptableObject
    {
        public string policyId;
        public string policyName;
        [TextArea] public string description;
        public PolicyCategory category;

        public float maintenanceCost;

        [Header("Modifiers")]
        public float productionMultiplier = 1.0f;
        public float attractivenessBonus = 0f;
        public float pollutionModifier = 1.0f;
        public float crimeModifier = 1.0f;

        [Header("Faction Loyalty")]
        public List<Politics.FactionLoyaltyEffect> loyaltyEffects;
    }
}
