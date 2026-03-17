using UnityEngine;
using CaudilloBay.Economy;
using CaudilloBay.Politics;
using CaudilloBay.Core;
using CaudilloBay.Data;

namespace CaudilloBay.Tests
{
    public class Sprint3Verification : MonoBehaviour
    {
        public void Start()
        {
            RunVerification();
        }

        public void RunVerification()
        {
            Debug.Log("--- Starting Sprint 3 Verification ---");

            VerifyDiplomacy();
            VerifyAdvancedTourism();
            VerifyClimateSystem();
            VerifySportsSuccess();
            VerifyExploration();

            Debug.Log("--- Sprint 3 Verification Complete ---");
        }

        private void VerifyDiplomacy()
        {
            GameObject go = new GameObject("RegionalPoliticsManager");
            var mgr = go.AddComponent<RegionalPoliticsManager>();

            NeighborState neighbor = ScriptableObject.CreateInstance<NeighborState>();
            neighbor.stateName = "Islas Muertas";
            neighbor.relations = 50f;

            mgr.ProposeNonAggressionPact(neighbor);

            if (neighbor.hasNonAggressionPact)
                Debug.Log("[PASS] Diplomacy: Non-aggression pact accepted.");
            else
                Debug.LogError("[FAIL] Diplomacy: Pact rejected at high relations.");
        }

        private void VerifyAdvancedTourism()
        {
            GameObject go = new GameObject("TouristManager");
            var mgr = go.AddComponent<TouristManager>();

            mgr.UpdateTourism(100f, 1f, 0); // January (Month 0) - Peak
            float peakSeason = mgr.seasonalityFactor;

            mgr.UpdateTourism(100f, 1f, 6); // July (Month 6) - Off-peak
            float offPeak = mgr.seasonalityFactor;

            if (peakSeason > offPeak)
                Debug.Log("[PASS] Advanced Tourism: Seasonality working.");
            else
                Debug.LogError("[FAIL] Advanced Tourism: Seasonality incorrect.");
        }

        private void VerifyClimateSystem()
        {
            GameObject go = new GameObject("ClimateManager");
            var mgr = go.AddComponent<ClimateManager>();

            mgr.ProcessMonthlyClimate(1000000f); // High pollution

            if (mgr.globalCO2Level > 400f)
                Debug.Log("[PASS] Climate: CO2 accumulation working.");
            else
                Debug.LogError("[FAIL] Climate: CO2 stable despite pollution.");
        }

        private void VerifySportsSuccess()
        {
            GameObject go = new GameObject("SportsManager");
            var mgr = go.AddComponent<SportsManager>();
            mgr.footballTeamLevel = 100f; // God-tier team

            float initialPride = mgr.nationalPride;
            mgr.RunRegionalChampionship();

            if (mgr.nationalPride >= initialPride)
                Debug.Log("[PASS] Sports: Championship logic functional.");
            else
                Debug.LogError("[FAIL] Sports: Pride logic error.");
        }

        private void VerifyExploration()
        {
            GameObject go = new GameObject("ExplorationManager");
            var mgr = go.AddComponent<ExplorationManager>();

            // Mock economy
            GameObject econGO = new GameObject("EconomyManager");
            var econ = econGO.AddComponent<EconomyManager>();
            econ.treasuryBalance = 1000f;

            mgr.StartGeologicalSurvey(100f);
            if (mgr.isExploring && econ.treasuryBalance == 900f)
                Debug.Log("[PASS] Exploration: Survey started successfully.");
            else
                Debug.LogError("[FAIL] Exploration: Survey start failed.");
        }
    }
}
