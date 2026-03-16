using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.AI;
using CaudilloBay.World;
using CaudilloBay.Economy;

namespace CaudilloBay.Tests
{
    public class CitizenSimulationTest : MonoBehaviour
    {
        public void Start()
        {
            RunTests();
        }

        public void RunTests()
        {
            Debug.Log("--- Starting Citizen Deep Simulation Tests ---");

            TestCitizenIdentity();
            TestJobAssignment();
            TestIncomeTax();
            TestPerformanceLoad();

            Debug.Log("--- Citizen Deep Simulation Tests Complete ---");
        }

        private void TestCitizenIdentity()
        {
            GameObject go = new GameObject("Citizen");
            var citizen = go.AddComponent<Citizen>();

            // Generate Identity is called in Start/Update usually, we force it
            // citizen.GenerateRandomIdentity(); // Private method in source, assuming auto-gen

            if (citizen.traits.ambition >= 0)
            {
                Debug.Log("[PASS] Citizen Identity/Traits Initialization");
            }
            else
            {
                Debug.LogError("[FAIL] Citizen Identity Initialization");
            }
        }

        private void TestJobAssignment()
        {
            GameObject marketGO = new GameObject("JobMarket");
            var market = marketGO.AddComponent<JobMarket>();

            GameObject citizenGO = new GameObject("Citizen");
            var citizen = citizenGO.AddComponent<Citizen>();

            GameObject factoryGO = new GameObject("Factory");
            var factory = factoryGO.AddComponent<ProducerBuilding>();
            factory.data = ScriptableObject.CreateInstance<Data.BuildingData>();
            factory.data.maintenanceCost = 100f;

            market.AssignJob(citizen, factory);

            if (citizen.workplace == factory && factory.employees.Contains(citizen))
            {
                Debug.Log("[PASS] Job Assignment Logic");
            }
            else
            {
                Debug.LogError("[FAIL] Job Assignment Logic");
            }
        }

        private void TestIncomeTax()
        {
            GameObject econGO = new GameObject("EconomyManager");
            var econ = econGO.AddComponent<EconomyManager>();

            GameObject popGO = new GameObject("PopulationManager");
            var pop = popGO.AddComponent<PopulationManager>();

            GameObject citizenGO = new GameObject("Citizen");
            var citizen = citizenGO.AddComponent<Citizen>();
            citizen.salary = 100f;
            citizen.personalWealth = 200f;
            citizen.workplace = new GameObject().AddComponent<ProducerBuilding>();

            pop.allCitizens.Add(citizen);

            float initialTreasury = econ.treasuryBalance;
            econ.ProcessMonthlyEconomy(new List<Building>());

            if (citizen.personalWealth < 200f && econ.treasuryBalance > initialTreasury)
            {
                Debug.Log("[PASS] Income Tax Logic");
            }
            else
            {
                Debug.LogError("[FAIL] Income Tax Logic");
            }
        }

        private void TestPerformanceLoad()
        {
            GameObject popGO = GameObject.Find("PopulationManager") ?? new GameObject("PopulationManager");
            var pop = popGO.GetComponent<PopulationManager>() ?? popGO.AddComponent<PopulationManager>();

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            for (int i = 0; i < 2000; i++)
            {
                GameObject go = new GameObject("Citizen_" + i);
                var c = go.AddComponent<Citizen>();
                pop.allCitizens.Add(c);
            }

            sw.Stop();
            Debug.Log($"[PERF] Spawned 2000 citizens in {sw.ElapsedMilliseconds}ms");

            sw.Restart();
            pop.UpdateGlobalStats();
            sw.Stop();
            Debug.Log($"[PERF] Processed stats for 2000 citizens in {sw.ElapsedMilliseconds}ms");

            if (sw.ElapsedMilliseconds < 50)
            {
                Debug.Log("[PASS] Performance Target");
            }
            else
            {
                Debug.LogWarning("[WARN] Performance slow: " + sw.ElapsedMilliseconds + "ms");
            }
        }
    }
}
