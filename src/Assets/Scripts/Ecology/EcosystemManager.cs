using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using CaudilloBay.World;
using CaudilloBay.Politics;

namespace CaudilloBay.Ecology
{
    public class EcosystemManager : MonoBehaviour
    {
        public static EcosystemManager Instance { get; private set; }

        public List<EcosystemZone> zones = new List<EcosystemZone>();
        public List<AnimalSpecies> allSpecies = new List<AnimalSpecies>();

        [Header("Global Modifiers")]
        public float globalReforestationRate = 0f;
        public float antiPoachingStrength = 0f;
        public bool isHuntingLegal = false;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void InitializeZones(int width, int height)
        {
            zones.Clear();
            int zoneCount = Random.Range(4, 8);
            for (int i = 0; i < zoneCount; i++)
            {
                EcosystemZone zone = new EcosystemZone {
                    zoneId = "zone_" + i,
                    type = (EcosystemType)Random.Range(0, 5),
                    center = new Vector2(Random.Range(0, width), Random.Range(0, height)),
                    radius = Random.Range(15f, 35f),
                    treeCover = Random.Range(10f, 90f),
                    health = Random.Range(70f, 100f),
                    biodiversity = Random.Range(60f, 90f)
                };

                // Initialize animal populations
                foreach(var s in allSpecies)
                {
                    if (Random.value > 0.3f)
                        zone.animalPopulations[s.speciesId] = Random.Range(5, 50);
                }

                zones.Add(zone);
            }
            Debug.Log($"Initialized {zones.Count} ecosystem zones.");
        }

        public void ProcessMonthlyTick()
        {
            float pollution = 0;
            if (StatsManager.Instance != null) pollution = StatsManager.Instance.globalPollution;

            foreach (var zone in zones)
            {
                zone.ProcessMonthlyTick(pollution, globalReforestationRate, antiPoachingStrength);
                UpdateAnimalPopulations(zone);
            }

            ApplyFactionConsequences();
        }

        private void UpdateAnimalPopulations(EcosystemZone zone)
        {
            foreach (var species in allSpecies)
            {
                if (!zone.animalPopulations.ContainsKey(species.speciesId)) continue;

                int pop = zone.animalPopulations[species.speciesId];
                if (pop <= 0) continue;

                float growth = pop * species.birthRate * (zone.biodiversity / 100f);
                float deaths = pop * species.deathRate * (1f + (100f - zone.health) / 100f);

                // Poaching vs Legal Hunting
                float extraLoss = 0f;
                if (isHuntingLegal)
                {
                    extraLoss = pop * 0.05f; // Legal quota
                }
                else
                {
                    // Poaching risk decreases with anti-poaching strength
                    extraLoss = (100f - antiPoachingStrength) * 0.001f * pop * (species.trophyValue / 50f);
                }

                zone.animalPopulations[species.speciesId] = Mathf.Max(0, Mathf.RoundToInt(pop + growth - deaths - extraLoss));
            }
        }

        private void ApplyFactionConsequences()
        {
            if (FactionManager.Instance == null) return;

            float avgBiodiversity = GetTotalBiodiversity();

            // Ecologists like high biodiversity
            float ecoDelta = (avgBiodiversity - 70f) * 0.05f;
            ModifyFactionLoyalty(FactionType.Environmentalists, ecoDelta);

            // New Faction "Hunters" (using Criminals as placeholder if not added yet, or just generic)
            if (isHuntingLegal)
                ModifyFactionLoyalty(FactionType.Nationalists, 0.5f); // Hunters like hunting
        }

        private void ModifyFactionLoyalty(FactionType type, float delta)
        {
            var faction = FactionManager.Instance.factions.Find(f => f.type == type);
            if (faction != null) faction.loyalty = Mathf.Clamp(faction.loyalty + delta, 0, 100);
        }

        public float GetTotalBiodiversity()
        {
            if (zones.Count == 0) return 0;
            return zones.Average(z => z.biodiversity);
        }

        public float GetEcoTourismBonus()
        {
            float bonus = 0;
            foreach (var zone in zones)
            {
                bonus += (zone.biodiversity * 0.15f) * (zone.health / 100f);
                foreach (var speciesPop in zone.animalPopulations)
                {
                    var species = allSpecies.Find(s => s.speciesId == speciesPop.Key);
                    if (species != null) bonus += species.ecoTourismBonus * (speciesPop.Value / 50f);
                }
            }
            return bonus;
        }

        public void Reforest(Vector2 center, float radius, float strength)
        {
            foreach(var zone in zones)
            {
                if (Vector2.Distance(center, zone.center) < radius + zone.radius)
                {
                    zone.treeCover = Mathf.Clamp(zone.treeCover + strength, 0, 100);
                    zone.health = Mathf.Clamp(zone.health + strength * 0.5f, 0, 100);
                }
            }
        }
    }
}
