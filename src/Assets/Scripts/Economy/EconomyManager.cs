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

        public void ProcessMonthlyEconomy(List<Building> buildings)
        {
            float totalMaintenance = 0f;
            float totalPollution = 0f;

            if (AI.PopulationManager.Instance != null)
                AI.PopulationManager.Instance.UpdateGlobalStats();

            foreach (var b in buildings)
            {
                if (b == null || !b.IsConstructed) continue;

                // 1. Maintenance
                totalMaintenance += b.data.maintenanceCost;

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

            treasuryBalance += (grossTax - corruptionLoss - totalMaintenance);
            Debug.Log($"Monthly Economy processed. Maintenance: {totalMaintenance}. Tax: {grossTax}. Corruption Loss: {corruptionLoss}. Treasury: {treasuryBalance}");
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
