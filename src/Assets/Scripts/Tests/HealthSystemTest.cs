using UnityEngine;
using CaudilloBay.Core;
using CaudilloBay.AI;
using CaudilloBay.World;

namespace CaudilloBay.Tests
{
    public class HealthSystemTest : MonoBehaviour
    {
        void Start()
        {
            RunTests();
        }

        public void RunTests()
        {
            Debug.Log("Starting HealthSystem Tests...");

            // Setup
            GameObject popGo = new GameObject("PopulationManager");
            PopulationManager popMan = popGo.AddComponent<PopulationManager>();
            GameObject citGo = new GameObject("Citizen");
            Citizen citizen = citGo.AddComponent<Citizen>();
            citizen.health = 50f;
            popMan.allCitizens.Add(citizen);

            GameObject healthGo = new GameObject("HealthManager");
            HealthManager healthMan = healthGo.AddComponent<HealthManager>();

            // Test 1: Global Health
            healthMan.ProcessMonthlyHealth();
            if (Mathf.Approximately(healthMan.globalHealthLevel, 50f)) Debug.Log("Test 1 Passed: Average calculation correct.");
            else Debug.LogError($"Test 1 Failed: Expected 50, got {healthMan.globalHealthLevel}");

            // Test 2: Health Decay without Clinics
            // monthlyHealthDecay = baseDecay (-1.0) = -1.0
            healthMan.ProcessMonthlyHealth();
            if (Mathf.Approximately(citizen.health, 49f)) Debug.Log("Test 2 Passed: Health decay correct.");
            else Debug.LogError($"Test 2 Failed: Expected 49, got {citizen.health}");

            // Test 3: Health Recovery with Clinic
            GameObject clinicGo = new GameObject("Clinic");
            Clinic clinic = clinicGo.AddComponent<Clinic>();
            healthMan.RegisterBuilding(clinic);
            // monthlyHealthDecay = baseDecay (-1.0) + medicalBonus (2.0) = 1.0
            healthMan.ProcessMonthlyHealth();
            if (Mathf.Approximately(citizen.health, 50f)) Debug.Log("Test 3 Passed: Health recovery correct.");
            else Debug.LogError($"Test 3 Failed: Expected 50, got {citizen.health}");

            Debug.Log("HealthSystem Tests Complete.");
        }
    }
}
