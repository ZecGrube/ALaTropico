using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.Data;

namespace CaudilloBay.Politics
{
    public class GlobalEventGenerator : MonoBehaviour
    {
        public static GlobalEventGenerator Instance { get; private set; }

        public List<GlobalEventTemplate> eventTemplates = new List<GlobalEventTemplate>();
        public List<GlobalEventInstance> activeEvents = new List<GlobalEventInstance>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void MonthlyTick()
        {
            UpdateExistingEvents();
            TryGenerateNewEvents();
        }

        private void UpdateExistingEvents()
        {
            for (int i = activeEvents.Count - 1; i >= 0; i--)
            {
                if (activeEvents[i].remainingDuration > 0)
                {
                    activeEvents[i].remainingDuration -= 1.0f;
                    if (activeEvents[i].remainingDuration <= 0)
                    {
                        Debug.Log($"Global Event Expired: {activeEvents[i].resolvedTitle}");
                        activeEvents.RemoveAt(i);
                    }
                }
            }
        }

        private void TryGenerateNewEvents()
        {
            // Check Neighbors
            if (RegionalPoliticsManager.Instance != null)
            {
                foreach (var neighbor in RegionalPoliticsManager.Instance.neighbors)
                {
                    RollForEvent(neighbor.stateName, neighbor);
                }
            }

            // Check Superpowers
            if (GlobalMapManager.Instance != null)
            {
                foreach (var sp in GlobalMapManager.Instance.superpowers)
                {
                    RollForEvent(sp.superpowerName.ToString(), null, sp);
                }
            }
        }

        private void RollForEvent(string countryName, NeighborState neighbor = null, Superpower sp = null)
        {
            foreach (var template in eventTemplates)
            {
                float chance = template.baseSpawnChance;

                // Modifier based on relations if applicable
                if (neighbor != null) chance *= (1.2f - (neighbor.relations / 200f));

                if (Random.value < chance)
                {
                    CreateEvent(template, countryName, neighbor, sp);
                    break; // Max one event per country per month
                }
            }
        }

        private void CreateEvent(GlobalEventTemplate template, string countryName, NeighborState neighbor, Superpower sp)
        {
            GlobalEventInstance instance = new GlobalEventInstance(template, countryName)
            {
                neighborRef = neighbor,
                superpowerRef = sp
            };

            activeEvents.Add(instance);
            Debug.Log($"NEW GLOBAL EVENT: {instance.resolvedTitle}");

            // Notify UI
            if (UI.EventNotificationUI.Instance != null)
            {
                // UI logic to show notification
            }
        }

        public void ResolveEvent(GlobalEventInstance instance, GlobalEventOption option)
        {
            foreach (var effect in option.effects)
            {
                ApplyEffect(instance, effect);
            }
            activeEvents.Remove(instance);
        }

        private void ApplyEffect(GlobalEventInstance instance, GlobalEventEffect effect)
        {
            switch (effect.type)
            {
                case GlobalEventEffect.EffectType.ChangeRelations:
                    if (instance.neighborRef != null) instance.neighborRef.relations += effect.value;
                    else if (instance.superpowerRef != null) instance.superpowerRef.relations += (int)effect.value;
                    break;
                case GlobalEventEffect.EffectType.AddFunds:
                    if (Economy.EconomyManager.Instance != null) Economy.EconomyManager.Instance.AddFunds(effect.value);
                    break;
                case GlobalEventEffect.EffectType.SpawnRefugees:
                    if (AI.MigrationManager.Instance != null) AI.MigrationManager.Instance.AcceptRefugees((int)effect.value);
                    break;
                case GlobalEventEffect.EffectType.AddTech:
                    if (World.TechnologyManager.Instance != null)
                        World.TechnologyManager.Instance.LoadResearchedTechs(new List<string> { effect.data });
                    break;
            }
        }
    }
}
