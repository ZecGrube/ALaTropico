using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.World;
using CaudilloBay.Data;

namespace CaudilloBay.Economy
{
    [System.Serializable]
    public class TransportOrder
    {
        public Building source;
        public Building destination;
        public ResourceType resourceType;
        public float amount;
        public bool isAssigned = false;
    }

    public class LogisticsManager : MonoBehaviour
    {
        public static LogisticsManager Instance { get; private set; }

        public List<TransportOrder> pendingOrders = new List<TransportOrder>();
        public List<Vehicle> registeredVehicles = new List<Vehicle>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Update()
        {
            DispatchOrders();
        }

        public void CreateOrder(Building from, Building to, ResourceType type, float amount)
        {
            pendingOrders.Add(new TransportOrder {
                source = from,
                destination = to,
                resourceType = type,
                amount = amount
            });
        }

        private void DispatchOrders()
        {
            foreach (var vehicle in registeredVehicles)
            {
                if (vehicle.currentOrder == null && pendingOrders.Count > 0)
                {
                    TransportOrder order = pendingOrders[0];
                    pendingOrders.RemoveAt(0);
                    vehicle.AssignOrder(order);
                }
            }
        }
    }
}
