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

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void UpdateTourism(float totalAttractiveness, float safetyFactor)
        {
            // Simplified flow calculation
            float targetTourists = totalAttractiveness * safetyFactor * 10f;
            currentTourists = Mathf.RoundToInt(Mathf.Lerp(currentTourists, targetTourists, 0.1f));

            float income = currentTourists * monthlyIncomePerTourist;
            Debug.Log($"Tourism Update: {currentTourists} tourists generated ${income}");

            // Faction influence
            if (FactionManager.Instance != null)
            {
                // Capitalists like tourism
                ModifyFactionLoyalty(FactionType.Capitalists, currentTourists * 0.01f);
                // Religious/Environmentalists might dislike excessive tourism (pollution/morality)
                ModifyFactionLoyalty(FactionType.Environmentalists, -currentTourists * 0.005f);
            }
        }

        private void ModifyFactionLoyalty(FactionType type, float delta)
        {
            var faction = FactionManager.Instance.factions.Find(f => f.type == type);
            if (faction != null) faction.loyalty = Mathf.Clamp(faction.loyalty + delta, 0, 100);
        }
    }
}
