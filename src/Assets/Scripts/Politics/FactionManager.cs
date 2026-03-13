using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.World;

namespace CaudilloBay.Politics
{
    public class FactionManager : MonoBehaviour
    {
        public static FactionManager Instance { get; private set; }

        [Header("Factions")]
        public List<FactionData> factions = new List<FactionData>();

        [Header("Politics State")]
        public int currentMandate = 20;
        public float monthlyMandateBase = 5f;

        private float monthlyTimer = 0f;
        public float monthLength = 60f; // 1 minute = 1 month

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Update()
        {
            monthlyTimer += Time.deltaTime;
            if (monthlyTimer >= monthLength)
            {
                StartCoroutine(MonthlyTickCoroutine());
                monthlyTimer = 0f;
            }
        }

        private System.Collections.IEnumerator MonthlyTickCoroutine()
        {
            // Spread expensive calculations over multiple frames
            StatsManager.Instance.RefreshStats();
            yield return null;

            UpdateAllFactions();
            yield return null;

            GenerateMandate();
            CheckDemands();
            yield return null;

            CheckRandomEvents();

            if (CoupManager.Instance != null)
                CoupManager.Instance.CheckCoupConditions();

            Debug.Log($"Monthly Political Update Complete: Mandate {currentMandate}");
        }

        private void UpdateAllFactions()
        {
            foreach (var faction in factions)
            {
                UpdateLoyaltyForFaction(faction);
            }
        }

        private void UpdateLoyaltyForFaction(FactionData faction)
        {
            // Logic based on StatsManager and specific faction needs
            float targetSatisfaction = 50f;

            switch (faction.type)
            {
                case FactionType.Peasants:
                    targetSatisfaction = StatsManager.Instance.averageHappiness;
                    break;
                case FactionType.Capitalists:
                    // Placeholder: Capitalists like low tax (e.g. 100 - tax*100)
                    targetSatisfaction = 70f;
                    break;
                // ... other factions
            }

            faction.needsSatisfaction = Mathf.Lerp(faction.needsSatisfaction, targetSatisfaction, 0.1f);
            faction.loyalty = Mathf.Lerp(faction.loyalty, faction.needsSatisfaction, 0.1f);
        }

        private void GenerateMandate()
        {
            float totalLoyalty = 0;
            foreach(var f in factions) totalLoyalty += f.loyalty;
            float avgLoyalty = factions.Count > 0 ? totalLoyalty / factions.Count : 50f;

            float bonus = (avgLoyalty - 50) / 10f;
            currentMandate += Mathf.RoundToInt(monthlyMandateBase + bonus);
        }

        private void CheckDemands()
        {
            foreach (var faction in factions)
            {
                if (faction.loyalty < 30 && faction.activeDemands.Count == 0)
                {
                    GenerateDemandForFaction(faction);
                }
            }
        }

        private void GenerateDemandForFaction(FactionData faction)
        {
            Demand d = new Demand {
                title = $"{faction.displayName} Demand",
                description = "Build a new service building to satisfy our needs.",
                type = DemandType.BuildBuilding,
                rewardLoyalty = 10,
                penaltyLoyalty = -15
            };
            faction.activeDemands.Add(d);
            Debug.Log($"New Demand from {faction.displayName}!");
        }

        private void CheckRandomEvents()
        {
            // Placeholder
        }

        public void ModifyRelations(FactionType a, FactionType b, float delta)
        {
            FactionData dataA = factions.Find(f => f.type == a);
            if (dataA != null)
            {
                if (dataA.relations.ContainsKey(b)) dataA.relations[b] += delta;
                else dataA.relations.Add(b, delta);
            }
        }

        public void ApplyDecree(Decree decree)
        {
            if (currentMandate >= decree.mandateCost)
            {
                currentMandate -= decree.mandateCost;

                foreach (var effect in decree.loyaltyEffects)
                {
                    FactionData targetFaction = factions.Find(f => f.type == effect.faction);
                    if (targetFaction != null)
                    {
                        targetFaction.loyalty += effect.effect;
                    }
                }

                Debug.Log($"Issued Decree: {decree.decreeName}");
            }
        }
    }
}
