using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Data
{
    public enum ResolutionEffectType { TradeBonus, EnvironmentalProtection, Sanctions, Peacekeeping }

    [CreateAssetMenu(fileName = "New Resolution", menuName = "Caudillo Bay/Diplomacy/Resolution")]
    public class ResolutionTemplate : ScriptableObject
    {
        public string resolutionId;
        public string resolutionName;
        [TextArea]
        public string description;
        public ResolutionEffectType type;

        public float influenceCostToPropose = 50f;
        public float baseSupport = 30f; // Base chance to pass
        public float requiredVotes = 66f; // Percentage to pass

        public List<ModifierData> activeModifiers = new List<ModifierData>();
    }

    [System.Serializable]
    public class ResolutionInstance
    {
        public ResolutionTemplate template;
        public float currentSupport = 0f;
        public int forVotes = 0;
        public int againstVotes = 0;
        public bool isPassed = false;
        public float votingEndsInMonths = 3f;
    }
}
