using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.Core;
using CaudilloBay.Data;

namespace CaudilloBay.World
{
    public class ProducerBuilding : Building, IProducer
    {
        public float productionRate = 1.0f;
        private float currentCycleTimer = 0f;

        public float ProductionRate => productionRate;
        public bool CanProduce => IsConstructed && HasInputResources();

        private void Update()
        {
            if (IsConstructed && !HasInputResources())
            {
                RequestInputs();
            }
        }

        private void RequestInputs()
        {
            if (Economy.LogisticsManager.Instance == null) return;

            foreach (var input in data.consumption)
            {
                if (inventory.GetAmount(input.resourceType) < input.amount)
                {
                    // Logic to find a storage and create order would go here
                    // LogisticsManager.Instance.CreateOrder(source, this, input.resourceType, input.amount);
                }
            }
        }

        public void ProduceCycle()
        {
            if (!CanProduce) return;
            ConsumeInputs();
            ProduceOutputs();
        }

        private bool HasInputResources()
        {
            if (data == null) return false;
            foreach (var input in data.consumption)
            {
                if (!inventory.HasResource(input.resourceType, input.amount)) return false;
            }
            return true;
        }

        private void ConsumeInputs()
        {
            foreach (var input in data.consumption)
            {
                inventory.RemoveResource(input.resourceType, input.amount);
            }
        }

        private void ProduceOutputs()
        {
            foreach (var output in data.production)
            {
                inventory.AddResource(output.resourceType, output.amount);
            }
        }
    }
}
