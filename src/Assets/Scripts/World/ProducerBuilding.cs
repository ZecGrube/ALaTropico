using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.Core;
using CaudilloBay.Data;

namespace CaudilloBay.World
{
    public class ProducerBuilding : Building, IProducer
    {
        [Header("Production Settings")]
        public ProductionRecipe recipe;
        public float productionRate = 1.0f;

        private float currentCycleTimer = 0f;

        public float ProductionRate => productionRate;
        public bool CanProduce => IsConstructed && HasInputResources();

        private void Update()
        {
            if (CanProduce)
            {
                currentCycleTimer += Time.deltaTime * productionRate;
                if (currentCycleTimer >= recipe.cycleTime)
                {
                    ProduceCycle();
                    currentCycleTimer = 0f;
                }
            }
        }

        public void ProduceCycle()
        {
            ConsumeInputs();
            ProduceOutputs();
        }

        private bool HasInputResources()
        {
            foreach (var input in recipe.inputs)
            {
                if (!inventory.HasResource(input.resourceType, input.amount)) return false;
            }
            return true;
        }

        private void ConsumeInputs()
        {
            foreach (var input in recipe.inputs)
            {
                inventory.RemoveResource(input.resourceType, input.amount);
            }
        }

        private void ProduceOutputs()
        {
            foreach (var output in recipe.outputs)
            {
                inventory.AddResource(output.resourceType, output.amount);
            }
        }
    }

    [System.Serializable]
    public struct ProductionRecipe
    {
        public List<ResourceCost> inputs;
        public List<ResourceCost> outputs;
        public float cycleTime;
    }
}
