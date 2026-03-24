using UnityEngine;
using CaudilloBay.World;

namespace CaudilloBay.Systems.Automation
{
    public class AutomatedBuilding : ProducerBuilding
    {
        [Header("Automation Settings")]
        [Range(0, 100)]
        public float automationLevel = 0f;
        public float energyMultiplier = 1.5f;
        public float malfunctionRisk = 0.01f;

        public override bool IsFunctional()
        {
            // Automated buildings might require more power but fewer workers
            bool powerSatisfied = !requiresPower || (powerNode != null && powerNode.isSatisfied);

            // Reduced worker requirement check
            float requiredWorkers = data.maxWorkers * (1f - (automationLevel / 100f));
            bool workersSatisfied = currentWorkers >= requiredWorkers;

            return isConstructed && currentHealth > 0 && powerSatisfied && workersSatisfied;
        }

        protected override void ProduceCycle()
        {
            if (!IsFunctional()) return;

            // Malfunction check
            if (Random.value < malfunctionRisk * (automationLevel / 100f))
            {
                TakeDamage(10f);
                Debug.LogWarning($"[AutomatedBuilding] {displayName} malfunctioned!");
                return;
            }

            // Production efficiency is boosted by automation level
            float automationBonus = 1f + (automationLevel / 100f);

            // Logic to apply this bonus to production results would go here
            base.ProduceCycle();
        }
    }
}
