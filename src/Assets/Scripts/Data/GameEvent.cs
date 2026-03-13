using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Data
{
    [CreateAssetMenu(fileName = "New Game Event", menuName = "CaudilloBay/Game Event")]
    public class GameEvent : ScriptableObject
    {
        public string eventId;
        public string title;
        [TextArea] public string description;
        public Sprite eventImage;

        public float randomWeight = 1.0f;

        [Header("Conditions")]
        public float minLegitimacy = 0f;
        public int minMandate = 0;
        public List<FactionRequirement> factionRequirements = new List<FactionRequirement>();

        [Header("Choices")]
        public List<EventChoice> choices = new List<EventChoice>();
    }

    [System.Serializable]
    public class FactionRequirement
    {
        public Politics.FactionType factionType;
        public float minLoyalty;
    }

    [System.Serializable]
    public class EventChoice
    {
        public string choiceText;
        public string outcomeText;

        [Header("Immediate Effects")]
        public float legitimacyChange;
        public int mandateChange;
        public List<ResourceChange> resourceChanges = new List<ResourceChange>();

        [Header("Timed Modifiers")]
        public List<ModifierData> modifiers = new List<ModifierData>();
    }

    [System.Serializable]
    public class ResourceChange
    {
        public string resourceId;
        public float amount;
    }

    [System.Serializable]
    public class ModifierData
    {
        public string modifierId;
        public string displayName;
        public ModifierType type;
        public float value;
        public int durationMonths;
    }

    public enum ModifierType
    {
        ProductionEfficiency,
        CitizenHappiness,
        LogisticsSpeed,
        PollutionRate,
        ExportPrices
    }
}
