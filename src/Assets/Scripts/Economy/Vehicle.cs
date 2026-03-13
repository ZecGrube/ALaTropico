using UnityEngine;
using UnityEngine.AI;
using CaudilloBay.World;
using CaudilloBay.Data;

namespace CaudilloBay.Economy
{
    public enum VehicleState { Idle, MovingToSource, Loading, MovingToDestination, Unloading }

    [RequireComponent(typeof(NavMeshAgent))]
    public class Vehicle : MonoBehaviour
    {
        public VehicleState currentState = VehicleState.Idle;
        public TransportOrder currentOrder;
        public Inventory cargo = new Inventory { maxWeight = 50f };
        public float speed = 10f;

        private NavMeshAgent agent;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.speed = speed;

            if (LogisticsManager.Instance != null)
                LogisticsManager.Instance.registeredVehicles.Add(this);
        }

        public void AssignOrder(TransportOrder order)
        {
            currentOrder = order;
            currentState = VehicleState.MovingToSource;
            agent.SetDestination(order.source.transform.position);
        }

        private void Update()
        {
            if (currentOrder == null) return;

            switch (currentState)
            {
                case VehicleState.MovingToSource:
                    if (agent.remainingDistance < 2f)
                    {
                        currentState = VehicleState.Loading;
                        LoadResources();
                    }
                    break;
                case VehicleState.Loading:
                    currentState = VehicleState.MovingToDestination;
                    agent.SetDestination(currentOrder.destination.transform.position);
                    break;
                case VehicleState.MovingToDestination:
                    if (agent.remainingDistance < 2f)
                    {
                        currentState = VehicleState.Unloading;
                        UnloadResources();
                    }
                    break;
                case VehicleState.Unloading:
                    currentOrder = null;
                    currentState = VehicleState.Idle;
                    break;
            }
        }

        private void LoadResources()
        {
            currentOrder.source.inventory.TransferTo(cargo, currentOrder.resourceType, currentOrder.amount);
        }

        private void UnloadResources()
        {
            cargo.TransferTo(currentOrder.destination.inventory, currentOrder.resourceType, currentOrder.amount);
        }
    }
}
