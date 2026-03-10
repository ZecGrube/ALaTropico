using UnityEngine;
using UnityEngine.AI;
using CaudilloBay.World;
using CaudilloBay.Economy;
using CaudilloBay.Data;

namespace CaudilloBay.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class BuilderAI : MonoBehaviour
    {
        public enum BuilderState { Idle, MovingToStorage, MovingToBuild, Building }

        [Header("Settings")]
        public BuilderState currentState = BuilderState.Idle;
        public float buildRange = 2f;
        public float buildSpeed = 1f;

        [Header("Inventory")]
        public Inventory localInventory = new Inventory { maxWeight = 20f };

        private NavMeshAgent agent;
        private Building currentTarget;
        private StorageBuilding nearestStorage;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            // Find initial storage if any
            nearestStorage = UnityEngine.Object.FindAnyObjectByType<StorageBuilding>();
        }

        private void Update()
        {
            UpdateState();
        }

        private void UpdateState()
        {
            switch (currentState)
            {
                case BuilderState.Idle:
                    var task = TaskManager.Instance.RequestTask();
                    if (task != null)
                    {
                        currentTarget = task.Value.targetBuilding;
                        DetermineNextStep();
                    }
                    break;

                case BuilderState.MovingToStorage:
                    if (nearestStorage == null) break;
                    if (Vector3.Distance(transform.position, nearestStorage.transform.position) <= buildRange)
                    {
                        CollectResourcesFromStorage();
                        currentState = BuilderState.MovingToBuild;
                        agent.SetDestination(currentTarget.transform.position);
                    }
                    break;

                case BuilderState.MovingToBuild:
                    if (Vector3.Distance(transform.position, currentTarget.transform.position) <= buildRange)
                    {
                        currentState = BuilderState.Building;
                        agent.ResetPath();
                    }
                    break;

                case BuilderState.Building:
                    if (currentTarget.IsConstructed)
                    {
                        currentState = BuilderState.Idle;
                        currentTarget = null;
                        break;
                    }

                    // Spend resources and add progress
                    currentTarget.AddProgress(Time.deltaTime * buildSpeed);
                    break;
            }
        }

        private void DetermineNextStep()
        {
            // Simplified: for now assume we always need to check storage or go straight if already built
            if (nearestStorage != null && !HasRequiredResources())
            {
                currentState = BuilderState.MovingToStorage;
                agent.SetDestination(nearestStorage.transform.position);
            }
            else
            {
                currentState = BuilderState.MovingToBuild;
                agent.SetDestination(currentTarget.transform.position);
            }
        }

        private bool HasRequiredResources()
        {
            // logic to check if builder has what the building needs
            return true;
        }

        private void CollectResourcesFromStorage()
        {
            // logic to transfer from storage to builder
            Debug.Log("Builder collected resources from storage.");
        }

        public void SetTargetConstruction(Transform target)
        {
            Building b = target.GetComponent<Building>();
            if (b != null)
            {
                currentTarget = b;
                currentState = BuilderState.MovingToBuild;
                agent.SetDestination(currentTarget.transform.position);
            }
        }
    }
}
