using UnityEngine;
using CaudilloBay.Core;
using CaudilloBay.Politics;
using CaudilloBay.World;

namespace CaudilloBay.Tests
{
    public class CorruptionSystemTest : MonoBehaviour
    {
        void Start()
        {
            RunTests();
        }

        public void RunTests()
        {
            Debug.Log("Starting CorruptionSystem Tests...");

            // Setup
            GameObject go = new GameObject("CorruptionManager");
            CorruptionManager manager = go.AddComponent<CorruptionManager>();
            manager.blackMarketMoney = 1000f;

            // Test 1: Black Market Generation
            GameObject denGo = new GameObject("SmugglingDen");
            SmugglingDen den = denGo.AddComponent<SmugglingDen>();
            manager.RegisterShadowBuilding(den);

            manager.ProcessMonthlyCorruption();
            // Should add 100
            if (Mathf.Approximately(manager.blackMarketMoney, 1100f)) Debug.Log("Test 1 Passed: Passive shadow income generated.");
            else Debug.LogError($"Test 1 Failed: Expected 1100, got {manager.blackMarketMoney}");

            // Test 2: Bribery
            GameObject factionGo = new GameObject("FactionManager");
            FactionManager fMan = factionGo.AddComponent<FactionManager>();
            FactionData data = new FactionData { type = FactionType.Peasants, loyalty = 50f };
            fMan.factions.Add(data);

            fMan.BribeFaction(FactionType.Peasants, 100f);
            // manager.blackMarketMoney should be 1000 now.
            // loyalty should be 50 + (100/100) = 51.
            if (Mathf.Approximately(manager.blackMarketMoney, 1000f) && Mathf.Approximately(data.loyalty, 51f))
                Debug.Log("Test 2 Passed: Bribery logic correct.");
            else
                Debug.LogError($"Test 2 Failed: Money {manager.blackMarketMoney}, Loyalty {data.loyalty}");

            Debug.Log("CorruptionSystem Tests Complete.");
        }
    }
}
