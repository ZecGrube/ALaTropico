using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.World;
using CaudilloBay.Data;
using CaudilloBay.Economy;

namespace CaudilloBay.World
{
    public class StatsManager : MonoBehaviour
    {
        public static StatsManager Instance { get; private set; }

        public float totalUnemployment = 0f;
        public float averageHappiness = 50f;
        public float globalPollution = 0f;
        public Dictionary<string, float> globalStockpiles = new Dictionary<string, float>();

        private List<Building> _trackedBuildings = new List<Building>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
#if !UNITY_EDITOR
            // In release, the monthly tick handles this via FactionManager
            // but we keep this as a fallback if needed
#endif
        }

        public void RegisterBuilding(Building b)
        {
            if (!_trackedBuildings.Contains(b))
            {
                _trackedBuildings.Add(b);
            }
        }

        public void UnregisterBuilding(Building b)
        {
            if (_trackedBuildings.Contains(b))
            {
                _trackedBuildings.Remove(b);
            }
        }

        public void RefreshStats()
        {
            globalStockpiles.Clear();
            float happinessSum = 0;
            float pollutionSum = 0;
            int residentialCount = 0;

            foreach (var b in _trackedBuildings)
            {
                if (b == null) continue;

                pollutionSum += b.pollutionOutput;

                // Aggregate inventories into global stockpiles
                if (b.inventory != null)
                {
                    foreach (var resourceId in b.inventory.GetStoredResourceIds())
                    {
                        float amount = b.inventory.GetAmountById(resourceId);
                        if (globalStockpiles.ContainsKey(resourceId))
                            globalStockpiles[resourceId] += amount;
                        else
                            globalStockpiles.Add(resourceId, amount);
                    }
                }

                if (b is ResidentialBuilding rb)
                {
                    happinessSum += rb.GetHappiness();
                    residentialCount++;
                }
            }

            globalPollution = Mathf.Clamp(pollutionSum, 0, 100);

            if (residentialCount > 0)
                averageHappiness = happinessSum / residentialCount;
        }

        public float GetResourceStockpile(string resourceId)
        {
            return globalStockpiles.ContainsKey(resourceId) ? globalStockpiles[resourceId] : 0f;
        }

        public List<Building> GetTrackedBuildings()
        {
            return _trackedBuildings;
        }
    }
}
