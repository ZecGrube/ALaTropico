using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.AI;

namespace CaudilloBay.Core
{
    public class HealthManager : MonoBehaviour
    {
        public static HealthManager Instance { get; private set; }

        [Header("Global Health State")]
        public float globalHealthLevel = 80f;
        public float monthlyHealthDecay = -1.0f;

        private List<World.Building> medicalBuildings = new List<World.Building>();
        private bool isEpidemicActive = false;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void ProcessMonthlyHealth()
        {
            CalculateHealthDecay();
            UpdateCitizenHealth();
            CheckForEpidemic();
            CalculateAverageHealth();
        }

        private void CalculateHealthDecay()
        {
            float baseDecay = -1.0f;

            // Pollution penalty
            if (StatsManager.Instance != null)
                baseDecay -= StatsManager.Instance.globalPollution * 0.05f;

            // Ecology bonus
            float ecologyBonus = 0f;
            if (Ecology.EcosystemManager.Instance != null)
            {
                ecologyBonus = Ecology.EcosystemManager.Instance.GetTotalBiodiversity() * 0.05f;
            }

            // Medical bonus
            float medicalBonus = 0f;
            foreach (var b in medicalBuildings)
            {
                if (b.IsFunctional())
                {
                    if (b is World.Clinic) medicalBonus += 2.0f;
                    else if (b is World.Hospital) medicalBonus += 5.0f;
                }
            }

            monthlyHealthDecay = baseDecay + medicalBonus + ecologyBonus;
            if (isEpidemicActive) monthlyHealthDecay -= 10.0f;
        }

        private void UpdateCitizenHealth()
        {
            if (PopulationManager.Instance == null) return;

            foreach (var citizen in PopulationManager.Instance.allCitizens)
            {
                citizen.UpdateHealth(monthlyHealthDecay);
            }
        }

        private void CheckForEpidemic()
        {
            if (isEpidemicActive) return;

            if (globalHealthLevel < 40f)
            {
                float chance = (40f - globalHealthLevel) * 0.5f;
                if (Random.Range(0f, 100f) < chance)
                {
                    StartEpidemic();
                }
            }
        }

        public void StartEpidemic()
        {
            isEpidemicActive = true;
            Debug.Log("An epidemic has started!");
            // Notify EventManager etc.
        }

        public void StopEpidemic()
        {
            isEpidemicActive = false;
            Debug.Log("The epidemic has ended.");
        }

        private void CalculateAverageHealth()
        {
            if (PopulationManager.Instance == null || PopulationManager.Instance.allCitizens.Count == 0) return;

            float sum = 0f;
            foreach (var citizen in PopulationManager.Instance.allCitizens)
            {
                sum += citizen.health;
            }
            globalHealthLevel = sum / PopulationManager.Instance.allCitizens.Count;
        }

        public void RegisterBuilding(World.Building building)
        {
            if (!medicalBuildings.Contains(building)) medicalBuildings.Add(building);
        }

        public void UnregisterBuilding(World.Building building)
        {
            medicalBuildings.Remove(building);
        }
    }
}
