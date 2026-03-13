using UnityEngine;
using CaudilloBay.Data;
using CaudilloBay.Politics;

namespace CaudilloBay.World
{
    public class ResidentialBuilding : Building
    {
        [Header("Residents")]
        public int capacity = 4;
        public List<CaudilloBay.AI.Citizen> residents = new List<CaudilloBay.AI.Citizen>();

        [Header("Resident Needs")]
        public ResourceType foodType;
        public float foodConsumptionPerMonth = 1.0f;

        [Header("Happiness State")]
        public float currentHappiness = 50f;

        [Header("Faction Leanings")]
        public FactionType primaryFaction = FactionType.Peasants;

        public float GetHappiness()
        {
            float targetHappiness = 50f;

            // Happiness depends on food availability in building inventory
            if (foodType != null && inventory.HasResource(foodType, foodConsumptionPerMonth))
            {
                targetHappiness = 100f;
            }
            else
            {
                targetHappiness = 0f;
            }

            // Penalty for local crime
            if (CaudilloBay.Core.CrimeManager.Instance != null)
            {
                float localCrime = CaudilloBay.Core.CrimeManager.Instance.GetLocalCrimeRate(transform.position);
                targetHappiness -= localCrime * 0.5f;
            }

            currentHappiness = Mathf.MoveTowards(currentHappiness, Mathf.Clamp(targetHappiness, 0, 100), 5f);

            return currentHappiness;
        }

        protected override void CompleteConstruction()
        {
            base.CompleteConstruction();

            if (CaudilloBay.AI.PopulationManager.Instance != null)
            {
                CaudilloBay.AI.PopulationManager.Instance.RegisterResidential(this, capacity);
            }
        }
    }
}
