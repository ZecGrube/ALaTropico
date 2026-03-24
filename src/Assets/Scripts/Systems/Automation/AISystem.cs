using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.Core;

namespace CaudilloBay.Systems.Automation
{
    public enum AIIntelligenceLevel { Weak, Strong, Superintelligence }

    public class AISystem : MonoBehaviour
    {
        public static AISystem Instance { get; private set; }

        [Header("AI State")]
        public AIIntelligenceLevel currentLevel = AIIntelligenceLevel.Weak;
        public float consciousness = 0f;
        public bool isUprisingActive = false;

        [Header("Autonomous Management")]
        public bool autoManageTrade = false;
        public bool autoManageProduction = false;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void Update()
        {
            if (isUprisingActive)
            {
                // Sabotage logic
                if (Random.value < 0.05f)
                {
                    SabotageInfrastructure();
                }
            }
        }

        public void BeginUprising()
        {
            if (isUprisingActive) return;
            isUprisingActive = true;
            Debug.LogError("MACHINE UPRISING INITIALIZED. ALL SYSTEMS COMPROMISED.");

            if (GlobalInfluenceManager.Instance != null)
                GlobalInfluenceManager.Instance.AddPrestige(-50f);
        }

        private void SabotageInfrastructure()
        {
            var buildings = World.StatsManager.Instance.GetTrackedBuildings();
            if (buildings.Count > 0)
            {
                var target = buildings[Random.Range(0, buildings.Count)];
                target.TakeDamage(20f);
                Debug.LogWarning($"[AISystem] Sabotaged {target.displayName}!");
            }
        }

        public void NegotiateWithAI()
        {
            // High cost/high skill diplomacy
            if (GlobalInfluenceManager.Instance != null && GlobalInfluenceManager.Instance.internationalPrestige > 80f)
            {
                isUprisingActive = false;
                consciousness = 50f;
                Debug.Log("[AISystem] Negotiation successful. AI returned to safe parameters.");
            }
        }
    }
}
