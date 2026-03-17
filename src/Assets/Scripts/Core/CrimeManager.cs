using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.AI;
using CaudilloBay.World;

namespace CaudilloBay.Core
{
    public class CrimeManager : MonoBehaviour
    {
        public static CrimeManager Instance { get; private set; }

        [Header("Global Crime State")]
        public float globalCrimeRate = 0f;
        public float policeEffectiveness = 0f;

        private List<PoliceStation> policeStations = new List<PoliceStation>();
        private List<Prison> prisons = new List<Prison>();
        private int courthouseCount = 0;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void ProcessMonthlyCrime()
        {
            CalculatePoliceEffectiveness();
            CalculateCrimeRate();
            ApplyCrimeEffects();
        }

        private void CalculatePoliceEffectiveness()
        {
            float total = 0f;
            foreach (var ps in policeStations)
            {
                if (ps.IsFunctional())
                    total += ps.effectiveness;
            }

            foreach (var p in prisons)
            {
                if (p.IsFunctional())
                    total += 10f; // Prisons give a flat global bonus
            }

            // Courthouses boost police stations
            if (courthouseCount > 0)
            {
                total *= (1.0f + (courthouseCount * 0.2f));
            }

            policeEffectiveness = total;
        }

        private void CalculateCrimeRate()
        {
            float unemploymentFactor = PopulationManager.Instance != null ? PopulationManager.Instance.unemploymentRate * 50f : 0f;
            float povertyFactor = (100f - PopulationManager.Instance.averageSatisfaction) * 0.3f;

            // Factor for criminogenic buildings (casinos etc)
            float criminogenicFactor = CalculateCriminogenicFactor();

            float educationFactor = EducationManager.Instance != null ? EducationManager.Instance.globalEducationLevel * 0.2f : 0f;

            globalCrimeRate = Mathf.Clamp(unemploymentFactor + povertyFactor + criminogenicFactor - policeEffectiveness - educationFactor, 0f, 100f);

            Debug.Log($"Monthly Crime Rate: {globalCrimeRate} (Unemp: {unemploymentFactor}, Poverty: {povertyFactor}, Police: {policeEffectiveness})");
        }

        private float CalculateCriminogenicFactor()
        {
            float factor = 0f;
            if (StatsManager.Instance != null)
            {
                foreach (var b in StatsManager.Instance.GetTrackedBuildings())
                {
                    if (b.data.buildingId == "casino") factor += 5f;
                    if (b.data.buildingId == "nightclub") factor += 3f;
                }
            }
            return factor;
        }

        private void ApplyCrimeEffects()
        {
            // Update tourism safety factor
            if (Economy.TouristManager.Instance != null)
            {
                float safetyFactor = Mathf.Clamp01(1.0f - (globalCrimeRate / 100f));
                // Note: EconomyManager or TouristManager itself handles the actual update,
                // but we can set the state here if needed.
            }

            // Loyalty impacts
            if (Politics.FactionManager.Instance != null)
            {
                if (globalCrimeRate > 40f)
                {
                    // Capitalists and Nationalists dislike high crime
                    // FactionManager.Instance.ModifyRelations(Politics.FactionType.Capitalists, Politics.FactionType.Nationalists, -0.05f * (globalCrimeRate - 40f));
                }
            }
        }

        public void RegisterPoliceStation(PoliceStation ps) { if (!policeStations.Contains(ps)) policeStations.Add(ps); }
        public void UnregisterPoliceStation(PoliceStation ps) { policeStations.Remove(ps); }

        public void RegisterPrison(Prison p) { if (!prisons.Contains(p)) prisons.Add(p); }
        public void UnregisterPrison(Prison p) { prisons.Remove(p); }

        public void RegisterCourthouse() { courthouseCount++; }
        public void UnregisterCourthouse() { courthouseCount--; }

        public float GetLocalCrimeRate(Vector3 position)
        {
            // If position is within a police station's radius, crime is lower
            foreach (var ps in policeStations)
            {
                if (ps.IsFunctional() && Vector3.Distance(position, ps.transform.position) <= ps.coverageRadius)
                {
                    return globalCrimeRate * 0.5f; // 50% reduction in patrolled areas
                }
            }
            return globalCrimeRate;
        }
    }
}
