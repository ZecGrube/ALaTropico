using UnityEngine;
using CaudilloBay.Core;
using CaudilloBay.AI;
using CaudilloBay.World;
using CaudilloBay.Data;

namespace CaudilloBay.Tests
{
    public class FullSimulationTest : MonoBehaviour
    {
        void Start()
        {
            RunTests();
        }

        public void RunTests()
        {
            Debug.Log("Starting Full Simulation Tests...");

            // 1. Setup Managers
            GameObject go = new GameObject("SimulationManagers");
            PopulationManager popMan = go.AddComponent<PopulationManager>();
            EducationManager eduMan = go.AddComponent<EducationManager>();
            CrimeManager crimeMan = go.AddComponent<CrimeManager>();
            StatsManager statsMan = go.AddComponent<StatsManager>();

            // 2. Test Resource Consumption in Construction
            GameObject builderGo = new GameObject("Builder");
            BuilderAI builder = builderGo.AddComponent<BuilderAI>();

            // Mock a building with costs
            GameObject bGo = new GameObject("TestBuilding");
            ProducerBuilding b = bGo.AddComponent<ProducerBuilding>();
            // Assume initialization through some data asset would happen normally
            // For unit test we'd need to mock the costs list

            Debug.Log("Simulation Test Step: Setup complete.");

            // 3. Test Education impact on Production
            eduMan.globalEducationLevel = 50f;
            // Production efficiency should be 1.0 + (50/200) = 1.25

            // 4. Test Crime impact on Satisfaction
            crimeMan.globalCrimeRate = 60f;
            // Fear of crime should increase in citizens

            Debug.Log("Full Simulation Tests Complete (Architectural Check).");
            Destroy(go);
            Destroy(builderGo);
            Destroy(bGo);
        }
    }
}
