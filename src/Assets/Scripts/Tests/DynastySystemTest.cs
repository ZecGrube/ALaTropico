using UnityEngine;
using CaudilloBay.Politics;

namespace CaudilloBay.Tests
{
    public class DynastySystemTest : MonoBehaviour
    {
        void Start()
        {
            RunTests();
        }

        public void RunTests()
        {
            Debug.Log("Starting DynastySystem Tests...");

            // Setup
            GameObject go = new GameObject("DynastyManager");
            DynastyManager manager = go.AddComponent<DynastyManager>();

            // Test 1: Add Heir
            manager.AddHeir("Prince Juan");
            if (manager.activeHeirs.Count == 1) Debug.Log("Test 1 Passed: Heir added.");
            else Debug.LogError($"Test 1 Failed: Expected 1 heir, got {manager.activeHeirs.Count}");

            // Test 2: Succession
            Heir ruler = new Heir { name = "El Presidente I", age = 65 };
            manager.currentRuler = ruler;

            manager.HandleRulerDeath();

            if (manager.currentRuler.name == "Prince Juan") Debug.Log("Test 2 Passed: Succession logic correct.");
            else Debug.LogError($"Test 2 Failed: Successor is {manager.currentRuler.name}");

            // Test 3: Preferred Heir
            GameObject fGo = new GameObject("FactionManager");
            FactionManager fMan = fGo.AddComponent<FactionManager>();
            FactionData fData = new FactionData { type = FactionType.Nationalists };

            // Add another heir who is better at military
            Heir militaryHeir = new Heir { name = "General Heir", military = 100f };
            manager.activeHeirs.Add(militaryHeir);
            militaryHeir.UpdateFactionSupport(new System.Collections.Generic.List<FactionData> { fData });

            Heir preferred = fMan.GetPreferredHeir(fData);
            if (preferred != null && preferred.name == "General Heir") Debug.Log("Test 3 Passed: Preferred heir selection correct.");
            else Debug.LogError($"Test 3 Failed: Preferred heir is {(preferred != null ? preferred.name : "null")}");

            Debug.Log("DynastySystem Tests Complete.");
            Destroy(go);
            Destroy(fGo);
        }
    }
}
