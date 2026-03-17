using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Politics
{
    public class DynastyManager : MonoBehaviour
    {
        public static DynastyManager Instance { get; private set; }

        public Heir currentRuler;
        public List<Heir> activeHeirs = new List<Heir>();

        private int monthCounter = 0;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void ProcessMonthlyDynasty()
        {
            monthCounter++;
            if (monthCounter >= 12)
            {
                AdvanceDynastyAge();
                monthCounter = 0;
            }

            UpdateHeirSupport();
            CheckForBirth();
        }

        private void CheckForBirth()
        {
            if (currentRuler == null || currentRuler.age > 60 || activeHeirs.Count >= 3) return;

            // 2% chance per month if young ruler
            if (Random.Range(0f, 100f) < 2f)
            {
                AddHeir("Child of El Presidente");
                if (Core.EventManager.Instance != null)
                {
                    var birthEvent = Resources.Load<Data.GameEvent>("Events/royal_birth");
                    if (birthEvent != null) Core.EventManager.Instance.TriggerEvent(birthEvent);
                }
            }
        }

        private void AdvanceDynastyAge()
        {
            if (currentRuler != null) currentRuler.age++;
            foreach (var h in activeHeirs) h.age++;

            // Probability of death for ruler
            if (currentRuler != null && currentRuler.age > 60)
            {
                float deathChance = (currentRuler.age - 60) * 0.05f;
                if (Random.Range(0f, 100f) < deathChance)
                {
                    HandleRulerDeath();
                }
            }
        }

        private void UpdateHeirSupport()
        {
            if (FactionManager.Instance == null) return;
            foreach (var h in activeHeirs) h.UpdateFactionSupport(FactionManager.Instance.factions);
        }

        public void HandleRulerDeath()
        {
            Debug.Log($"El Presidente {currentRuler.name} has passed away.");

            if (activeHeirs.Count > 0)
            {
                // Simple succession: oldest heir
                Heir successor = activeHeirs[0];
                activeHeirs.RemoveAt(0);
                currentRuler = successor;
                Debug.Log($"{currentRuler.name} is the new El Presidente!");

                // Reset Legitimacy and reset faction loyalties slightly
                if (LegitimacySystem.Instance != null)
                    LegitimacySystem.Instance.currentLegitimacy = Mathf.Clamp(successor.charisma, 30, 80);
            }
            else
            {
                Debug.Log("Game Over: No heirs to take the throne.");
                // Trigger Game Over logic
            }
        }

        public void AddHeir(string name)
        {
            Heir h = new Heir { name = name, age = 0 };
            h.GenerateRandomStats();
            activeHeirs.Add(h);
            Debug.Log($"A new heir has been born: {name}!");
        }

        public void AssignSuccessor(Heir heir)
        {
            // Move to front of list for simple succession
            if (activeHeirs.Contains(heir))
            {
                activeHeirs.Remove(heir);
                activeHeirs.Insert(0, heir);
                Debug.Log($"{heir.name} is now the primary successor.");
            }
        }

        public void SendToMilitaryAcademy(Heir heir)
        {
            heir.military = Mathf.Min(heir.military + 20f, 100f);
            Debug.Log($"{heir.name} graduated from the Military Academy.");
        }

        public void TriggerSuccessionCrisis()
        {
            if (activeHeirs.Count < 2) return;

            Debug.LogWarning("SUCCESSION CRISIS! Multiple heirs are fighting for the throne.");
            // Logic to split faction loyalties between heirs
        }

        public void ArrangeMarriage(Heir heir, string foreignDignitary)
        {
            Debug.Log($"{heir.name} has married {foreignDignitary}. International relations improved!");
            // Boost relations with specific neighbor or superpower
        }
    }
}
