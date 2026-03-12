using UnityEngine;

namespace CaudilloBay.Politics
{
    public enum DemandType
    {
        BuildBuilding,
        EnactDecree,
        ReachResourceAmount
    }

    [System.Serializable]
    public class Demand
    {
        public string title;
        public string description;
        public DemandType type;
        public string targetId;
        public int requiredValue;
        public int rewardLoyalty;
        public int penaltyLoyalty;
        public float timeLimit; // 0 for infinite
        public float timer;
    }
}
