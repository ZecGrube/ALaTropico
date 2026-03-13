using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Politics
{
    public class CultureManager : MonoBehaviour
    {
        public static CultureManager Instance { get; private set; }

        [Range(0f, 100f)] public float cultureLevel = 10f;
        public float festivalBonus = 0f;
        public float monthlySubsidyCost = 0f;

        private readonly Dictionary<string, float> buildingContributions = new Dictionary<string, float>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void RegisterCultureBuilding(string key, float monthlyContribution)
        {
            buildingContributions[key] = monthlyContribution;
        }

        public void UnregisterCultureBuilding(string key)
        {
            if (buildingContributions.ContainsKey(key)) buildingContributions.Remove(key);
        }

        public void ProcessMonthlyCulture()
        {
            float monthlyGain = festivalBonus;
            foreach (var kv in buildingContributions) monthlyGain += kv.Value;

            cultureLevel = Mathf.Clamp(cultureLevel + monthlyGain, 0f, 100f);
            festivalBonus = Mathf.Max(0f, festivalBonus - 1f);
        }

        public float GetTourismMultiplier()
        {
            return 1f + (cultureLevel / 100f) * 0.5f;
        }

        public void ApplyFestivalDecree()
        {
            festivalBonus += 6f;
        }

        public void ApplyArtsSubsidy()
        {
            monthlySubsidyCost = 120f;
            cultureLevel = Mathf.Clamp(cultureLevel + 2f, 0f, 100f);
        }

        public void ApplyDestructionPenalty(float value)
        {
            cultureLevel = Mathf.Clamp(cultureLevel - Mathf.Max(0f, value), 0f, 100f);
        }
    }
}
