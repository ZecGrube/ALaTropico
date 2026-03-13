using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Politics
{
    public class DynastyManager : MonoBehaviour
    {
        public static DynastyManager Instance { get; private set; }

        [Header("Dynasty")]
        public Heir currentRuler;
        public List<Heir> heirs = new List<Heir>();
        public int monthsSinceYearTick = 0;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            if (currentRuler == null)
            {
                currentRuler = new Heir
                {
                    heirName = "El Presidente",
                    age = 45,
                    gender = HeirGender.Male,
                    isAlive = true,
                    loyaltyToRuler = 100f
                };
                currentRuler.GenerateRandomStats();
            }
        }

        public void AddHeir(Heir heir)
        {
            if (heir == null) return;
            heirs.Add(heir);
            UpdateHeirSupportFromFactions();
        }

        public void RemoveHeir(Heir heir)
        {
            if (heir == null) return;
            heirs.Remove(heir);
        }

        public bool AppointSuccessor(Heir heir)
        {
            if (heir == null || !heirs.Contains(heir) || !heir.isAlive) return false;

            heir.loyaltyToRuler = Mathf.Clamp(heir.loyaltyToRuler + 15f, 0f, 100f);
            heir.charisma = Mathf.Clamp(heir.charisma + 5f, 0f, 100f);
            Debug.Log($"Successor appointed: {heir.heirName}");
            return true;
        }

        public Heir AdoptHeir(string name)
        {
            Heir adopted = new Heir
            {
                heirName = name,
                age = Random.Range(8, 20),
                gender = (HeirGender)Random.Range(0, 3),
                isAlive = true,
                loyaltyToRuler = 75f
            };
            adopted.GenerateRandomStats();
            AddHeir(adopted);
            return adopted;
        }

        public bool SendHeirToMilitaryAcademy(Heir heir)
        {
            if (heir == null || !heir.isAlive) return false;
            heir.military = Mathf.Clamp(heir.military + 12f, 0f, 100f);
            heir.charisma = Mathf.Clamp(heir.charisma + 4f, 0f, 100f);
            heir.loyaltyToRuler = Mathf.Clamp(heir.loyaltyToRuler + 3f, 0f, 100f);
            return true;
        }

        public void ProcessMonthlyDynasty()
        {
            monthsSinceYearTick++;
            if (monthsSinceYearTick >= 12)
            {
                monthsSinceYearTick = 0;
                AdvanceAge();
            }

            CheckBirthEvent();
            CheckHeirDeathEvents();
            CheckConspiracyEvent();
            CheckSuccession();
        }

        public void AdvanceAge()
        {
            if (currentRuler != null && currentRuler.isAlive) currentRuler.age += 1;
            foreach (var heir in heirs)
            {
                if (heir.isAlive) heir.age += 1;
            }
        }

        public void CheckSuccession()
        {
            if (currentRuler != null && currentRuler.isAlive) return;
            HandleRulerDeath();
        }

        public void HandleRulerDeath()
        {
            List<Heir> livingHeirs = heirs.FindAll(h => h != null && h.isAlive);
            if (livingHeirs.Count == 0)
            {
                if (LegitimacySystem.Instance != null) LegitimacySystem.Instance.currentLegitimacy -= 20f;
                Debug.Log("Dynasty crisis: no heirs available. Regency crisis started.");
                return;
            }

            livingHeirs.Sort((a, b) => b.GetAverageSupport().CompareTo(a.GetAverageSupport()));
            Heir winner = livingHeirs[0];

            // If top two are close, trigger succession crisis penalties
            if (livingHeirs.Count > 1)
            {
                float diff = winner.GetAverageSupport() - livingHeirs[1].GetAverageSupport();
                if (diff < 8f)
                {
                    if (LegitimacySystem.Instance != null) LegitimacySystem.Instance.currentLegitimacy -= 10f;
                    if (FactionManager.Instance != null)
                    {
                        foreach (var faction in FactionManager.Instance.factions)
                        {
                            faction.loyalty = Mathf.Clamp(faction.loyalty - 5f, 0f, 100f);
                        }
                    }
                    Debug.Log("Succession crisis: multiple heirs contest the throne.");
                }
            }

            currentRuler = winner;
            heirs.Remove(winner);
            currentRuler.loyaltyToRuler = 100f;
            ApplyNewRulerPoliticalReset(currentRuler);
            Debug.Log($"Power transferred to heir: {currentRuler.heirName}");
        }

        public void UpdateHeirSupportFromFactions()
        {
            if (FactionManager.Instance == null) return;

            foreach (var heir in heirs)
            {
                heir.UpdateSupportBasedOnFactions(FactionManager.Instance.factions);
            }

            if (currentRuler != null)
                currentRuler.UpdateSupportBasedOnFactions(FactionManager.Instance.factions);
        }

        private void CheckBirthEvent()
        {
            if (currentRuler == null || !currentRuler.isAlive) return;
            if (currentRuler.age >= 60) return;

            if (heirs.Count == 0 && Random.Range(0f, 100f) < 6f)
            {
                Heir newborn = new Heir
                {
                    heirName = $"Heir {Random.Range(100, 999)}",
                    age = 0,
                    gender = (HeirGender)Random.Range(0, 3),
                    isAlive = true
                };
                newborn.GenerateRandomStats();
                AddHeir(newborn);

                if (FactionManager.Instance != null)
                {
                    foreach (var faction in FactionManager.Instance.factions)
                    {
                        faction.loyalty = Mathf.Clamp(faction.loyalty + 2f, 0f, 100f);
                    }
                }

                Debug.Log("Dynasty event: newborn heir added.");
            }
        }

        private void CheckHeirDeathEvents()
        {
            foreach (var heir in heirs)
            {
                if (!heir.isAlive) continue;

                float deathChance = heir.age < 50 ? 0.2f : (heir.age - 45) * 0.25f;
                if (Random.Range(0f, 100f) < deathChance * 0.05f)
                {
                    heir.isAlive = false;
                    if (LegitimacySystem.Instance != null) LegitimacySystem.Instance.currentLegitimacy -= 1f;
                    Debug.Log($"Dynasty tragedy: heir {heir.heirName} died.");
                }
            }
        }

        private void CheckConspiracyEvent()
        {
            foreach (var heir in heirs)
            {
                if (!heir.isAlive) continue;
                if (heir.loyaltyToRuler > 35f || heir.cruelty < 70f) continue;

                float conspiracyChance = ((100f - heir.loyaltyToRuler) * 0.05f) + ((heir.cruelty - 60f) * 0.04f);
                if (Random.Range(0f, 100f) < conspiracyChance)
                {
                    TriggerConspiracyCrisis(heir);
                    break;
                }
            }
        }

        private void TriggerConspiracyCrisis(Heir heir)
        {
            if (FactionManager.Instance != null)
            {
                foreach (var faction in FactionManager.Instance.factions)
                {
                    faction.loyalty = Mathf.Clamp(faction.loyalty - 3f, 0f, 100f);
                }
            }

            if (LegitimacySystem.Instance != null)
                LegitimacySystem.Instance.currentLegitimacy -= 4f;

            Debug.Log($"Dynasty intrigue: {heir.heirName} is suspected in a palace conspiracy.");
        }

        private void ApplyNewRulerPoliticalReset(Heir newRuler)
        {
            if (FactionManager.Instance == null || newRuler == null) return;

            foreach (var faction in FactionManager.Instance.factions)
            {
                float support = newRuler.factionSupport.ContainsKey(faction.type) ? newRuler.factionSupport[faction.type] : 50f;
                faction.loyalty = Mathf.Clamp(Mathf.Lerp(faction.loyalty, support, 0.4f), 0f, 100f);
            }
        }
    }
}
