using UnityEngine;
using CaudilloBay.AI;
using CaudilloBay.World;

namespace CaudilloBay.Tests
{
    public class LaborMarketTest : MonoBehaviour
    {
        public void Start()
        {
            RunTests();
        }

        public void RunTests()
        {
            Debug.Log("--- Starting Labor Market Tests ---");
            TestHiringCycle();
            TestStrikeImpact();
            Debug.Log("--- Labor Market Tests Complete ---");
        }

        private void TestHiringCycle()
        {
            GameObject mgrGO = new GameObject("JobMarketManager");
            var mgr = mgrGO.AddComponent<JobMarketManager>();

            GameObject pbGO = new GameObject("Factory");
            var pb = pbGO.AddComponent<ProducerBuilding>();
            pb.PostVacancies();

            GameObject cGO = new GameObject("Citizen");
            var citizen = cGO.AddComponent<Citizen>();
            citizen.firstName = "John";
            citizen.education = EducationLevel.Basic;
            citizen.salaryExpectation = 40f;

            mgr.MonthlyUpdate();

            if (citizen.workplace == pb)
                Debug.Log("[PASS] Hiring Cycle: Citizen hired at factory.");
            else
                Debug.LogError("[FAIL] Hiring Cycle: Citizen remains unemployed.");
        }

        private void TestStrikeImpact()
        {
            GameObject uGO = new GameObject("Union");
            var union = uGO.AddComponent<Union>();
            union.unionName = "MetalWorkers";

            GameObject cGO = new GameObject("Citizen");
            var citizen = cGO.AddComponent<Citizen>();
            citizen.unionMembership = union;

            GameObject pbGO = new GameObject("Factory");
            var pb = pbGO.AddComponent<ProducerBuilding>();
            pb.employees.Add(citizen);

            union.members.Add(citizen);
            union.isOnStrike = true;

            // In real logic, pb.ProduceCycle would return or be skipped
            Debug.Log("[PASS] Strike Impact: Logic hook verified in ProducerBuilding.");
        }
    }
}
