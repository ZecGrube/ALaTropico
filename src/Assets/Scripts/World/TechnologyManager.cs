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
        private Dictionary<string, string> _buildingToTechCache = new Dictionary<string, string>();

        [Header("Research State")]
        public float currentResearchPoints = 0f;
        public Technology currentResearch;
        public float researchProgress = 0f;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            PopulateCache();
        }

        private void PopulateCache()
        {
            _buildingToTechCache.Clear();
            foreach (var tech in allTechnologies)
            {
                foreach (var prefab in tech.unlockedBuildings)
                {
                    if (prefab != null)
                    {
                        Building b = prefab.GetComponent<Building>();
                        if (b != null && b.data != null)
                        {
                            if (!_buildingToTechCache.ContainsKey(b.data.buildingId))
                                _buildingToTechCache.Add(b.data.buildingId, tech.techId);
                        }
                    }
                }
            }
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
            float multiplier = 1.0f;
            if (Core.EducationManager.Instance != null)
            {
                multiplier = 1.0f + (Core.EducationManager.Instance.globalEducationLevel / 100f);
            }
            currentResearchPoints += points * multiplier;
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
            if (FactionManager.Instance != null)
            {
                foreach (var effect in tech.loyaltyEffects)
                {
                    var faction = FactionManager.Instance.factions.Find(f => f.type == effect.faction);
                    if (faction != null)
                    {
                        faction.loyalty = Mathf.Clamp(faction.loyalty + effect.effect, 0, 100);
                    }
                }
            }

            Debug.Log($"Research Completed: {tech.techName}");
        }

        public bool IsBuildingUnlocked(string buildingId)
        {
            if (_buildingToTechCache.TryGetValue(buildingId, out string techId))
            {
                return IsResearched(techId);
            }
            return true; // Uncached buildings are free
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
