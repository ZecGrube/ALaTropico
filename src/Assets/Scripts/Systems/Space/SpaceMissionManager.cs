using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.Data;
using CaudilloBay.Economy;
using CaudilloBay.Core;

namespace CaudilloBay.Systems.Space
{
    public class SpaceMissionManager : MonoBehaviour
    {
        public static SpaceMissionManager Instance { get; private set; }

        [Header("Active Missions")]
        public List<SpaceMissionInstance> activeMissions = new List<SpaceMissionInstance>();
        public List<SatelliteTemplate> launchedSatellites = new List<SatelliteTemplate>();

        [Header("Space Race Milestones")]
        public bool firstSatellite = false;
        public bool firstHumanInOrbit = false;
        public bool firstMoonLanding = false;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void ScheduleMission(SpaceMissionTemplate template)
        {
            if (activeMissions.Count >= 2) return; // Limit concurrent launches

            // Resources taken upfront
            bool hasResources = true;
            foreach (var cost in template.requiredResources)
            {
                if (EconomyManager.Instance != null && !World.StatsManager.Instance.globalStockpiles.ContainsKey(cost.resourceType.resourceId) || World.StatsManager.Instance.globalStockpiles[cost.resourceType.resourceId] < cost.amount)
                    hasResources = false;
            }

            if (hasResources)
            {
                foreach (var cost in template.requiredResources)
                {
                    // Placeholder: Deduct from global inventory
                }
                activeMissions.Add(new SpaceMissionInstance { template = template });
                Debug.Log($"[SpaceMissionManager] Scheduled mission: {template.missionName}");
            }
        }

        public void ProcessMonthlyUpdate()
        {
            for (int i = activeMissions.Count - 1; i >= 0; i--)
            {
                var mission = activeMissions[i];
                if (mission.isLaunched) continue;

                mission.currentProgress += 1f;
                if (mission.currentProgress >= mission.template.preparationTime)
                {
                    LaunchMission(mission);
                    activeMissions.RemoveAt(i);
                }
            }
        }

        private void LaunchMission(SpaceMissionInstance mission)
        {
            mission.isLaunched = true;
            float roll = Random.Range(0, 100f);

            if (roll <= mission.template.baseSuccessChance)
            {
                mission.isSuccessful = true;
                CompleteMission(mission);
            }
            else
            {
                mission.isSuccessful = false;
                Debug.Log($"[SpaceMissionManager] LAUNCH FAILURE: {mission.template.missionName}");
                if (GlobalInfluenceManager.Instance != null) GlobalInfluenceManager.Instance.AddPrestige(-15f);
            }
        }

        private void CompleteMission(SpaceMissionInstance mission)
        {
            Debug.Log($"[SpaceMissionManager] SUCCESSFUL LAUNCH: {mission.template.missionName}");

            if (GlobalInfluenceManager.Instance != null)
            {
                GlobalInfluenceManager.Instance.AddPrestige(mission.template.prestigeReward);
                GlobalInfluenceManager.Instance.globalInfluence += mission.template.influenceReward;
            }

            if (mission.template.satelliteToDeploy != null)
            {
                DeploySatellite(mission.template.satelliteToDeploy);
            }

            // Check Milestones
            if (mission.template.type == SpaceMissionType.Satellite && !firstSatellite)
            {
                firstSatellite = true;
                TriggerMilestone("First Satellite in Orbit!");
            }
            else if (mission.template.type == SpaceMissionType.CrewedOrbit && !firstHumanInOrbit)
            {
                firstHumanInOrbit = true;
                TriggerMilestone("First Human in Space!");
            }
        }

        private void DeploySatellite(SatelliteTemplate satellite)
        {
            launchedSatellites.Add(satellite);
            foreach (var mod in satellite.activeModifiers)
            {
                if (ModifierManager.Instance != null)
                    ModifierManager.Instance.ApplyModifier(mod, -1); // Permanent
            }
            Debug.Log($"[SpaceMissionManager] Satellite {satellite.satelliteName} active.");
        }

        private void TriggerMilestone(string message)
        {
            Debug.Log($"[SpaceMissionManager] MILESTONE ACHIEVED: {message}");
            if (GlobalInfluenceManager.Instance != null)
            {
                GlobalInfluenceManager.Instance.AddPrestige(50f);
                GlobalInfluenceManager.Instance.globalInfluence += 200f;
            }
        }
    }
}
