using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.Core;
using CaudilloBay.Data;

namespace CaudilloBay.Tests
{
    public class ModifierManagerTest : MonoBehaviour
    {
        void Start()
        {
            RunTests();
        }

        public void RunTests()
        {
            Debug.Log("Starting ModifierManager Tests...");

            // Setup
            GameObject go = new GameObject("ModifierManagerTest");
            ModifierManager manager = go.AddComponent<ModifierManager>();

            // Test 1: Add and Total
            ModifierData m1 = new ModifierData { modifierId = "test1", type = ModifierType.ProductionEfficiency, value = 0.5f, durationMonths = 2 };
            ModifierData m2 = new ModifierData { modifierId = "test2", type = ModifierType.ProductionEfficiency, value = 0.2f, durationMonths = 1 };

            manager.AddModifier(m1);
            manager.AddModifier(m2);

            float total = manager.GetTotalModifier(ModifierType.ProductionEfficiency);
            if (Mathf.Approximately(total, 0.7f)) Debug.Log("Test 1 Passed: Totals correct.");
            else Debug.LogError($"Test 1 Failed: Expected 0.7, got {total}");

            // Test 2: Monthly Tick and Expiry
            manager.ProcessMonthlyTick(); // Month 1 passed
            total = manager.GetTotalModifier(ModifierType.ProductionEfficiency);
            // m2 should expire after 1 month? Wait, remainingMonths starts at duration.
            // In code: remainingMonths starts at data.durationMonths. ProcessMonthlyTick: remainingMonths--. if remainingMonths <= 0 remove.
            // After 1 tick: m1 (1 left), m2 (0 left) -> m2 removed.

            if (Mathf.Approximately(total, 0.5f)) Debug.Log("Test 2 Passed: m2 expired correctly.");
            else Debug.LogError($"Test 2 Failed: Expected 0.5, got {total}");

            manager.ProcessMonthlyTick(); // Month 2 passed
            total = manager.GetTotalModifier(ModifierType.ProductionEfficiency);
            if (Mathf.Approximately(total, 0f)) Debug.Log("Test 3 Passed: m1 expired correctly.");
            else Debug.LogError($"Test 3 Failed: Expected 0, got {total}");

            Debug.Log("ModifierManager Tests Complete.");
            Destroy(go);
        }
    }
}
