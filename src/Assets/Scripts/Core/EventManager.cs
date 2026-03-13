using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.Data;
using CaudilloBay.Politics;

namespace CaudilloBay.Core
{
    public class EventManager : MonoBehaviour
    {
        public static EventManager Instance { get; private set; }

        public List<GameEvent> allEvents = new List<GameEvent>();
        private List<GameEvent> activeEventQueue = new List<GameEvent>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void CheckForEvents()
        {
            // Monthly check logic
            float roll = Random.Range(0f, 100f);
            if (roll < 20f) // 20% chance of an event per month
            {
                TriggerRandomEvent();
            }
        }

        public void TriggerRandomEvent()
        {
            List<GameEvent> possibleEvents = new List<GameEvent>();
            foreach (var ev in allEvents)
            {
                if (IsEventPossible(ev)) possibleEvents.Add(ev);
            }

            if (possibleEvents.Count > 0)
            {
                // Simple weighted random
                GameEvent selected = possibleEvents[Random.Range(0, possibleEvents.Count)];
                TriggerEvent(selected);
            }
        }

        private bool IsEventPossible(GameEvent ev)
        {
            if (LegitimacySystem.Instance != null && LegitimacySystem.Instance.currentLegitimacy < ev.minLegitimacy) return false;
            if (FactionManager.Instance != null && FactionManager.Instance.currentMandate < ev.minMandate) return false;

            foreach (var req in ev.factionRequirements)
            {
                var faction = FactionManager.Instance.factions.Find(f => f.type == req.factionType);
                if (faction == null || faction.loyalty < req.minLoyalty) return false;
            }

            return true;
        }

        public void TriggerEvent(GameEvent ev)
        {
            Debug.Log($"Event Triggered: {ev.title}");
            if (UI.EventNotificationUI.Instance != null)
            {
                UI.EventNotificationUI.Instance.ShowEvent(ev);
            }
            else
            {
                // Fallback: auto-pick first choice if no UI
                ApplyChoiceEffects(ev.choices[0]);
            }
        }

        public void ApplyChoiceEffects(EventChoice choice)
        {
            if (LegitimacySystem.Instance != null)
                LegitimacySystem.Instance.currentLegitimacy += choice.legitimacyChange;

            if (FactionManager.Instance != null)
                FactionManager.Instance.currentMandate += choice.mandateChange;

            if (World.StatsManager.Instance != null)
            {
                foreach (var rc in choice.resourceChanges)
                {
                    World.StatsManager.Instance.AddResource(rc.resourceId, rc.amount);
                }
            }

            if (ModifierManager.Instance != null)
            {
                foreach (var mod in choice.modifiers)
                {
                    ModifierManager.Instance.AddModifier(mod);
                }
            }
        }
    }
}
