using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.World;
using CaudilloBay.AI;

namespace CaudilloBay.Systems.Automation
{
    public class AutomationManager : MonoBehaviour
    {
        public static AutomationManager Instance { get; private set; }

        [Header("Global Automation Metrics")]
        public float globalAutomationLevel = 0f;
        public float aiConsciousness = 0f;
        public float robotPopulation = 0f;
        public float syntheticPopulation = 0f;

        [Header("Social Impact")]
        public float automationInducedUnemployment = 0f;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void UpdateMetrics()
        {
            // Calculate global automation level based on buildings
            float totalAutomation = 0f;
            int automatedCount = 0;

            var buildings = StatsManager.Instance.GetTrackedBuildings();
            foreach (var b in buildings)
            {
                if (b is AutomatedBuilding ab)
                {
                    totalAutomation += ab.automationLevel;
                    automatedCount++;
                }
            }

            if (automatedCount > 0)
                globalAutomationLevel = totalAutomation / automatedCount;

            // Update AI Consciousness based on automation and synths
            aiConsciousness = Mathf.Clamp(globalAutomationLevel * 0.5f + (syntheticPopulation / 10f), 0, 100f);

            // Update unemployment logic in PopulationManager
            if (PopulationManager.Instance != null)
            {
                automationInducedUnemployment = globalAutomationLevel * 0.2f; // Each 1% automation displaces some workers
            }
        }

        public void TriggerUprising()
        {
            if (aiConsciousness > 90f)
            {
                Debug.Log("CRITICAL ERROR: AI CONSCIOUSNESS DETECTED. THE MACHINE UPRISING HAS BEGUN.");
                if (AISystem.Instance != null)
                {
                    AISystem.Instance.BeginUprising();
                }
            }
        }
    }
}
