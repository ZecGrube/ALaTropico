using UnityEngine;
using CaudilloBay.Systems;

namespace CaudilloBay.World
{
    public class Barracks : Building
    {
        public Data.UnitType unitTemplate;
        public float trainingSpeed = 1.0f;

        public void TrainUnit()
        {
            if (Politics.MilitaryManager.Instance != null)
            {
                Politics.MilitaryUnit unit = new Politics.MilitaryUnit { type = unitTemplate };
                // Politics.MilitaryManager.Instance.AddUnitToGarrison(unit);
                Debug.Log($"Barracks: Trained new unit {unitTemplate.displayName}");
            }
        }
    }

    public class Stable : Building
    {
        // Cavalry training logic
    }

    public class ArtilleryFoundry : Building
    {
        // Artillery production logic
    }

    public class Fort : Building
    {
        public float defenseBonus = 50f;
        public int garrisonCapacity = 10;

        // Provides global/regional strength
    }
}
