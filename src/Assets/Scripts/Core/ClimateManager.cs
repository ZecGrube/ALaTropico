using UnityEngine;
using CaudilloBay.Economy;

namespace CaudilloBay.Core
{
    public class ClimateManager : MonoBehaviour
    {
        public static ClimateManager Instance { get; private set; }

        public float globalCO2Level = 400f; // ppm
        public float seaLevelRise = 0f; // in units
        public float temperatureIncrease = 0f;

        [Header("Disasters")]
        public float hurricaneChance = 0.05f;
        public float droughtChance = 0.05f;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void ProcessMonthlyClimate(float islandPollution)
        {
            // Island pollution contributes to global CO2 (simplified)
            globalCO2Level += islandPollution * 0.001f;

            // Sea level rises based on CO2
            if (globalCO2Level > 450)
            {
                float riseDelta = (globalCO2Level - 450) * 0.0001f;
                seaLevelRise += riseDelta;
                temperatureIncrease += riseDelta * 10f;
            }

            CheckForDisasters();
        }

        private void CheckForDisasters()
        {
            float roll = Random.Range(0f, 1f);

            // Chances increase with temperature
            float tempModifier = 1.0f + (temperatureIncrease * 0.5f);

            if (roll < hurricaneChance * tempModifier)
            {
                TriggerHurricane();
            }
            else if (roll < (hurricaneChance + droughtChance) * tempModifier)
            {
                TriggerDrought();
            }
        }

        private void TriggerHurricane()
        {
            Debug.LogWarning("HURRICANE STRIKE! High damage to coastal buildings.");
            // Logic to damage coastal buildings (World.Building.TakeDamage)
        }

        private void TriggerDrought()
        {
            Debug.LogWarning("DROUGHT! Agriculture efficiency reduced.");
            // Logic to reduce farming output for 1-3 months
        }
    }
}
