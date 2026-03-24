using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.World;
using CaudilloBay.Politics;

namespace CaudilloBay.AI
{
    public class PopulationManager : MonoBehaviour
    {
        public static PopulationManager Instance { get; private set; }

        public GameObject citizenPrefab;
        public List<Citizen> allCitizens = new List<Citizen>();

        [Header("Stats")]
        public float averageSatisfaction = 50f;
        public float unemploymentRate = 0f;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void RegisterResidential(ResidentialBuilding building, int capacity)
        {
            for (int i = 0; i < capacity; i++)
            {
                SpawnCitizen(building);
            }
        }

        private void SpawnCitizen(ResidentialBuilding home)
        {
            GameObject go = Instantiate(citizenPrefab, home.transform.position, Quaternion.identity);
            Citizen citizen = go.GetComponent<Citizen>();
            citizen.home = home;
            citizen.id = allCitizens.Count;
            allCitizens.Add(citizen);

            home.residents.Add(citizen);
        }

        public void UpdateGlobalStats()
        {
            if (allCitizens.Count == 0) return;

            float satSum = 0;
            int unemployedCount = 0;

            foreach (var c in allCitizens)
            {
                satSum += c.satisfaction;
                if (c.workplace == null) unemployedCount++;
            }

            averageSatisfaction = satSum / allCitizens.Count;

            // Automation displacement penalty
            float automationPenalty = 0f;
            if (Systems.Automation.AutomationManager.Instance != null)
            {
                automationPenalty = Systems.Automation.AutomationManager.Instance.automationInducedUnemployment;
            }

            unemploymentRate = ((float)unemployedCount / allCitizens.Count) + automationPenalty;

            // Handle Demographic cycle monthly
            foreach (var c in allCitizens)
            {
                // Aging: every 12 months in logic
                if (Random.value < 0.08f) c.age++;

                // Income distribution
                if (c.workplace != null) c.personalWealth += c.salary;
            }

            CheckForDemographics();
            CheckForEmigration();
        }

        private void CheckForDemographics()
        {
            // Simplified birth logic
            int potentialMothers = allCitizens.FindAll(c => c.gender == Gender.Female && c.age > 18 && c.age < 45).Count;
            if (Random.value < (potentialMothers * 0.001f))
            {
                // Find a house with capacity for a new citizen
                foreach (var b in StatsManager.Instance.GetTrackedBuildings())
                {
                    if (b is ResidentialBuilding rb && rb.residents.Count < rb.capacity)
                    {
                        SpawnCitizen(rb);
                        break;
                    }
                }
            }
        }

        private void CheckForEmigration()
        {
            for (int i = allCitizens.Count - 1; i >= 0; i--)
            {
                Citizen c = allCitizens[i];
                if (c.satisfaction < 10f) // Very unhappy
                {
                    Emigrate(c);
                }
            }
        }

        public void Emigrate(Citizen citizen)
        {
            Debug.Log($"Citizen {citizen.id} is emigrating due to low satisfaction!");
            RemoveCitizen(citizen);
        }

        public void Die(Citizen citizen)
        {
            Debug.Log($"Citizen {citizen.id} has died!");
            RemoveCitizen(citizen);
        }

        private void RemoveCitizen(Citizen citizen)
        {
            if (citizen.home != null)
                citizen.home.residents.Remove(citizen);

            allCitizens.Remove(citizen);
            Destroy(citizen.gameObject);
        }
    }
}
