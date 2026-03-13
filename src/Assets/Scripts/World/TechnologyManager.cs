using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.Data;
using CaudilloBay.Politics;

namespace CaudilloBay.World
{
    public class TechnologyManager : MonoBehaviour
    {
        public static TechnologyManager Instance { get; private set; }

        public List<Technology> allTechnologies = new List<Technology>();
        private HashSet<string> researchedTechIds = new HashSet<string>();

        [Header("Research State")]
        public float currentResearchPoints = 0f;
        public Technology currentResearch;
        public float researchProgress = 0f;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Update()
        {
            if (currentResearch != null)
            {
                UpdateResearchProgress(Time.deltaTime);
            }
        }

        public void AddResearchPoints(float points)
        {
            currentResearchPoints += points;
        }

        public bool CanResearch(Technology tech)
        {
            if (researchedTechIds.Contains(tech.techId)) return false;

            foreach (var pre in tech.prerequisites)
            {
                if (!researchedTechIds.Contains(pre.techId)) return false;
            }

            return currentResearchPoints >= tech.researchCost;
        }

        public void StartResearch(Technology tech)
        {
            if (CanResearch(tech))
            {
                currentResearch = tech;
                researchProgress = 0f;
            }
        }

        private void UpdateResearchProgress(float deltaTime)
        {
            // Simplified: progression is based on time, but cost is subtracted at start or during
            researchProgress += deltaTime / (currentResearch.researchTime * 60f); // 60s per month assume

            if (researchProgress >= 1.0f)
            {
                CompleteResearch(currentResearch);
            }
        }

        private void CompleteResearch(Technology tech)
        {
            researchedTechIds.Add(tech.techId);
            currentResearchPoints -= tech.researchCost;
            currentResearch = null;
            researchProgress = 0f;

            // Apply loyalty effects
            foreach (var effect in tech.loyaltyEffects)
            {
                // In a real case, call FactionManager
                // FactionManager.Instance.ModifyLoyalty(effect.faction, effect.effect);
            }

            Debug.Log($"Research Completed: {tech.techName}");
        }

        public bool IsBuildingUnlocked(string buildingId)
        {
            // For now, if it's not explicitly in any tech's unlock list, it's considered free
            // If it is in an unlock list, check if that tech is researched
            bool foundInAnyTech = false;
            foreach (var tech in allTechnologies)
            {
                foreach (var prefab in tech.unlockedBuildings)
                {
                    if (prefab != null)
                    {
                        // Check against the building data ID if possible, or prefab name
                        Building b = prefab.GetComponent<Building>();
                        if (b != null && b.data != null && b.data.buildingId == buildingId)
                        {
                            foundInAnyTech = true;
                            if (IsResearched(tech.techId)) return true;
                        }
                    }
                }
            }
            return !foundInAnyTech;
        }

        public bool IsResearched(string techId)
        {
            return researchedTechIds.Contains(techId);
        }

        public List<string> GetResearchedTechIds()
        {
            return new List<string>(researchedTechIds);
        }

        public void LoadResearchedTechs(List<string> ids)
        {
            researchedTechIds.Clear();
            foreach (var id in ids) researchedTechIds.Add(id);
        }
    }
}
