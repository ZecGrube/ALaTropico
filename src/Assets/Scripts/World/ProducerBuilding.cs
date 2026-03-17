using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.Core;
using CaudilloBay.Data;

namespace CaudilloBay.World
{
    public class ProducerBuilding : Building, IProducer
    {
        public float productionRate = 1.0f;
        public List<CaudilloBay.AI.JobPosition> vacancies = new List<CaudilloBay.AI.JobPosition>();
        public List<CaudilloBay.AI.Citizen> employees = new List<CaudilloBay.AI.Citizen>();
        private float currentCycleTimer = 0f;

        public float ProductionRate => productionRate;
        public bool CanProduce => IsFunctional() && HasInputResources();

        private float logisticsCheckTimer = 0f;
        private float logisticsCheckInterval = 5f;

        private void Update()
        {
            if (IsFunctional())
            {
                logisticsCheckTimer += Time.deltaTime;
                if (logisticsCheckTimer >= logisticsCheckInterval)
                {
                    if (!HasInputResources())
                    {
                        RequestInputs();
                    }

                    if (IsOutputStockHigh())
                    {
                        RequestOutputPickup();
                    }
                    logisticsCheckTimer = 0f;
                }
            }
        }

        private bool IsOutputStockHigh()
        {
            foreach (var output in data.production)
            {
                if (inventory.GetAmount(output.resourceType) >= output.amount * 5f) return true;
            }
            return false;
        }

        private void RequestOutputPickup()
        {
            if (Economy.LogisticsManager.Instance == null) return;

            foreach (var output in data.production)
            {
                if (inventory.GetAmount(output.resourceType) > 0)
                {
                    Building storage = FindStorageForResource(output.resourceType);
                    if (storage != null)
                    {
                        Economy.LogisticsManager.Instance.CreateOrder(this, storage, output.resourceType, inventory.GetAmount(output.resourceType));
                    }
                }
            }
        }

        private Building FindStorageForResource(ResourceType type)
        {
            foreach (var b in StatsManager.Instance.GetTrackedBuildings())
            {
                if (b is StorageBuilding && b.inventory.GetTotalWeight() < b.data.storageCapacity)
                {
                    return b;
                }
            }
            return null;
        }

        private void RequestInputs()
        {
            if (Economy.LogisticsManager.Instance == null) return;

            foreach (var input in data.consumption)
            {
                if (inventory.GetAmount(input.resourceType) < input.amount)
                {
                    // Find a source with the required resource
                    Building source = FindSourceForResource(input.resourceType);
                    if (source != null)
                    {
                        Economy.LogisticsManager.Instance.CreateOrder(source, this, input.resourceType, input.amount);
                    }
                }
            }
        }

        private Building FindSourceForResource(ResourceType type)
        {
            // Simple lookup: in a real project use a specialized manager or distance check
            foreach (var b in StatsManager.Instance.GetTrackedBuildings())
            {
                if (b is StorageBuilding && b.inventory.HasResource(type, 1.0f))
                {
                    return b;
                }
            }
            return null;
        }

        public void ProduceCycle()
        {
            if (!CanProduce) return;

            // Check for strike
            bool onStrike = false;
            foreach(var e in employees) if(e.unionMembership != null && e.unionMembership.isOnStrike) onStrike = true;
            if(onStrike) return;

            ConsumeInputs();
            ProduceOutputsWithEducation();
        }

        public void PostVacancies()
        {
            if (vacancies.Count == 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    var vp = new CaudilloBay.AI.JobPosition("Worker", CaudilloBay.AI.EducationLevel.Basic, 50f, this);
                    vacancies.Add(vp);
                    if (CaudilloBay.AI.JobMarketManager.Instance != null)
                        CaudilloBay.AI.JobMarketManager.Instance.RegisterVacancy(vp);
                }
            }
        }

        private void ProduceOutputsWithEducation()
        {
            float healthMultiplier = 1.0f;
            float employeeEfficiency = employees.Count > 0 ? (float)employees.Count / 10f : 0f; // Mock efficiency
            if (Core.HealthManager.Instance != null)
            {
                healthMultiplier = 0.5f + (Core.HealthManager.Instance.globalHealthLevel / 200f);
            }

            float educationMultiplier = 1.0f;
            if (Core.EducationManager.Instance != null)
            {
                educationMultiplier = 1.0f + (Core.EducationManager.Instance.globalEducationLevel / 200f);
            }

            float modifierMultiplier = 1.0f;
            if (Core.ModifierManager.Instance != null)
            {
                modifierMultiplier += Core.ModifierManager.Instance.GetTotalModifier(Data.ModifierType.ProductionEfficiency);
            }

            foreach (var output in data.production)
            {
                inventory.AddResource(output.resourceType, output.amount * healthMultiplier * educationMultiplier * modifierMultiplier * employeeEfficiency);
            }
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
