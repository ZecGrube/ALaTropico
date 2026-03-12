using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.World;

namespace CaudilloBay.Politics
{
    public class FactionManager : MonoBehaviour
    {
        public static FactionManager Instance { get; private set; }

        [Header("Peasants Faction")]
        public FactionData peasantFaction = new FactionData { type = FactionType.Peasants, displayName = "Peasants" };

        [Header("Politics State")]
        public int currentMandate = 20;
        public float monthlyMandateBase = 5f;

        private float monthlyTimer = 0f;
        public float monthLength = 60f; // 1 minute = 1 month

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Update()
        {
            monthlyTimer += Time.deltaTime;
            if (monthlyTimer >= monthLength)
            {
                MonthlyTick();
                monthlyTimer = 0f;
            }
        }

        private void MonthlyTick()
        {
            UpdatePeasantLoyalty();
            GenerateMandate();
            CheckRandomEvents();
            Debug.Log($"Monthly Political Update: Loyalty {peasantFaction.loyalty}, Mandate {currentMandate}");
        }

        private void UpdatePeasantLoyalty()
        {
            // Gather happiness from all residential buildings
            ResidentialBuilding[] homes = UnityEngine.Object.FindObjectsByType<ResidentialBuilding>(FindObjectsSortMode.None);
            if (homes.Length == 0) return;

            float totalHappiness = 0;
            foreach (var home in homes)
            {
                totalHappiness += home.GetHappiness();
            }

            float avgHappiness = totalHappiness / homes.Length;
            // Shift loyalty towards average happiness (simplified)
            peasantFaction.loyalty = Mathf.Lerp(peasantFaction.loyalty, avgHappiness, 0.2f);
        }

        private void GenerateMandate()
        {
            float bonus = (peasantFaction.loyalty - 50) / 10f;
            currentMandate += Mathf.RoundToInt(monthlyMandateBase + bonus);
        }

        private void CheckRandomEvents()
        {
            // Placeholder for event system logic
        }

        public void ApplyDecree(Decree decree)
        {
            if (currentMandate >= decree.mandateCost)
            {
                currentMandate -= decree.mandateCost;
                peasantFaction.loyalty += decree.peasantLoyaltyEffect;
                Debug.Log($"Issued Decree: {decree.decreeName}");
            }
        }
    }
}
