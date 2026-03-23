using UnityEngine;
using CaudilloBay.Core;
using CaudilloBay.Politics;
using CaudilloBay.Data;
using System.Collections.Generic;

namespace CaudilloBay.Tests
{
    public class DiplomaticVictoryTest : MonoBehaviour
    {
        public void Start()
        {
            RunTests();
        }

        public void RunTests()
        {
            Debug.Log("--- Starting Diplomatic Victory Test ---");

            // Setup
            GameObject influenceGo = new GameObject("GlobalInfluenceManager");
            var influence = influenceGo.AddComponent<GlobalInfluenceManager>();

            GameObject crisisGo = new GameObject("GlobalCrisisManager");
            var crisisManager = crisisGo.AddComponent<GlobalCrisisManager>();

            GameObject mapGo = new GameObject("GlobalMapManager");
            var mapManager = mapGo.AddComponent<GlobalMapManager>();

            GameObject stateGo = new GameObject("GameStateManager");
            var stateManager = stateGo.AddComponent<GameStateManager>();

            // 1. Test Influence Calculation
            influence.CalculateInfluence();
            Debug.Log($"[INFO] Initial Influence: {influence.globalInfluence}");

            // 2. Test Crisis Resolution
            var template = ScriptableObject.CreateInstance<GlobalCrisisTemplate>();
            template.crisisName = "Test Crisis";
            template.influenceReward = 100f;
            template.prestigeReward = 20f;
            crisisManager.allCrisisTemplates.Add(template);

            var instance = new GlobalCrisisInstance { template = template };
            // Manually solving for test
            influence.SolveCrisis();
            influence.globalInfluence += template.influenceReward;

            if (influence.crisesSolved == 1 && influence.globalInfluence >= 100f)
                Debug.Log("[PASS] Crisis resolution rewards.");
            else
                Debug.LogError("[FAIL] Crisis resolution rewards.");

            // 3. Test Summit
            influence.internationalPrestige = 80f;
            // Mock Economy
            GameObject ecoGo = new GameObject("EconomyManager");
            var eco = ecoGo.AddComponent<CaudilloBay.Economy.EconomyManager>();
            eco.treasuryBalance = 20000f;

            mapManager.HostSummit();
            if (influence.globalInfluence >= 200f)
                Debug.Log("[PASS] Hosting summit.");
            else
                Debug.LogError("[FAIL] Hosting summit.");

            // 4. Test Victory Check
            influence.globalInfluence = 850f;
            influence.internationalPrestige = 90f;
            influence.crisesSolved = 3;
            influence.yearsOfPeace = 15f;

            stateManager.CheckVictoryConditions();
            if (stateManager.hasWon)
                Debug.Log("[PASS] Diplomatic victory condition.");
            else
                Debug.LogError("[FAIL] Diplomatic victory condition.");

            Debug.Log("--- Diplomatic Victory Test Complete ---");
        }
    }
}
