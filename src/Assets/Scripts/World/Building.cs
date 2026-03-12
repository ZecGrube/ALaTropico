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
        [Header("General Settings")]
        public string buildingId;
        public string displayName;
        public Vector2Int footprint = new Vector2Int(1, 1);

        [Header("Construction")]
        public List<ResourceCost> buildCosts;
        public float constructionTime = 10f;
        private float currentProgress = 0f;
        private bool isConstructed = false;

        [Header("Economy")]
        public Inventory inventory = new Inventory();

        [Header("Politics")]
        public List<FactionType> favoredFactions;
        public List<FactionType> dislikedFactions;

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

            // Progress is only made if materials are provided (already checked by Builder AI usually)
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
        }
    }

    [System.Serializable]
    public struct ResourceCost
    {
        public ResourceType resourceType;
        public float amount;
    }
}
