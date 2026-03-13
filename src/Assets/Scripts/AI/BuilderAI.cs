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
                        if (currentTarget != null)
                        {
                            DetermineNextStep();
                        }
                    }
                    break;

                case BuilderState.MovingToStorage:
                    if (nearestStorage == null)
                    {
                        currentState = BuilderState.Idle;
                        break;
                    }

                    if (Vector3.Distance(transform.position, nearestStorage.transform.position) <= buildRange)
                    {
                        CollectResourcesFromStorage();

                        if (currentTarget != null)
                        {
                            currentState = BuilderState.MovingToBuild;
                            agent.SetDestination(currentTarget.transform.position);
                        }
                        else
                        {
                            currentState = BuilderState.Idle;
                        }
                    }
                    break;

                case BuilderState.MovingToBuild:
                    if (currentTarget == null)
                    {
                        currentState = BuilderState.Idle;
                        agent.ResetPath();
                        break;
                    }

                    if (Vector3.Distance(transform.position, currentTarget.transform.position) <= buildRange)
                    {
                        currentState = BuilderState.Building;
                        agent.ResetPath();
                    }
                    break;

                case BuilderState.Building:
                    if (currentTarget == null || currentTarget.IsConstructed)
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
            if (currentTarget == null)
            {
                currentState = BuilderState.Idle;
                return;
            }

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
            if (currentTarget == null) return true;
            foreach (var cost in currentTarget.buildCosts)
            {
                if (localInventory.GetAmount(cost.resourceType) < cost.amount) return false;
            }
            return true;
        }

        private void CollectResourcesFromStorage()
        {
            if (nearestStorage == null || currentTarget == null) return;

            foreach (var cost in currentTarget.buildCosts)
            {
                float needed = cost.amount - localInventory.GetAmount(cost.resourceType);
                if (needed > 0)
                {
                    nearestStorage.inventory.TransferTo(localInventory, cost.resourceType, needed);
                }
            }
#if UNITY_EDITOR
            Debug.Log($"Builder collected resources from storage for {currentTarget.displayName}.");
#endif
        }

        public void SetTargetConstruction(Transform target)
        {
            if (target == null) return;

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
