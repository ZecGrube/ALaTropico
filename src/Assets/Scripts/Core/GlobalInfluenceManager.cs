using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.Economy;
using CaudilloBay.Politics;
using CaudilloBay.World;

namespace CaudilloBay.Core
{
    public class GlobalInfluenceManager : MonoBehaviour
    {
        public static GlobalInfluenceManager Instance { get; private set; }

        [Header("Global Metrics")]
        [Range(0f, 1000f)]
        public float globalInfluence = 0f;

        [Range(0f, 100f)]
        public float internationalPrestige = 50f;

        [Header("Influence Components")]
        public float economicWeight = 0f;
        public float militaryWeight = 0f;
        public float culturalWeight = 0f;
        public float diplomaticWeight = 0f;
        public float scientificWeight = 0f;

        [Header("Victory State")]
        public int crisesSolved = 0;
        public bool isWorldLeader = false;
        public float yearsOfPeace = 0f;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            // Monthly calculation logic
            // In a real scenario, this would be triggered by a monthly tick
        }

        public void CalculateInfluence()
        {
            // 1. Economic Weight (GDP, Trade Balance)
            if (EconomyManager.Instance != null)
            {
                // Simple normalized GDP/Trade weight
                economicWeight = Mathf.Clamp(EconomyManager.Instance.GetGDP() / 1000f, 0, 200f);
            }

            // 2. Military Weight (Strength + Nuclear Status)
            if (MilitaryManager.Instance != null)
            {
                militaryWeight = Mathf.Clamp(MilitaryManager.Instance.totalMilitaryStrength / 5f, 0, 150f);

                if (Systems.Nuclear.NuclearManager.Instance != null && Systems.Nuclear.NuclearManager.Instance.arsenal.Count > 0)
                {
                    militaryWeight += 50f; // Nuclear status bonus
                }
            }

            // 3. Cultural Weight (Tourism, UNESCO)
            if (CultureManager.Instance != null)
            {
                culturalWeight = CultureManager.Instance.globalCultureLevel * 2f;
                if (UNESCOManager.Instance != null)
                {
                    culturalWeight += UNESCOManager.Instance.GetUNESCOCount() * 20f;
                }
            }

            // 4. Diplomatic Weight (Alliances, Orgs)
            if (GlobalMapManager.Instance != null)
            {
                diplomaticWeight = GlobalMapManager.Instance.alliedSuperpowers.Count * 50f;
                if (OrganizationManager.Instance != null)
                {
                    diplomaticWeight += OrganizationManager.Instance.activeMemberships.Count * 30f;
                }
            }

            // 5. Scientific Weight (Technology)
            if (TechnologyManager.Instance != null)
            {
                scientificWeight = TechnologyManager.Instance.GetResearchedTechIds().Count * 5f;
            }

            // Total Weighted Sum
            globalInfluence = economicWeight + militaryWeight + culturalWeight + diplomaticWeight + scientificWeight;
            globalInfluence = Mathf.Clamp(globalInfluence, 0, 1000f);

            Debug.Log($"[GlobalInfluenceManager] Influence recalculated: {globalInfluence:F1}/1000");
        }

        public void AddPrestige(float amount)
        {
            internationalPrestige = Mathf.Clamp(internationalPrestige + amount, 0, 100f);
            Debug.Log($"[GlobalInfluenceManager] Prestige changed to: {internationalPrestige:F1}");
        }

        public void SolveCrisis()
        {
            crisesSolved++;
            AddPrestige(10f);
            Debug.Log($"[GlobalInfluenceManager] Global crisis solved. Total solved: {crisesSolved}");
        }

        public Dictionary<string, float> GetInfluenceBreakdown()
        {
            return new Dictionary<string, float>
            {
                { "Economic", economicWeight },
                { "Military", militaryWeight },
                { "Cultural", culturalWeight },
                { "Diplomatic", diplomaticWeight },
                { "Scientific", scientificWeight }
            };
        }
    }
}
