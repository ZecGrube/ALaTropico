using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Data
{
    public enum QuestType { BuildBuilding, ReachPopulation, ResourceThreshold, ResearchTech, FactionLoyalty }

    [CreateAssetMenu(fileName = "NewQuest", menuName = "CaudilloBay/Quest")]
    public class Quest : ScriptableObject
    {
        public string questId;
        public string questName;
        [TextArea] public string description;
        public QuestType type;
        public string targetId; // ID of building, resource, or faction
        public int requiredAmount;

        public bool isMainQuest;
        public List<QuestReward> rewards;
    }

    [System.Serializable]
    public class QuestReward
    {
        public enum RewardType { Resources, Technology, Mandate, Legitimacy }
        public RewardType type;
        public string data; // Resource ID, Tech ID, etc.
        public float amount;
    }
}
