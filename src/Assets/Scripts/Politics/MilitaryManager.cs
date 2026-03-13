using UnityEngine;

namespace CaudilloBay.Politics
{
    public class MilitaryManager : MonoBehaviour
    {
        public static MilitaryManager Instance { get; private set; }

        [Header("Army")]
        public float armyStrength = 0f;
        [Range(0f, 100f)] public float trainingLevel = 10f;
        [Range(0f, 100f)] public float armyLoyalty = 50f;

        [Header("Defense Infrastructure")]
        public float coastalDefenseLevel = 0f;
        public float airPowerLevel = 0f;
        public float foreignSupportStrength = 0f;

        [Header("Economy")]
        public float upkeepPerStrengthPoint = 0.4f;
        public float upkeepPerTrainingPoint = 2f;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void AddArmyStrength(float amount) => armyStrength += Mathf.Max(0f, amount);
        public void RemoveArmyStrength(float amount) => armyStrength = Mathf.Max(0f, armyStrength - Mathf.Max(0f, amount));

        public void ImproveTraining(float amount) => trainingLevel = Mathf.Clamp(trainingLevel + amount, 0f, 100f);
        public void ReduceTraining(float amount) => trainingLevel = Mathf.Clamp(trainingLevel - amount, 0f, 100f);

        public void AddCoastalDefense(float amount) => coastalDefenseLevel += Mathf.Max(0f, amount);
        public void RemoveCoastalDefense(float amount) => coastalDefenseLevel = Mathf.Max(0f, coastalDefenseLevel - Mathf.Max(0f, amount));

        public void AddAirPower(float amount) => airPowerLevel += Mathf.Max(0f, amount);
        public void RemoveAirPower(float amount) => airPowerLevel = Mathf.Max(0f, airPowerLevel - Mathf.Max(0f, amount));

        public void AddForeignSupport(float amount) => foreignSupportStrength += Mathf.Max(0f, amount);
        public void ClearForeignSupport() => foreignSupportStrength = 0f;

        public float CalculateDefensePower()
        {
            float trainingMultiplier = 0.5f + trainingLevel / 100f;
            float loyaltyMultiplier = 0.7f + armyLoyalty / 200f;
            float infraBonus = coastalDefenseLevel + (airPowerLevel * 0.5f);
            return (armyStrength * trainingMultiplier * loyaltyMultiplier) + infraBonus + foreignSupportStrength;
        }

        public float CalculateMonthlyUpkeep()
        {
            return (armyStrength * upkeepPerStrengthPoint) + (trainingLevel * upkeepPerTrainingPoint);
        }

        public void UpdateArmyLoyalty(float delta)
        {
            armyLoyalty = Mathf.Clamp(armyLoyalty + delta, 0, 100);
        }

        public void ApplyMonthlyBudgetPressure(float treasuryBalance)
        {
            float upkeep = CalculateMonthlyUpkeep();
            if (treasuryBalance < upkeep)
            {
                UpdateArmyLoyalty(-2f);
                ReduceTraining(1f);
            }
        }
    }
}
