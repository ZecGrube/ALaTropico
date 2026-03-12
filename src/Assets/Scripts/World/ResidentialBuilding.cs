using UnityEngine;
using CaudilloBay.Data;
using CaudilloBay.Politics;

namespace CaudilloBay.World
{
    public class ResidentialBuilding : Building
    {
        [Header("Resident Needs")]
        public ResourceType foodType;
        public float foodConsumptionPerMonth = 1.0f;

        [Header("Happiness State")]
        public float currentHappiness = 50f;

        [Header("Faction Leanings")]
        public FactionType primaryFaction = FactionType.Peasants;

        public float GetHappiness()
        {
            // Happiness depends on food availability in building inventory
            if (foodType != null && inventory.HasResource(foodType, foodConsumptionPerMonth))
            {
                currentHappiness = Mathf.MoveTowards(currentHappiness, 100f, 5f);
            }
            else
            {
                currentHappiness = Mathf.MoveTowards(currentHappiness, 0f, 10f);
            }

            return currentHappiness;
        }

        protected override void CompleteConstruction()
        {
            base.CompleteConstruction();
            // Start consuming food if built
        }
    }
}
