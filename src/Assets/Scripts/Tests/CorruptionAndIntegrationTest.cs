using UnityEngine;
using CaudilloBay.Core;
using CaudilloBay.World;

namespace CaudilloBay.Tests
{
    public class CorruptionAndIntegrationTest : MonoBehaviour
    {
        void Start()
        {
            RunTests();
        }

        public void RunTests()
        {
            Debug.Log("Starting CorruptionAndIntegration Tests...");

            // 1. Setup Corruption
            GameObject go = new GameObject("CorruptionManager");
            CorruptionManager corruption = go.AddComponent<CorruptionManager>();
            corruption.blackMarketMoney = 500f;

            // 2. Setup Stats with Abstract Pool
            GameObject statsGo = new GameObject("StatsManager");
            StatsManager stats = statsGo.AddComponent<StatsManager>();
            stats.AddResource("aid", 1000f);

            stats.RefreshStats();
            if (Mathf.Approximately(stats.GetResourceStockpile("aid"), 1000f)) Debug.Log("Test 1 Passed: Abstract resources preserved.");
            else Debug.LogError($"Test 1 Failed: Expected 1000, got {stats.GetResourceStockpile("aid")}");

            // 3. Shadow Economy Tax diversion
            GameObject econGo = new GameObject("EconomyManager");
            Economy.EconomyManager econ = econGo.AddComponent<Economy.EconomyManager>();
            corruption.globalCorruptionRate = 20f;

            // EconomyManager calls AddBlackMarketMoney(grossTax * rate * 0.5)
            // If grossTax = 1 * 50 = 50. CorruptionLoss = 50 * 0.2 = 10. ShadowContribution = 10 * 0.5 = 5.
            econ.ProcessMonthlyEconomy(new System.Collections.Generic.List<Building>());

            // Initial 500 + 5 = 505
            if (Mathf.Approximately(corruption.blackMarketMoney, 505f)) Debug.Log("Test 2 Passed: Corruption tax diversion works.");
            else Debug.LogError($"Test 2 Failed: Expected 505, got {corruption.blackMarketMoney}");

            Debug.Log("CorruptionAndIntegration Tests Complete.");
            Destroy(go);
            Destroy(statsGo);
            Destroy(econGo);
        }
    }
}
