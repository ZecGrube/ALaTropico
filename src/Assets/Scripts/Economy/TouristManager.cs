using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.World;
using CaudilloBay.Politics;

namespace CaudilloBay.Economy
{
    public class TouristManager : MonoBehaviour
    {
        public static TouristManager Instance { get; private set; }

        public float touristFlowRate = 1.0f;
        public int currentTourists = 0;
        public float monthlyIncomePerTourist = 50f;

        [Header("Advanced Tourism")]
        public List<TouristType> activeTouristTypes = new List<TouristType>();
        public float seasonalityFactor = 1.0f; // 0.5 (off-season) to 1.5 (peak)
        public float activeEventBonus = 0f;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void UpdateTourism(float totalAttractiveness, float safetyFactor, int currentMonth)
        {
            UpdateSeasonality(currentMonth);

            // Culture bonus to attractiveness
            if (Core.CultureManager.Instance != null)
            {
                totalAttractiveness += Core.CultureManager.Instance.globalCultureLevel;
            }

            // Religion and Cult bonus
            if (Core.ReligionManager.Instance != null)
            {
                totalAttractiveness += Core.ReligionManager.Instance.religiousInfluence * 0.5f;
            }
            if (Core.PersonalityCultManager.Instance != null)
            {
                totalAttractiveness += Core.PersonalityCultManager.Instance.cultLevel * 0.3f;
            }

            // Waste/Pollution impact
            if (WasteManager.Instance != null)
            {
                totalAttractiveness -= WasteManager.Instance.GetGlobalGarbageLevel() * 0.05f;
            }

            // Override safetyFactor with crime rate if CrimeManager is present
            if (Core.CrimeManager.Instance != null)
            {
                safetyFactor = Mathf.Clamp01(1.0f - (Core.CrimeManager.Instance.globalCrimeRate / 100f));
            }

            // Advanced flow calculation
            float effectiveAttractiveness = (totalAttractiveness + activeEventBonus) * seasonalityFactor;
            float targetTourists = effectiveAttractiveness * safetyFactor * 10f;
            currentTourists = Mathf.RoundToInt(Mathf.Lerp(currentTourists, targetTourists, 0.1f));

            float totalIncome = 0f;
            foreach (var type in activeTouristTypes)
            {
                // Each type has its own multiplier or logic
                float typeShare = GetTypeShare(type);
                float count = currentTourists * typeShare;
                totalIncome += count * type.budget * 0.1f; // 10% of budget spent monthly
            }

            if (activeTouristTypes.Count == 0) totalIncome = currentTourists * monthlyIncomePerTourist;

            Debug.Log($"Tourism Update: {currentTourists} tourists. Season: {seasonalityFactor}. Income: ${totalIncome}");

            // Faction influence
            if (FactionManager.Instance != null)
            {
                ModifyFactionLoyalty(FactionType.Capitalists, currentTourists * 0.01f);
                ModifyFactionLoyalty(FactionType.Environmentalists, -currentTourists * 0.005f);
            }
        }

        private void UpdateSeasonality(int month)
        {
            // Dec, Jan, Feb = High Season in Caribbean
            if (month == 11 || month == 0 || month == 1) seasonalityFactor = 1.5f;
            // June, July, Aug = Hurricane Season / Hot = Lower
            else if (month >= 5 && month <= 7) seasonalityFactor = 0.7f;
            else seasonalityFactor = 1.0f;
        }

        private float GetTypeShare(TouristType type)
        {
            // Logic to distribute tourists among types based on island facilities
            // For now, simple uniform distribution
            return 1.0f / (activeTouristTypes.Count > 0 ? activeTouristTypes.Count : 1);
        }

        public void TriggerEvent(string name, float bonus, float duration)
        {
            activeEventBonus += bonus;
            Debug.Log($"Tourism Event: {name} started! Attractiveness +{bonus}");
            // Start coroutine to end event or handle via tick
        }

        private void ModifyFactionLoyalty(FactionType type, float delta)
        {
            var faction = FactionManager.Instance.factions.Find(f => f.type == type);
            if (faction != null) faction.loyalty = Mathf.Clamp(faction.loyalty + delta, 0, 100);
        }
    }
}
