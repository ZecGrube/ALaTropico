using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.Politics;

namespace CaudilloBay.Data
{
    [CreateAssetMenu(fileName = "New Mission", menuName = "Caudillo Bay/Campaign Mission")]
    public class CampaignMission : ScriptableObject
    {
        public string missionId;
        public string missionName;
        [TextArea]
        public string description;

        [Header("Map Settings")]
        public int mapWidth = 50;
        public int mapHeight = 50;
        public float mapScale = 10f;

        [Header("Starting Conditions")]
        public float initialTreasury = 1000f;
        public List<InitialLoyalty> factionLoyaltyOffsets;

        [Header("Objectives")]
        public List<CampaignObjectiveData> objectives;

        [TextArea]
        public string briefingText;
        [TextArea]
        public string winText;
        [TextArea]
        public string loseText;
    }

    [System.Serializable]
    public struct InitialLoyalty
    {
        public FactionType faction;
        public float offset;
    }
}
