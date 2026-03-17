using UnityEngine;

namespace CaudilloBay.Economy
{
    public class GarbageTruck : Vehicle
    {
        protected override void Start()
        {
            vehicleType = VehicleType.Truck; // Garbage truck is a specialized truck
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
            if (currentState == VehicleState.Loading && currentOrder != null)
            {
                WasteManager.Instance.CollectGarbage(currentOrder.source, cargo.maxWeight);
            }
        }
    }
}
