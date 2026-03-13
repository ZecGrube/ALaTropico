using UnityEngine;

namespace CaudilloBay.World
{
    public class TourismBuilding : Building
    {
        [Header("Tourism Settings")]
        public float attractivenessBonus = 10f;
        public int capacity = 20;

        protected override void CompleteConstruction()
        {
            base.CompleteConstruction();
            // Register with TouristManager in a real case
        }
    }
}
