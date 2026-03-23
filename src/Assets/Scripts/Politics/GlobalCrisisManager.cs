using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.Data;
using CaudilloBay.Core;
using CaudilloBay.Economy;

namespace CaudilloBay.Politics
{
    public class GlobalCrisisManager : MonoBehaviour
    {
        public static GlobalCrisisManager Instance { get; private set; }

        [Header("Crisis Database")]
        public List<GlobalCrisisTemplate> allCrisisTemplates = new List<GlobalCrisisTemplate>();
        public List<GlobalCrisisInstance> activeCrises = new List<GlobalCrisisInstance>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void GenerateRandomCrisis()
        {
            if (allCrisisTemplates.Count == 0) return;

            var template = allCrisisTemplates[Random.Range(0, allCrisisTemplates.Count)];
            var instance = new GlobalCrisisInstance { template = template, remainingDuration = 12f };
            activeCrises.Add(instance);

            Debug.Log($"[GlobalCrisisManager] GLOBAL CRISIS TRIGGERED: {template.crisisName}");
            // Trigger UI event notification
        }

        public void AttemptSolution(GlobalCrisisInstance instance)
        {
            if (instance.isSolved) return;

            // Check requirements
            bool canSolve = true;
            if (EconomyManager.Instance != null && EconomyManager.Instance.treasuryBalance < instance.template.requiredFunds) canSolve = false;
            // Additional resource/tech checks here

            if (canSolve)
            {
                if (EconomyManager.Instance != null) EconomyManager.Instance.AddFunds(-instance.template.requiredFunds);

                SolveCrisis(instance);
            }
        }

        private void SolveCrisis(GlobalCrisisInstance instance)
        {
            instance.isSolved = true;
            if (GlobalInfluenceManager.Instance != null)
            {
                GlobalInfluenceManager.Instance.AddPrestige(instance.template.prestigeReward);
                GlobalInfluenceManager.Instance.globalInfluence += instance.template.influenceReward;
                GlobalInfluenceManager.Instance.SolveCrisis();
            }

            foreach (var mod in instance.template.resolutionModifiers)
            {
                if (ModifierManager.Instance != null)
                    ModifierManager.Instance.ApplyModifier(mod, 24); // 2-year bonus
            }

            Debug.Log($"[GlobalCrisisManager] Crisis solved: {instance.template.crisisName}");
        }

        public void ProcessMonthlyUpdate()
        {
            for (int i = activeCrises.Count - 1; i >= 0; i--)
            {
                var crisis = activeCrises[i];
                if (crisis.isSolved)
                {
                    activeCrises.RemoveAt(i);
                    continue;
                }

                crisis.remainingDuration -= 1f;
                if (crisis.remainingDuration <= 0)
                {
                    // Crisis failed - apply penalties
                    Debug.Log($"[GlobalCrisisManager] Crisis failed: {crisis.template.crisisName}");
                    if (GlobalInfluenceManager.Instance != null)
                        GlobalInfluenceManager.Instance.AddPrestige(-15f);
                    activeCrises.RemoveAt(i);
                }
            }
        }
    }
}
