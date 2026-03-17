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
        private float searchTimer = 0f;
        private float searchInterval = 10f;

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
            searchTimer += Time.deltaTime;
            if (searchTimer >= searchInterval)
            {
                nearestStorage = UnityEngine.Object.FindAnyObjectByType<StorageBuilding>();
                searchTimer = 0f;
            }
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

                    if (!HasRequiredResources())
                    {
                        DetermineNextStep();
                        break;
                    }

                    // Spend resources and add progress
                    ConsumeResourcesForConstruction();
                    currentTarget.AddProgress(Time.deltaTime * buildSpeed);
                    break;
            }
        }

        private void ConsumeResourcesForConstruction()
        {
            if (currentTarget == null) return;
            // Proportionally consume resources relative to time and build speed
            float progressDelta = Time.deltaTime * buildSpeed;
            float totalBuildTime = currentTarget.constructionTime;

            foreach (var cost in currentTarget.buildCosts)
            {
                float amountToConsume = (cost.amount / totalBuildTime) * progressDelta;
                localInventory.RemoveResource(cost.resourceType, amountToConsume);
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
                // Builder only needs SOME progress-worth of resources to continue building
                // If the total cost is 100 but builder capacity is 20, they should build in chunks.
                float requiredForChunk = Mathf.Min(cost.amount * 0.1f, 1f); // 10% or at least 1 unit
                if (localInventory.GetAmount(cost.resourceType) < requiredForChunk) return false;
            }
            return true;
        }

        private void CollectResourcesFromStorage()
        {
            if (nearestStorage == null || currentTarget == null) return;

            foreach (var cost in currentTarget.buildCosts)
            {
                float neededTotal = cost.amount;
                float currentInHand = localInventory.GetAmount(cost.resourceType);

                // Only take what we can carry (maxWeight limit of localInventory)
                float spaceAvailable = localInventory.maxWeight - localInventory.GetTotalWeight();
                float amountToTake = Mathf.Min(neededTotal - currentInHand, spaceAvailable);

                if (amountToTake > 0)
                {
                    nearestStorage.inventory.TransferTo(localInventory, cost.resourceType, amountToTake);
                }
            }
#if UNITY_EDITOR
            Debug.Log($"Builder collected resources from storage for {currentTarget.displayName}. Hand: {localInventory.GetTotalWeight()}");
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
