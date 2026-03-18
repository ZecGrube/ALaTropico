using UnityEngine;
using CaudilloBay.World;
using CaudilloBay.Economy;
using System.Collections.Generic;

namespace CaudilloBay.Tests
{
    public class OffshoreSystemTest : MonoBehaviour
    {
        public void Start()
        {
            RunTests();
        }

        public void RunTests()
        {
            Debug.Log("--- Starting Offshore System Tests ---");
            TestDepositDiscovery();
            TestPlatformExtraction();
            TestSpillConsequences();
            Debug.Log("--- Offshore System Tests Complete ---");
        }

        private void TestDepositDiscovery()
        {
            GameObject mgrGO = new GameObject("OffshoreManager");
            var mgr = mgrGO.AddComponent<OffshoreManager>();

            var dep = new OffshoreDeposit("oil", new Vector2(50, 50), 100, 1000);
            mgr.allDeposits.Add(dep);

            mgr.ScanArea(new Vector2(50, 50), 10);

            if (dep.isDiscovered)
                Debug.Log("[PASS] Deposit Discovery: Scan revealed hidden deposit.");
            else
                Debug.LogError("[FAIL] Deposit Discovery: Scan failed to reveal deposit.");
        }

        private void TestPlatformExtraction()
        {
            GameObject mgrGO = new GameObject("OffshoreManager");
            var mgr = mgrGO.AddComponent<OffshoreManager>();

            GameObject platGO = new GameObject("DrillingPlatform");
            var platform = platGO.AddComponent<DrillingPlatform>();
            platform.extractionRate = 100;

            var dep = new OffshoreDeposit("oil", Vector2.zero, 100, 1000);
            platform.targetDeposit = dep;

            platform.PerformExtraction();

            if (dep.quantity == 900)
                Debug.Log("[PASS] Platform Extraction: Resources subtracted correctly.");
            else
                Debug.LogError("[FAIL] Platform Extraction: Incorrect remaining quantity: " + dep.quantity);
        }

        private void TestSpillConsequences()
        {
            GameObject mgrGO = new GameObject("OffshoreManager");
            var mgr = mgrGO.AddComponent<OffshoreManager>();

            GameObject platGO = new GameObject("FaultyPlatform");
            var platform = platGO.AddComponent<DrillingPlatform>();
            platform.riskFactor = 1.0f; // Force accident

            GameObject statsGO = new GameObject("StatsManager");
            var stats = statsGO.AddComponent<CaudilloBay.Core.StatsManager>();
            float initialPollution = stats.globalPollution;

            mgr.activePlatforms.Add(platform);
            mgr.MonthlyUpdate();

            if (stats.globalPollution > initialPollution)
                Debug.Log("[PASS] Spill Consequences: Pollution increased after accident.");
            else
                Debug.LogError("[FAIL] Spill Consequences: Pollution remains stable.");
        }
    }
}
