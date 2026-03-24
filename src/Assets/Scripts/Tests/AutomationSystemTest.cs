using UnityEngine;
using CaudilloBay.Systems.Automation;
using CaudilloBay.AI;
using CaudilloBay.World;
using System.Collections.Generic;

namespace CaudilloBay.Tests
{
    public class AutomationSystemTest : MonoBehaviour
    {
        public void Start()
        {
            RunTests();
        }

        public void RunTests()
        {
            Debug.Log("--- Starting Automation System Test ---");

            // Setup
            GameObject mgrGo = new GameObject("AutomationManager");
            var manager = mgrGo.AddComponent<AutomationManager>();

            GameObject aiGo = new GameObject("AISystem");
            var ai = aiGo.AddComponent<AISystem>();

            GameObject popGo = new GameObject("PopulationManager");
            var pop = popGo.AddComponent<PopulationManager>();

            GameObject statsGo = new GameObject("StatsManager");
            var stats = statsGo.AddComponent<StatsManager>();

            // 1. Test Automation Displacement
            manager.globalAutomationLevel = 50f;
            manager.UpdateMetrics();
            pop.UpdateGlobalStats();

            if (pop.unemploymentRate > 0)
                Debug.Log($"[PASS] Automation-induced unemployment: {pop.unemploymentRate}");
            else
                Debug.LogError("[FAIL] Automation unemployment calculation.");

            // 2. Test Synthetic Citizen
            GameObject synthGo = new GameObject("SyntheticUnit");
            var synth = synthGo.AddComponent<SyntheticCitizen>();
            synth.powerLevel = 100f;
            synth.CalculateHappiness();

            if (synth.satisfaction == 100f)
                Debug.Log("[PASS] Synthetic satisfaction logic.");
            else
                Debug.LogError("[FAIL] Synthetic satisfaction logic.");

            // 3. Test Machine Uprising
            manager.aiConsciousness = 95f;
            manager.TriggerUprising();

            if (ai.isUprisingActive)
                Debug.Log("[PASS] Machine uprising trigger.");
            else
                Debug.LogError("[FAIL] Machine uprising trigger.");

            Debug.Log("--- Automation System Test Complete ---");
        }
    }
}
