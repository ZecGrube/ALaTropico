using UnityEngine;
using CaudilloBay.Politics;
using CaudilloBay.Data;
using System.Collections.Generic;

namespace CaudilloBay.Tests
{
    public class GlobalEventTest : MonoBehaviour
    {
        public void Start()
        {
            RunTests();
        }

        public void RunTests()
        {
            Debug.Log("--- Starting Global Event Tests ---");
            TestEventGeneration();
            TestEventResolution();
            Debug.Log("--- Global Event Tests Complete ---");
        }

        private void TestEventGeneration()
        {
            GameObject genGO = new GameObject("GlobalEventGenerator");
            var gen = genGO.AddComponent<GlobalEventGenerator>();

            GlobalEventTemplate template = ScriptableObject.CreateInstance<GlobalEventTemplate>();
            template.eventId = "test_crisis";
            template.titleTemplate = "Crisis in {country}";
            template.descriptionTemplate = "Help {country}!";
            template.baseSpawnChance = 1.0f; // Force spawn

            gen.eventTemplates.Add(template);

            // Mock neighbor
            GameObject dipGO = new GameObject("RegionalPoliticsManager");
            var dip = dipGO.AddComponent<RegionalPoliticsManager>();
            NeighborState neighbor = ScriptableObject.CreateInstance<NeighborState>();
            neighbor.stateName = "Islas Libres";
            dip.neighbors.Add(neighbor);

            gen.MonthlyTick();

            if (gen.activeEvents.Count > 0 && gen.activeEvents[0].resolvedTitle.Contains("Islas Libres"))
                Debug.Log("[PASS] Global Event Generation: Event spawned with correct country name.");
            else
                Debug.LogError("[FAIL] Global Event Generation: Event failed to spawn or resolve title.");
        }

        private void TestEventResolution()
        {
            var gen = GlobalEventGenerator.Instance;
            var ev = gen.activeEvents[0];

            GlobalEventOption option = new GlobalEventOption();
            option.optionText = "Send Money";
            option.effects = new List<GlobalEventEffect> {
                new GlobalEventEffect { type = GlobalEventEffect.EffectType.ChangeRelations, value = 20f }
            };

            float initialRelations = ev.neighborRef.relations;
            gen.ResolveEvent(ev, option);

            if (ev.neighborRef.relations == initialRelations + 20f && gen.activeEvents.Count == 0)
                Debug.Log("[PASS] Global Event Resolution: Effects applied and event removed.");
            else
                Debug.LogError("[FAIL] Global Event Resolution: Relations not updated or event not cleared.");
        }
    }
}
