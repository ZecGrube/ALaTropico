using UnityEngine;

namespace CaudilloBay.World
{
    public class StorageBuilding : Building
    {
        protected override void CompleteConstruction()
        {
            base.CompleteConstruction();
            // Capacity is set in base class using data.storageCapacity
        }
    }
}
