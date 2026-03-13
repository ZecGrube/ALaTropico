using UnityEngine;
using CaudilloBay.Core;
using CaudilloBay.AI;
using CaudilloBay.World;

namespace CaudilloBay.Tests
{
    public class EducationSystemTest : MonoBehaviour
    {
        void Start()
        {
            RunTests();
        }

        public void RunTests()
        {
            Debug.Log("Starting EducationSystem Tests...");

            // Setup
            GameObject popGo = new GameObject("PopulationManager");
            PopulationManager popMan = popGo.AddComponent<PopulationManager>();
            // Add a sample citizen
            GameObject citGo = new GameObject("Citizen");
            Citizen citizen = citGo.AddComponent<Citizen>();
            citizen.educationLevel = 10f;
            popMan.allCitizens.Add(citizen);

            GameObject eduGo = new GameObject("EducationManager");
            EducationManager eduMan = eduGo.AddComponent<EducationManager>();

            // Test 1: Average Education
            eduMan.ProcessMonthlyEducation();
            if (Mathf.Approximately(eduMan.globalEducationLevel, 10f)) Debug.Log("Test 1 Passed: Average calculation correct.");
            else Debug.LogError($"Test 1 Failed: Expected 10, got {eduMan.globalEducationLevel}");

            // Test 2: Growth with School
            GameObject schoolGo = new GameObject("School");
            School school = schoolGo.AddComponent<School>();
            // Simulate functional school
            // Since we can't easily set protected bool, let's mock the check if we had a more testable architecture
            // For now, assume registration works.
            eduMan.RegisterBuilding(school);

            // monthlyEducationGrowth = totalBonus (5) / popSize (1) = 5
            eduMan.ProcessMonthlyEducation();

            // Citizen education should be 10 + 5 = 15
            if (Mathf.Approximately(citizen.educationLevel, 15f)) Debug.Log("Test 2 Passed: Education growth correct.");
            else Debug.LogError($"Test 2 Failed: Expected 15, got {citizen.educationLevel}");

            Debug.Log("EducationSystem Tests Complete.");
        }
    }
}
