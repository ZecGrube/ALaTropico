using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.World;
using CaudilloBay.Politics;

namespace CaudilloBay.Core
{
    public class CorruptionManager : MonoBehaviour
    {
        public static CorruptionManager Instance { get; private set; }

        [Header("Global Corruption State")]
        public float globalCorruptionRate = 0f;
        public float blackMarketMoney = 1000f;

        private List<Building> shadowBuildings = new List<Building>();
        private int policeHQCount = 0;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void ProcessMonthlyCorruption()
        {
            CalculateCorruptionRate();
            GeneratePassiveShadowIncome();
            CheckForScandals();
        }

        private void CalculateCorruptionRate()
        {
            float baseCorruption = 0f;
            foreach (var b in shadowBuildings)
            {
                if (b.IsFunctional()) baseCorruption += 5f;
            }

            // Anti-corruption
            float reduction = policeHQCount * 20f;

            globalCorruptionRate = Mathf.Clamp(baseCorruption - reduction, 0, 100);
            Debug.Log($"Monthly Corruption: {globalCorruptionRate}%. Black Market: ${blackMarketMoney}");
        }

        private void GeneratePassiveShadowIncome()
        {
            foreach (var b in shadowBuildings)
            {
                if (b.IsFunctional() && b is World.SmugglingDen)
                {
                    AddBlackMarketMoney(100f);
                }
            }
        }

        private void CheckForScandals()
        {
            if (globalCorruptionRate > 70f)
            {
                if (Random.Range(0f, 100f) < 10f)
                {
                    TriggerScandal();
                }
            }
        }

        private void TriggerScandal()
        {
            Debug.Log("CORRUPTION SCANDAL EXPOSED!");
            if (LegitimacySystem.Instance != null)
                LegitimacySystem.Instance.currentLegitimacy -= 15f;
        }

        public void AddBlackMarketMoney(float amount) { blackMarketMoney += amount; }
        public bool SpendBlackMarketMoney(float amount)
        {
            if (blackMarketMoney >= amount)
            {
                blackMarketMoney -= amount;
                return true;
            }
            return false;
        }

        public void RegisterShadowBuilding(Building b) { if (!shadowBuildings.Contains(b)) shadowBuildings.Add(b); }
        public void UnregisterShadowBuilding(Building b) { shadowBuildings.Remove(b); }

        public void RegisterPoliceHQ() { policeHQCount++; }
        public void UnregisterPoliceHQ() { policeHQCount--; }

        public void LaunderMoney(float amount)
        {
            if (SpendBlackMarketMoney(amount))
            {
                float commission = 0.3f; // 30% fee
                float received = amount * (1.0f - commission);
                if (Economy.EconomyManager.Instance != null)
                    Economy.EconomyManager.Instance.treasuryBalance += received;

                Debug.Log($"Laundered ${amount} shadow money into ${received} official treasury.");
            }
        }
    }
}
