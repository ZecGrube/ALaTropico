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
            unemploymentRate = (float)unemployedCount / allCitizens.Count;

            // Notify politics
            if (FactionManager.Instance != null)
            {
                // In a real case, update specific factions
            }

            CheckForEmigration();
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

            if (citizen.home != null)
                citizen.home.residents.Remove(citizen);

            allCitizens.Remove(citizen);
            Destroy(citizen.gameObject);
        }
    }
}
