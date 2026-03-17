using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.World;
using CaudilloBay.Politics;

namespace CaudilloBay.Economy
{
    public class EconomyManager : MonoBehaviour
    {
        public static EconomyManager Instance { get; private set; }

        [Header("National Treasury")]
        public float treasuryBalance = 5000f;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void AddFunds(float amount)
        {
            treasuryBalance += amount;
        }

        public void ProcessMonthlyEconomy(List<Building> buildings)
        {
            float totalMaintenance = 0f;
            float totalIncomeTax = 0f;
            float totalUtilityMaintenance = 0f;

            if (AI.PopulationManager.Instance != null)
            {
                foreach (var c in AI.PopulationManager.Instance.allCitizens)
                {
                    if (c.workplace != null)
                    {
                        float tax = c.salary * 0.1f; // 10% flat PIT
                        c.personalWealth -= tax;
                        totalIncomeTax += tax;
                    }
                }
            }
            float totalPollution = 0f;
            float totalCorporateTaxes = 0f;

            if (AI.PopulationManager.Instance != null)
                AI.PopulationManager.Instance.UpdateGlobalStats();

            foreach (var b in buildings)
            {
                if (b == null || !b.IsConstructed) continue;

                // 0. Corporate Profit Handling
                if (b.ownerCorporation != null)
                {
                    // For simplicity, let's say buildings generate some income based on type/production
                    float buildingProfit = 100f; // Placeholder: In real logic, this would be derived from pb.ProduceCycle result
                    float taxAmount = buildingProfit * 0.15f; // 15% corporate tax

                    b.ownerCorporation.treasury += (buildingProfit - taxAmount);
                    totalCorporateTaxes += taxAmount;

                    // Maintenance for corporate buildings is paid by the corporation
                    b.ownerCorporation.treasury -= b.data.maintenanceCost;
                }
                else
                {
                    // 1. Maintenance (Paid by state if not corporate)
                    totalMaintenance += b.data.maintenanceCost;
                }

                // 2. Production/Consumption (for Producers)
                if (b is ProducerBuilding pb)
                {
                    pb.ProduceCycle();
                }

                // 3. Pollution
                totalPollution += b.pollutionOutput;

                // 4. Political Effects (Monthly)
                ApplyMonthlyPoliticalEffects(b);
            }

            // Tax calculation (simplified)
            float grossTax = buildings.Count * 50f; // Mock tax per building
            float corruptionLoss = 0f;
            if (Core.CorruptionManager.Instance != null)
            {
                corruptionLoss = grossTax * (Core.CorruptionManager.Instance.globalCorruptionRate / 100f);
                Core.CorruptionManager.Instance.AddBlackMarketMoney(corruptionLoss * 0.5f); // 50% of corruption loss goes to player shadow funds
            }

            // Add Utility Maintenance
            if (Systems.PowerGridManager.Instance != null || Systems.WaterNetworkManager.Instance != null)
            {
                // Simple placeholder for utility infrastructure upkeep
                totalUtilityMaintenance = buildings.Count * 10f;
            }

            treasuryBalance += (grossTax + totalCorporateTaxes + totalIncomeTax - corruptionLoss - totalMaintenance - totalUtilityMaintenance);

            if (CorporationManager.Instance != null)
                CorporationManager.Instance.ProcessMonthlyFinances();

            Debug.Log($"Monthly Economy processed. Maintenance: {totalMaintenance}. Tax: {grossTax}. Corp Tax: {totalCorporateTaxes}. Corruption Loss: {corruptionLoss}. Treasury: {treasuryBalance}");
        }

        private void ApplyMonthlyPoliticalEffects(Building b)
        {
            if (FactionManager.Instance == null || b.data.loyaltyEffects == null) return;

            foreach (var effect in b.data.loyaltyEffects)
            {
                var faction = FactionManager.Instance.factions.Find(f => f.type == effect.faction);
                if (faction != null)
                {
                    // Monthly effect could be a fraction of the build effect, or a separate field
                    // For now, let's assume loyaltyEffects in BuildingData are applied monthly if the building exists
                    faction.loyalty = Mathf.Clamp(faction.loyalty + effect.effect, 0, 100);
                }
            }
        }
    }
}
