using UnityEngine;

namespace CaudilloBay.World
{
    public class StorageBuilding : Building
    {
        [Header("Storage Settings")]
        public float storageCapacity = 1000f;

        protected override void CompleteConstruction()
        {
            base.CompleteConstruction();
            inventory.maxWeight = storageCapacity;
            // Register with a global logistics manager if needed
        }
    }
}
