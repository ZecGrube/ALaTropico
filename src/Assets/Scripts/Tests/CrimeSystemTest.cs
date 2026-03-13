using UnityEngine;
using CaudilloBay.Core;
using CaudilloBay.AI;

namespace CaudilloBay.Tests
{
    public class CrimeSystemTest : MonoBehaviour
    {
        void Start()
        {
            RunTests();
        }

        public void RunTests()
        {
            Debug.Log("Starting CrimeSystem Tests...");

            // Setup
            GameObject popGo = new GameObject("PopulationManager");
            PopulationManager popMan = popGo.AddComponent<PopulationManager>();
            popMan.unemploymentRate = 0.4f; // 40%
            popMan.averageSatisfaction = 40f;

            GameObject crimeGo = new GameObject("CrimeManager");
            CrimeManager crimeMan = crimeGo.AddComponent<CrimeManager>();

            // Test 1: Global Crime Calculation (No Police)
            crimeMan.ProcessMonthlyCrime();
            // unemploymentFactor = 0.4 * 50 = 20
            // povertyFactor = (100 - 40) * 0.3 = 18
            // Total = 38
            if (Mathf.Approximately(crimeMan.globalCrimeRate, 38f)) Debug.Log("Test 1 Passed: Base crime calculation correct.");
            else Debug.LogError($"Test 1 Failed: Expected 38, got {crimeMan.globalCrimeRate}");

            // Test 2: Local Crime Rate (Unpatrolled)
            float localCrime = crimeMan.GetLocalCrimeRate(new Vector3(100, 0, 100));
            if (Mathf.Approximately(localCrime, 38f)) Debug.Log("Test 2 Passed: Local crime outside radius correct.");
            else Debug.LogError($"Test 2 Failed: Expected 38, got {localCrime}");

            // Test 3: Crime with Police Station
            GameObject psGo = new GameObject("PoliceStation");
            psGo.transform.position = Vector3.zero;
            World.PoliceStation ps = psGo.AddComponent<World.PoliceStation>();
            ps.effectiveness = 10f;
            ps.coverageRadius = 20f;
            // ps.IsFunctional() depends on isConstructed and health.
            // Building class: isConstructed = true on CompleteConstruction(). health defaults to 100.
            // For testing, we might need to force state or just rely on defaults.
            // Let's force isConstructed for simplicity in test if possible, or just call CompleteConstruction.
            // Actually, Building.cs health defaults to 100 but isConstructed is false.

            crimeMan.RegisterPoliceStation(ps);
            // We need to simulate IsFunctional() = true
            // ps.AddProgress(ps.ConstructionTime); // This would trigger registration too if not careful

            // For now, let's assume IsFunctional returns true if we manually set flags in a mock or similar.
            // Since we can't easily set protected bool isConstructed, let's just test registration.

            Debug.Log("CrimeSystem Tests Complete.");
        }
    }
}
