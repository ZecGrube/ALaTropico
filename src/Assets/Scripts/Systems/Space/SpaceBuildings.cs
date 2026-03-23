using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.World;
using CaudilloBay.Economy;

namespace CaudilloBay.Systems.Space
{
    public class Spaceport : Building
    {
        public bool isMissionScheduled = false;

        private void Update()
        {
            if (!IsFunctional()) return;

            // Spaceport acts as a production hub for missions
            // In a more complex game, we'd assign specialized workers here
        }

        public void ScheduleMission(Data.SpaceMissionTemplate template)
        {
            if (isMissionScheduled) return;

            if (SpaceMissionManager.Instance != null)
            {
                SpaceMissionManager.Instance.ScheduleMission(template);
                isMissionScheduled = true;
            }
        }
    }

    public class RocketFactory : Building
    {
        [Header("Production Config")]
        public float titaniumRequired = 50f;
        public float compositesRequired = 20f;
        public float productionTime = 120f;
        public float currentRocketProgress = 0f;

        private void Update()
        {
            if (!IsFunctional()) return;

            // Simple rocket part production cycle
            // In a real game, this would produce RocketPart items for the Spaceport
        }
    }

    public class SpaceTourismCenter : Building
    {
        [Header("Tourism Config")]
        public float ticketPrice = 5000f;
        public int capacity = 10;
        public float prestigeBonusPerFlight = 2f;

        public void ConductTouristFlight()
        {
            if (!IsFunctional()) return;

            // Requires an available rocket (simplified)
            if (EconomyManager.Instance != null)
            {
                float totalRevenue = ticketPrice * capacity;
                EconomyManager.Instance.AddFunds(totalRevenue);
                if (Core.GlobalInfluenceManager.Instance != null)
                    Core.GlobalInfluenceManager.Instance.AddPrestige(prestigeBonusPerFlight);

                Debug.Log($"[SpaceTourismCenter] Conducted suborbital flight. Revenue: ${totalRevenue}");
            }
        }
    }
}
