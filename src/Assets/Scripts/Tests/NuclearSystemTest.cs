using UnityEngine;
using CaudilloBay.Systems.Nuclear;
using CaudilloBay.World;
using System.Collections.Generic;

namespace CaudilloBay.Tests
{
    public class NuclearSystemTest : MonoBehaviour
    {
        public void Start()
        {
            RunTests();
        }

        public void RunTests()
        {
            Debug.Log("--- Starting Nuclear System Test ---");

            // Setup Managers
            GameObject go = new GameObject("NuclearManager");
            var manager = go.AddComponent<NuclearManager>();

            GameObject statsGo = new GameObject("StatsManager");
            var stats = statsGo.AddComponent<StatsManager>();

            // 1. Test Warhead Production
            manager.OnBuildingEnrichmentCompleted();
            if (manager.arsenal.Count == 1)
                Debug.Log("[PASS] Warhead production.");
            else
                Debug.LogError("[FAIL] Warhead production.");

            // 2. Test Delivery System Loading
            var icbm = new ICBM();
            icbm.loadedWarhead = manager.arsenal[0];
            manager.systems.Add(icbm);
            if (icbm.IsLoaded)
                Debug.Log("[PASS] Delivery system loading.");
            else
                Debug.LogError("[FAIL] Delivery system loading.");

            // 3. Test Deterrence Calculation
            manager.CalculateDeterrence();
            if (manager.currentDeterrence > 0)
                Debug.Log($"[PASS] Deterrence calculation: {manager.currentDeterrence}");
            else
                Debug.LogError("[FAIL] Deterrence calculation.");

            // 4. Test Nuclear Test
            float initialTension = manager.nuclearTension;
            manager.ConductTest("atmospheric");
            if (manager.nuclearTension > initialTension)
                Debug.Log($"[PASS] Nuclear test tension increase: {manager.nuclearTension}");
            else
                Debug.LogError("[FAIL] Nuclear test tension increase.");

            // 5. Test Strike & Radiation
            Vector2 target = new Vector2(100, 100);
            manager.LaunchStrike("EnemyState", target, icbm);

            var zone = FindFirstObjectByType<RadiationZone>();
            if (zone != null)
                Debug.Log("[PASS] Radiation zone creation.");
            else
                Debug.LogError("[FAIL] Radiation zone creation.");

            Debug.Log("--- Nuclear System Test Complete ---");
        }
    }
}
