using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Politics
{
    public class CorruptionManager : MonoBehaviour
    {
        public static CorruptionManager Instance { get; private set; }

        [Range(0f, 100f)] public float corruptionLevel = 5f;
        public float blackMarketMoney = 0f;
        public float monthlyCorruptionDelta = 0f;

        private readonly Dictionary<string, float> corruptionSources = new Dictionary<string, float>();
        private readonly Dictionary<string, float> antiCorruptionSources = new Dictionary<string, float>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void RegisterCorruptionSource(string key, float power)
        {
            corruptionSources[key] = Mathf.Max(0f, power);
        }

        public void UnregisterCorruptionSource(string key)
        {
            if (corruptionSources.ContainsKey(key)) corruptionSources.Remove(key);
        }

        public void RegisterAntiCorruptionSource(string key, float power)
        {
            antiCorruptionSources[key] = Mathf.Max(0f, power);
        }

        public void UnregisterAntiCorruptionSource(string key)
        {
            if (antiCorruptionSources.ContainsKey(key)) antiCorruptionSources.Remove(key);
        }

        public void ProcessMonthlyCorruption()
        {
            float totalCorruption = 0f;
            float totalAntiCorruption = 0f;

            foreach (var source in corruptionSources) totalCorruption += source.Value;
            foreach (var source in antiCorruptionSources) totalAntiCorruption += source.Value;

            monthlyCorruptionDelta = (totalCorruption * 0.35f) - (totalAntiCorruption * 0.4f);
            corruptionLevel = Mathf.Clamp(corruptionLevel + monthlyCorruptionDelta, 0f, 100f);
        }

        public float ApplyTaxLeakage(float grossTaxRevenue)
        {
            float leakageRate = (corruptionLevel / 100f) * 0.4f;
            float leaked = grossTaxRevenue * leakageRate;
            AddBlackMarketMoney(leaked);
            return Mathf.Max(0f, grossTaxRevenue - leaked);
        }

        public float GetConstructionCostMultiplier()
        {
            return 1f + (corruptionLevel / 100f) * 0.25f;
        }

        public void AddBlackMarketMoney(float amount)
        {
            blackMarketMoney += Mathf.Max(0f, amount);
        }

        public bool SpendBlackMarketMoney(float amount)
        {
            amount = Mathf.Max(0f, amount);
            if (blackMarketMoney < amount) return false;
            blackMarketMoney -= amount;
            return true;
        }

        public float LaunderMoney(float amount, float feeRate)
        {
            amount = Mathf.Clamp(amount, 0f, blackMarketMoney);
            if (amount <= 0f) return 0f;

            blackMarketMoney -= amount;
            float legalMoney = amount * Mathf.Clamp01(1f - feeRate);
            return legalMoney;
        }
    }
}
