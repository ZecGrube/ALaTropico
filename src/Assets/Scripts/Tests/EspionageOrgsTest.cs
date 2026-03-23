using UnityEngine;
using CaudilloBay.Politics;
using CaudilloBay.Data;
using System.Collections.Generic;

namespace CaudilloBay.Tests
{
    public class EspionageOrgsTest : MonoBehaviour
    {
        public void Start()
        {
            RunTests();
        }

        public void RunTests()
        {
            Debug.Log("--- Starting Espionage & Orgs Tests ---");
            TestJoinOrganization();
            TestAllianceFormation();
            TestSpyNetworkGrowth();
            Debug.Log("--- Espionage & Orgs Tests Complete ---");
        }

        private void TestJoinOrganization()
        {
            GameObject mgrGO = new GameObject("OrganizationManager");
            var mgr = mgrGO.AddComponent<OrganizationManager>();

            GameObject legGO = new GameObject("LegitimacySystem");
            var leg = legGO.AddComponent<LegitimacySystem>();
            leg.currentLegitimacy = 100f;

            InternationalOrganization org = ScriptableObject.CreateInstance<InternationalOrganization>();
            org.orgId = "caribbean_bloc";
            org.legitimacyThreshold = 50f;

            mgr.TryJoin(org);

            if (mgr.activeMemberships.Contains(org))
                Debug.Log("[PASS] Join Organization: Membership approved.");
            else
                Debug.LogError("[FAIL] Join Organization: Membership denied.");
        }

        private void TestAllianceFormation()
        {
            GameObject mgrGO = new GameObject("AllianceManager");
            var mgr = mgrGO.AddComponent<AllianceManager>();

            NeighborState neighbor = ScriptableObject.CreateInstance<NeighborState>();
            neighbor.stateName = "Partner Island";
            neighbor.relations = 80f;

            mgr.CreateAlliance("Caribbean Axis", AllianceType.Military, neighbor);

            if (mgr.activeAlliances.Count > 0)
                Debug.Log("[PASS] Alliance Formation: Custom bloc created.");
            else
                Debug.LogError("[FAIL] Alliance Formation: Proposal rejected.");
        }

        private void TestSpyNetworkGrowth()
        {
            GameObject mgrGO = new GameObject("SpyNetworkManager");
            var mgr = mgrGO.AddComponent<SpyNetworkManager>();

            SpyNetwork net = new SpyNetwork { countryName = "Target", networkStrength = 0 };
            net.activeAgents.Add(new Agent()); // One agent

            net.UpdateMonthly();

            if (net.networkStrength > 0)
                Debug.Log("[PASS] Spy Network Growth: Strength increased with agents.");
            else
                Debug.LogError("[FAIL] Spy Network Growth: Strength stayed at zero.");
        }
    }
}
