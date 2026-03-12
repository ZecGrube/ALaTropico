using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.World;
using CaudilloBay.Data;

namespace CaudilloBay.World
{
    public class StatsManager : MonoBehaviour
    {
        public static StatsManager Instance { get; private set; }

        public float totalUnemployment = 0f;
        public float averageHappiness = 50f;
        public Dictionary<string, float> globalStockpiles = new Dictionary<string, float>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void RefreshStats()
        {
            Building[] allBuildings = UnityEngine.Object.FindObjectsByType<Building>(FindObjectsSortMode.None);
            globalStockpiles.Clear();
            float happinessSum = 0;
            int residentialCount = 0;

            foreach (var b in allBuildings)
            {
                // Aggregate resources
                // Note: Simplified logic for demonstration
                if (b is ResidentialBuilding rb)
                {
                    happinessSum += rb.GetHappiness();
                    residentialCount++;
                }
            }

            if (residentialCount > 0)
                averageHappiness = happinessSum / residentialCount;
        }

        public float GetResourceStockpile(string resourceId)
        {
            return globalStockpiles.ContainsKey(resourceId) ? globalStockpiles[resourceId] : 0f;
        }
    }
}
