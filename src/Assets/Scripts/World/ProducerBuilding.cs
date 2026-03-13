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
            // The EconomyManager handles monthly production now, but we keep this for real-time visual feedback if needed
            // Or we can move logic entirely to EconomyManager
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
