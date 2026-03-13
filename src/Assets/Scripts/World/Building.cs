using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.Data;
using CaudilloBay.Economy;
using CaudilloBay.Core;
using CaudilloBay.Politics;

namespace CaudilloBay.World
{
    public abstract class Building : MonoBehaviour, IPlaceable, IBuildable
    {
        [Header("Building Data")]
        public BuildingData data;

        [Header("State")]
        protected float currentProgress = 0f;
        protected bool isConstructed = false;
        public int currentWorkers = 0;

        [Header("Economy")]
        public Inventory inventory = new Inventory();

        // Convenience accessors
        public string buildingId => data.buildingId;
        public string displayName => data.buildingName;
        public Vector2Int footprint => data.footprint;
        public List<ResourceCost> buildCosts => data.buildCosts;
        public float constructionTime => data.buildTime;
        public float pollutionOutput => data.pollutionOutput;

        protected virtual void OnEnable()
        {
            if (StatsManager.Instance != null)
                StatsManager.Instance.RegisterBuilding(this);
        }

        protected virtual void OnDisable()
        {
            if (StatsManager.Instance != null)
                StatsManager.Instance.UnregisterBuilding(this);
        }

        // IPlaceable implementation
        public string Id => buildingId;
        public (int width, int height) Footprint => (footprint.x, footprint.y);
        public (int x, int z) GridPosition { get; set; }
        public bool IsFlippable => true;

        // IBuildable implementation
        public float ConstructionTime => constructionTime;
        public float CurrentProgress { get => currentProgress; set => currentProgress = value; }
        public bool IsConstructed => isConstructed;

        public void AddProgress(float delta)
        {
            if (isConstructed) return;

            currentProgress += delta;
            if (currentProgress >= constructionTime)
            {
                currentProgress = constructionTime;
                CompleteConstruction();
            }
        }

        public bool AreMaterialsProvided()
        {
            foreach (var cost in buildCosts)
            {
                if (inventory.GetAmount(cost.resourceType) < cost.amount) return false;
            }
            return true;
        }

        protected virtual void CompleteConstruction()
        {
            isConstructed = true;
            Debug.Log($"{displayName} construction complete!");

            if (data.storageCapacity > 0)
            {
                inventory.maxWeight = data.storageCapacity;
            }
        }
    }
}
