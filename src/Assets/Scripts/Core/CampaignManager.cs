using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.Data;
using CaudilloBay.World;
using CaudilloBay.Economy;
using CaudilloBay.Politics;

namespace CaudilloBay.Core
{
    public class CampaignManager : MonoBehaviour
    {
        public static CampaignManager Instance { get; private set; }

        public CampaignMission currentMission;
        public List<CampaignObjective> activeObjectives = new List<CampaignObjective>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void LoadMission(CampaignMission mission)
        {
            currentMission = mission;
            activeObjectives.Clear();
            foreach (var objData in mission.objectives)
            {
                activeObjectives.Add(new CampaignObjective(objData));
            }

            // Apply starting treasury
            if (EconomyManager.Instance != null)
                EconomyManager.Instance.treasuryBalance = mission.initialTreasury;

            // Apply faction loyalty
            if (FactionManager.Instance != null)
            {
                foreach (var offset in mission.factionLoyaltyOffsets)
                {
                    var faction = FactionManager.Instance.factions.Find(f => f.type == offset.faction);
                    if (faction != null) faction.loyalty += offset.offset;
                }
            }
        }

        public void CheckObjectives()
        {
            if (currentMission == null) return;

            bool allComplete = true;
            foreach (var obj in activeObjectives)
            {
                UpdateObjectiveProgress(obj);
                if (!obj.isComplete) allComplete = false;
            }

            if (allComplete)
            {
                Debug.Log("Mission Accomplished: " + currentMission.missionName);
                // Trigger Win UI
            }
        }

        private void UpdateObjectiveProgress(CampaignObjective obj)
        {
            switch (obj.data.type)
            {
                case ObjectiveType.AccumulateWealth:
                    obj.currentValue = EconomyManager.Instance.treasuryBalance;
                    break;
                case ObjectiveType.ReachPopulation:
                    if (AI.PopulationManager.Instance != null)
                        obj.currentValue = AI.PopulationManager.Instance.allCitizens.Count;
                    break;
                // Add more cases here
            }

            if (obj.currentValue >= obj.data.targetValue)
            {
                obj.isComplete = true;
            }
        }
    }
}
