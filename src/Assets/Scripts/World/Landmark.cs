using UnityEngine;

namespace CaudilloBay.World
{
    public class Landmark : Building
    {
        [Header("Landmark Properties")]
        public string landmarkUniqueId;
        public bool isUnique = true;

        [Header("Global Bonuses")]
        public float globalTourismBonus = 0f;
        public float globalPrestigeBonus = 0f;
        public float globalStabilityBonus = 0f;

        protected override void CompleteConstruction()
        {
            base.CompleteConstruction();
            ApplyGlobalBonuses();
            Debug.Log($"Landmark {displayName} completed! Global bonuses applied.");
        }

        private void ApplyGlobalBonuses()
        {
            if (Economy.TouristManager.Instance != null)
            {
                Economy.TouristManager.Instance.activeEventBonus += globalTourismBonus;
            }

            if (Politics.LegitimacySystem.Instance != null)
            {
                Politics.LegitimacySystem.Instance.currentLegitimacy += globalPrestigeBonus;
            }

            // Add other global effects as needed
        }
    }
}
