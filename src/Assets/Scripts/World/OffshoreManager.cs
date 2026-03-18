using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.World
{
    public class OffshoreManager : MonoBehaviour
    {
        public static OffshoreManager Instance { get; private set; }

        public List<OffshoreDeposit> allDeposits = new List<OffshoreDeposit>();
        public List<OffshorePlatform> activePlatforms = new List<OffshorePlatform>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void GenerateInitialDeposits(int width, int height)
        {
            // Simple Perlin-based generation for sea floor resources
            for (int x = 0; x < width; x += 10)
            {
                for (int y = 0; y < height; y += 10)
                {
                    float noise = Mathf.PerlinNoise(x * 0.1f, y * 0.1f);
                    if (noise > 0.7f)
                    {
                        var dep = new OffshoreDeposit("oil_crude", new Vector2(x, y), Random.Range(50, 500), Random.Range(5000, 20000));
                        allDeposits.Add(dep);
                    }
                }
            }
            Debug.Log($"Generated {allDeposits.Count} offshore deposits.");
        }

        public void ScanArea(Vector2 center, float radius)
        {
            foreach (var dep in allDeposits)
            {
                if (!dep.isDiscovered && Vector2.Distance(center, dep.position) <= radius)
                {
                    dep.isDiscovered = true;
                    Debug.Log($"Discovered new {dep.resourceId} deposit at {dep.position}!");
                }
            }
        }

        public void MonthlyUpdate()
        {
            foreach (var platform in activePlatforms)
            {
                if (platform.IsFunctional())
                {
                    platform.PerformExtraction();
                    CheckForAccidents(platform);
                }
            }
        }

        private void CheckForAccidents(OffshorePlatform platform)
        {
            if (Random.value < platform.riskFactor)
            {
                TriggerSpill(platform);
            }
        }

        private void TriggerSpill(OffshorePlatform platform)
        {
            Debug.LogWarning($"OIL SPILL! Disaster at {platform.displayName}!");

            if (Core.StatsManager.Instance != null)
            {
                Core.StatsManager.Instance.globalPollution += 500f;
                // Add specific "Oil Spill" modifier
                if (Core.ModifierManager.Instance != null)
                {
                    // Assume we have a Data.ModifierData for oil spill impacts
                    Debug.Log("Oil Spill: Tourism attractiveness penalized for 12 months.");
                }
            }

            if (Core.HealthManager.Instance != null)
            {
                Core.HealthManager.Instance.globalHealthLevel -= 5f;
            }
        }
    }
}
