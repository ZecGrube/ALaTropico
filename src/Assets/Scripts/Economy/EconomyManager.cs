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
        public float taxPerCitizen = 12f;

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

                totalMaintenance += b.data.maintenanceCost;

                if (b is ProducerBuilding pb)
                {
                    pb.ProduceCycle();
                }

                if (b is Museum museum)
                {
                    museum.ProcessMonthlyArtifacts();
                }

                if (b is ShadowEconomyBuilding shadow)
                {
                    shadow.ProcessMonthlyShadowIncome();
                }

                totalPollution += b.pollutionOutput;

                ApplyMonthlyPoliticalEffects(b);
            }

            float grossTaxRevenue = CalculateGrossTaxes();
            float netTaxRevenue = grossTaxRevenue;
            if (CorruptionManager.Instance != null)
            {
                netTaxRevenue = CorruptionManager.Instance.ApplyTaxLeakage(grossTaxRevenue);
            }

            float militaryUpkeep = 0f;
            if (MilitaryManager.Instance != null)
            {
                militaryUpkeep = MilitaryManager.Instance.CalculateMonthlyUpkeep();
                MilitaryManager.Instance.ApplyMonthlyBudgetPressure(treasuryBalance);
            }

            float cultureSpending = 0f;
            if (CultureManager.Instance != null)
            {
                CultureManager.Instance.ProcessMonthlyCulture();
                cultureSpending = CultureManager.Instance.monthlySubsidyCost;
            }

            treasuryBalance += netTaxRevenue;
            treasuryBalance -= totalMaintenance + militaryUpkeep + cultureSpending;

            Debug.Log($"Monthly Economy processed. Taxes(gross/net): {grossTaxRevenue}/{netTaxRevenue}, Maintenance: {totalMaintenance}, Military: {militaryUpkeep}, Culture: {cultureSpending}. Treasury: {treasuryBalance}");
        }

        private float CalculateGrossTaxes()
        {
            if (AI.PopulationManager.Instance == null) return 0f;
            return AI.PopulationManager.Instance.allCitizens.Count * taxPerCitizen;
        }

        private void ApplyMonthlyPoliticalEffects(Building b)
        {
            if (FactionManager.Instance == null || b.data.loyaltyEffects == null) return;

            foreach (var effect in b.data.loyaltyEffects)
            {
                var faction = FactionManager.Instance.factions.Find(f => f.type == effect.faction);
                if (faction != null)
                {
                    faction.loyalty = Mathf.Clamp(faction.loyalty + effect.effect, 0, 100);
                }
            }
        }
    }
}
