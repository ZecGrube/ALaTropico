using UnityEngine;
using CaudilloBay.Systems.Space;
using CaudilloBay.Data;
using CaudilloBay.Core;
using System.Collections.Generic;

namespace CaudilloBay.Tests
{
    public class SpaceRaceTest : MonoBehaviour
    {
        public void Start()
        {
            RunTests();
        }

        public void RunTests()
        {
            Debug.Log("--- Starting Space Race Test ---");

            // Setup
            GameObject missionGo = new GameObject("SpaceMissionManager");
            var missionManager = missionGo.AddComponent<SpaceMissionManager>();

            GameObject raceGo = new GameObject("SpaceRaceManager");
            var raceManager = raceGo.AddComponent<SpaceRaceManager>();

            GameObject influenceGo = new GameObject("GlobalInfluenceManager");
            var influence = influenceGo.AddComponent<GlobalInfluenceManager>();

            // 1. Test Mission Scheduling
            var template = ScriptableObject.CreateInstance<SpaceMissionTemplate>();
            template.missionName = "Test Sputnik";
            template.preparationTime = 1f;
            template.type = SpaceMissionType.Satellite;
            template.baseSuccessChance = 100f; // Force success

            missionManager.ScheduleMission(template);
            if (missionManager.activeMissions.Count == 1)
                Debug.Log("[PASS] Mission scheduling.");
            else
                Debug.LogError("[FAIL] Mission scheduling.");

            // 2. Test Launch and Milestone
            missionManager.ProcessMonthlyUpdate();
            if (missionManager.firstSatellite)
                Debug.Log("[PASS] First satellite milestone achieved.");
            else
                Debug.LogError("[FAIL] First satellite milestone.");

            // 3. Test Satellite Effects
            var satTemplate = ScriptableObject.CreateInstance<SatelliteTemplate>();
            satTemplate.satelliteName = "ScienceSat";
            var mod = new ModifierData { id = "ScienceBoost", value = 0.2f };
            satTemplate.activeModifiers.Add(mod);
            template.satelliteToDeploy = satTemplate;

            // Re-run for satellite deployment
            missionManager.ScheduleMission(template);
            missionManager.ProcessMonthlyUpdate();
            if (missionManager.launchedSatellites.Count == 1)
                Debug.Log("[PASS] Satellite deployment.");
            else
                Debug.LogError("[FAIL] Satellite deployment.");

            // 4. Test Space Race Competition
            raceManager.ussrProgress = 99f;
            raceManager.ProcessMonthlyUpdate();
            if (raceManager.satellite_USSR)
                Debug.Log("[PASS] Space race global milestone tracking.");
            else
                Debug.LogError("[FAIL] Space race global milestone tracking.");

            Debug.Log("--- Space Race Test Complete ---");
        }
    }
}
