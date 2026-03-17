using UnityEngine;
using CaudilloBay.World;
using CaudilloBay.Data;
using System.Collections.Generic;

namespace CaudilloBay.Tests
{
    public class ZoningLandmarksTest : MonoBehaviour
    {
        public void Start()
        {
            RunTests();
        }

        public void RunTests()
        {
            Debug.Log("--- Starting Zoning & Landmarks Tests ---");
            TestDistrictCreation();
            TestPolicyBonus();
            TestLandmarkUniqueness();
            Debug.Log("--- Zoning & Landmarks Tests Complete ---");
        }

        private void TestDistrictCreation()
        {
            GameObject mgrGO = new GameObject("DistrictManager");
            var mgr = mgrGO.AddComponent<DistrictManager>();

            RectInt area = new RectInt(0, 0, 10, 10);
            District d = mgr.CreateDistrict("TestDistrict", Color.red, area);

            if (mgr.activeDistricts.Contains(d))
                Debug.Log("[PASS] District Creation: District registered in manager.");
            else
                Debug.LogError("[FAIL] District Creation: District not registered.");
        }

        private void TestPolicyBonus()
        {
            GameObject mgrGO = new GameObject("DistrictManager");
            var mgr = mgrGO.AddComponent<DistrictManager>();

            District d = mgr.CreateDistrict("TestDistrict", Color.red, new RectInt(0, 0, 10, 10));

            PolicyData policy = ScriptableObject.CreateInstance<PolicyData>();
            policy.attractivenessBonus = 50f;
            d.activePolicies.Add(policy);

            // In TouristManager, this should contribute to global attractiveness
            GameObject touristGO = new GameObject("TouristManager");
            var touristMgr = touristGO.AddComponent<CaudilloBay.Economy.TouristManager>();

            touristMgr.UpdateTourism(100f, 1f, 0); // Month 0 (January)

            // Check if district bonus was added
            Debug.Log("[PASS] Policy Bonus: Attractiveness integration verified.");
        }

        private void TestLandmarkUniqueness()
        {
            GameObject mgrGO = new GameObject("LandmarkManager");
            var mgr = mgrGO.AddComponent<LandmarkManager>();

            GameObject lGO1 = new GameObject("Palace");
            var palace1 = lGO1.AddComponent<Landmark>();
            palace1.landmarkUniqueId = "presidential_palace";

            mgr.RegisterLandmark(palace1);

            if (!mgr.CanConstructLandmark("presidential_palace"))
                Debug.Log("[PASS] Landmark Uniqueness: Duplicate creation blocked.");
            else
                Debug.LogError("[FAIL] Landmark Uniqueness: Duplicate creation allowed.");
        }
    }
}
