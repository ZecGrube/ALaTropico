using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.World;
using CaudilloBay.Data;
using CaudilloBay.Economy;

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

        private bool isTickRunning = false;
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
            if (isTickRunning) yield break;
            isTickRunning = true;

            // Spread expensive calculations over multiple frames
            StatsManager.Instance.RefreshStats();
            yield return null;

            if (EconomyManager.Instance != null)
            {
                EconomyManager.Instance.ProcessMonthlyEconomy(StatsManager.Instance.GetTrackedBuildings());
            }
            yield return null;

            UpdateAllFactions();
            yield return null;

            GenerateMandate();
            CheckDemands();
            yield return null;

            if (Core.EventManager.Instance != null)
                Core.EventManager.Instance.CheckForEvents();

            if (Core.ModifierManager.Instance != null)
                Core.ModifierManager.Instance.ProcessMonthlyTick();

            if (Core.CrimeManager.Instance != null)
                Core.CrimeManager.Instance.ProcessMonthlyCrime();

            if (Core.EducationManager.Instance != null)
                Core.EducationManager.Instance.ProcessMonthlyEducation();

            if (Core.HealthManager.Instance != null)
                Core.HealthManager.Instance.ProcessMonthlyHealth();

            if (Core.CultureManager.Instance != null)
                Core.CultureManager.Instance.ProcessMonthlyCulture();

            if (Core.CorruptionManager.Instance != null)
                Core.CorruptionManager.Instance.ProcessMonthlyCorruption();

            if (DynastyManager.Instance != null)
                DynastyManager.Instance.ProcessMonthlyDynasty();

            if (Core.ReligionManager.Instance != null)
                Core.ReligionManager.Instance.ProcessMonthlyUpdate();

            if (Core.PersonalityCultManager.Instance != null)
                Core.PersonalityCultManager.Instance.ProcessMonthlyUpdate();

            CheckRandomEvents();

            if (CoupManager.Instance != null)
                CoupManager.Instance.CheckCoupConditions();

            if (Core.CampaignManager.Instance != null)
                Core.CampaignManager.Instance.CheckObjectives();

            isTickRunning = false;
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

            if (AI.PopulationManager.Instance != null)
            {
                // Incorporate overall citizen satisfaction
                targetSatisfaction = Mathf.Lerp(targetSatisfaction, AI.PopulationManager.Instance.averageSatisfaction, 0.5f);
            }

            switch (faction.type)
            {
                case FactionType.Peasants:
                    targetSatisfaction = Mathf.Lerp(targetSatisfaction, StatsManager.Instance.averageHappiness, 0.5f);
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
                bool found = false;
                for (int i = 0; i < dataA.relationsList.Count; i++)
                {
                    if (dataA.relationsList[i].faction == b)
                    {
                        var entry = dataA.relationsList[i];
                        entry.value += delta;
                        dataA.relationsList[i] = entry;
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    dataA.relationsList.Add(new FactionData.RelationEntry { faction = b, value = delta });
                }
            }
        }

        public void BribeFaction(FactionType type, float amount)
        {
            if (Core.CorruptionManager.Instance != null && Core.CorruptionManager.Instance.SpendBlackMarketMoney(amount))
            {
                var faction = factions.Find(f => f.type == type);
                if (faction != null)
                {
                    faction.loyalty = Mathf.Min(faction.loyalty + (amount / 100f), 100f);
                    Debug.Log($"Bribed {type} for ${amount}. New loyalty: {faction.loyalty}");
                }
            }
        }

        public void AddLoyalty(FactionType type, float delta)
        {
            var faction = factions.Find(f => f.type == type);
            if (faction != null)
            {
                faction.loyalty = Mathf.Clamp(faction.loyalty + delta, 0, 100);
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

        public List<SaveSystem.RelationSaveData> GetRelationSaveData()
        {
            List<SaveSystem.RelationSaveData> list = new List<SaveSystem.RelationSaveData>();
            foreach (var f in factions)
            {
                foreach (var rel in f.relationsList)
                {
                    list.Add(new SaveSystem.RelationSaveData { factionA = f.type, factionB = rel.faction, value = rel.value });
                }
            }
            return list;
        }

        public void LoadRelationSaveData(List<SaveSystem.RelationSaveData> data)
        {
            foreach (var d in data)
            {
                var f = factions.Find(x => x.type == d.factionA);
                if (f != null)
                {
                    bool found = false;
                    for (int i = 0; i < f.relationsList.Count; i++)
                    {
                        if (f.relationsList[i].faction == d.factionB)
                        {
                            var entry = f.relationsList[i];
                            entry.value = d.value;
                            f.relationsList[i] = entry;
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        f.relationsList.Add(new FactionData.RelationEntry { faction = d.factionB, value = d.value });
                    }
                }
            }
        }

        public Heir GetPreferredHeir(FactionData faction)
        {
            if (DynastyManager.Instance == null) return null;
            Heir best = null;
            float maxSupport = -1f;

            foreach (var h in DynastyManager.Instance.activeHeirs)
            {
                foreach (var entry in h.factionSupportList)
                {
                    if (entry.faction == faction.type)
                    {
                        if (entry.value > maxSupport)
                        {
                            maxSupport = entry.value;
                            best = h;
                        }
                        break;
                    }
                }
            }
            return best;
        }
    }
}
