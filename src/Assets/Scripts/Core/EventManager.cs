using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.Data;
using CaudilloBay.Politics;

namespace CaudilloBay.Core
{
    public enum InvasionAction
    {
        Resist,
        Negotiate,
        RequestAid
    }

    public class EventManager : MonoBehaviour
    {
        public static EventManager Instance { get; private set; }

        public List<GameEvent> allEvents = new List<GameEvent>();
        private List<GameEvent> activeEventQueue = new List<GameEvent>();

        [Header("Invasion")]
        public float invasionBaseChance = 5f;
        public Invasion currentInvasion;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void CheckForEvents()
        {
            CheckForInvasion();
            CheckForCorruptionScandal();
            CheckDynastyEvents();

            float roll = Random.Range(0f, 100f);
            if (roll < 20f)
            {
                TriggerRandomEvent();
            }
        }

        private void CheckForInvasion()
        {
            if (currentInvasion != null) return;

            float defense = MilitaryManager.Instance != null ? MilitaryManager.Instance.CalculateDefensePower() : 0f;
            float allianceReduction = (GlobalMapManager.Instance != null) ? GlobalMapManager.Instance.alliedSuperpowers.Count * 1.5f : 0f;
            float invasionChance = Mathf.Clamp(invasionBaseChance + Mathf.Max(0f, 20f - defense * 0.1f) - allianceReduction, 1f, 35f);

            if (Random.Range(0f, 100f) > invasionChance) return;

            SuperpowerType invader = SuperpowerType.USA;
            if (GlobalMapManager.Instance != null && GlobalMapManager.Instance.superpowers.Count > 0)
            {
                invader = GlobalMapManager.Instance.superpowers[Random.Range(0, GlobalMapManager.Instance.superpowers.Count)].type;
            }

            float strength = Random.Range(40f, 120f);
            currentInvasion = new Invasion(invader, strength, "Capital", Random.Range(1, 4), Random.Range(0, 100) < 60);

            if (UI.InvasionUI.Instance != null)
            {
                UI.InvasionUI.Instance.ShowInvasion(currentInvasion);
            }
            else
            {
                ResolveCurrentInvasion(InvasionAction.Resist);
            }
        }

        public void ResolveCurrentInvasion(InvasionAction action)
        {
            if (currentInvasion == null) return;

            float defensePower = MilitaryManager.Instance != null ? MilitaryManager.Instance.CalculateDefensePower() : 0f;

            switch (action)
            {
                case InvasionAction.RequestAid:
                    if (GlobalMapManager.Instance != null)
                    {
                        defensePower += GlobalMapManager.Instance.RequestMilitaryAid(currentInvasion.invader);
                    }
                    goto case InvasionAction.Resist;

                case InvasionAction.Resist:
                    if (defensePower >= currentInvasion.invasionStrength)
                    {
                        if (MilitaryManager.Instance != null) MilitaryManager.Instance.UpdateArmyLoyalty(5f);
                        if (LegitimacySystem.Instance != null) LegitimacySystem.Instance.currentLegitimacy += 3f;
                    }
                    else
                    {
                        ApplyInvasionDefeatConsequences();
                    }
                    break;

                case InvasionAction.Negotiate:
                    if (Economy.EconomyManager.Instance != null)
                        Economy.EconomyManager.Instance.treasuryBalance -= currentInvasion.reparationsCost;
                    if (LegitimacySystem.Instance != null)
                        LegitimacySystem.Instance.currentLegitimacy -= 2f;
                    break;
            }

            currentInvasion = null;
            if (MilitaryManager.Instance != null) MilitaryManager.Instance.ClearForeignSupport();
        }

        private void ApplyInvasionDefeatConsequences()
        {
            if (Economy.EconomyManager.Instance != null)
                Economy.EconomyManager.Instance.treasuryBalance -= currentInvasion.reparationsCost;

            if (LegitimacySystem.Instance != null)
                LegitimacySystem.Instance.currentLegitimacy -= 8f;

            if (MilitaryManager.Instance != null)
                MilitaryManager.Instance.UpdateArmyLoyalty(-8f);

            if (CultureManager.Instance != null)
                CultureManager.Instance.ApplyDestructionPenalty(currentInvasion.isNaval ? 6f : 4f);
        }

        private void CheckForCorruptionScandal()
        {
            if (CorruptionManager.Instance == null) return;

            float corruption = CorruptionManager.Instance.corruptionLevel;
            if (corruption < 70f) return;

            float antiCorruptionGuard = 0f;
            if (World.StatsManager.Instance != null)
            {
                foreach (var b in World.StatsManager.Instance.GetTrackedBuildings())
                {
                    if (b is World.PoliceHQ) antiCorruptionGuard += 10f;
                }
            }

            float scandalChance = Mathf.Clamp((corruption - 60f) - antiCorruptionGuard, 2f, 40f);
            if (Random.Range(0f, 100f) > scandalChance) return;

            if (LegitimacySystem.Instance != null)
                LegitimacySystem.Instance.currentLegitimacy -= 6f;

            if (FactionManager.Instance != null)
            {
                foreach (var faction in FactionManager.Instance.factions)
                {
                    if (faction.type != FactionType.Criminals)
                        faction.loyalty = Mathf.Clamp(faction.loyalty - 4f, 0f, 100f);
                }
            }

            if (GlobalMapManager.Instance != null)
            {
                foreach (var superpower in GlobalMapManager.Instance.superpowers)
                {
                    superpower.relations -= 4f;
                }
            }

            Debug.Log("Corruption scandal exposed! Legitimacy, loyalty and foreign relations decreased.");
        }

        private void CheckDynastyEvents()
        {
            if (DynastyManager.Instance == null) return;

            DynastyManager.Instance.CheckSuccession();

            if (DynastyManager.Instance.currentRuler != null && DynastyManager.Instance.currentRuler.isAlive && DynastyManager.Instance.currentRuler.age > 72f)
            {
                float deathByAgeChance = Mathf.Clamp((DynastyManager.Instance.currentRuler.age - 70) * 3f, 2f, 45f);
                if (Random.Range(0f, 100f) < deathByAgeChance)
                {
                    DynastyManager.Instance.currentRuler.isAlive = false;
                    DynastyManager.Instance.CheckSuccession();
                    Debug.Log("Dynasty event: ruler died of old age.");
                }
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
