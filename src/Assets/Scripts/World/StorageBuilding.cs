using UnityEngine;
using CaudilloBay.Data;
using CaudilloBay.Economy;

namespace CaudilloBay.World
{
    public class StorageBuilding : Building
    {
        [Header("Market Settings")]
        public bool isPublicMarket = false;

        protected override void CompleteConstruction()
        {
            base.CompleteConstruction();
            // Register with a global logistics manager if needed
        }

        public void RequestRefill(ResourceType type, float amount)
        {
            // Find a source and create order
            // This would be called by the LogisticsManager scan or the building itself
        }
    }
}
