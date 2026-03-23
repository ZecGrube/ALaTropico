using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Data
{
    public enum SpaceMissionType { Satellite, CrewedOrbit, LunarLanding, MarsExpedition }

    [CreateAssetMenu(fileName = "New Space Mission", menuName = "Caudillo Bay/Space/Space Mission")]
    public class SpaceMissionTemplate : ScriptableObject
    {
        public string missionId;
        public string missionName;
        [TextArea]
        public string description;
        public SpaceMissionType type;

        [Header("Requirements")]
        public List<ResourceCost> requiredResources = new List<ResourceCost>();
        public float preparationTime = 6f; // Months
        public int requiredTechLevel = 4;
        public float baseSuccessChance = 90f;

        [Header("Rewards")]
        public float prestigeReward = 10f;
        public float influenceReward = 50f;
        public SatelliteTemplate satelliteToDeploy;
    }

    public enum OrbitalFunction { Communication, Navigation, Reconnaissance, Science, Weather, Military }

    [CreateAssetMenu(fileName = "New Satellite", menuName = "Caudillo Bay/Space/Satellite")]
    public class SatelliteTemplate : ScriptableObject
    {
        public string satelliteId;
        public string satelliteName;
        public OrbitalFunction function;
        public List<ModifierData> activeModifiers = new List<ModifierData>();
    }

    [System.Serializable]
    public class SpaceMissionInstance
    {
        public SpaceMissionTemplate template;
        public float currentProgress = 0f;
        public bool isLaunched = false;
        public bool isSuccessful = false;
    }
}
