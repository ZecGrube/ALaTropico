using UnityEngine;

namespace CaudilloBay.Economy
{
    public class Bus : Vehicle
    {
        public int capacity = 20;
        public int currentPassengers = 0;

        protected override void Start()
        {
            vehicleType = VehicleType.Truck; // Bus uses roads
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
            // Logic for moving between BusStops and loading/unloading citizens
        }
    }
}
