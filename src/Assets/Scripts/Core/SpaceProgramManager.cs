using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Core
{
    public enum SatelliteType { Communication, Reconnaissance, Science }

    public class SpaceProgramManager : MonoBehaviour
    {
        public static SpaceProgramManager Instance { get; private set; }

        public bool hasSpaceport = false;
        public List<SatelliteType> launchedSatellites = new List<SatelliteType>();

        public float rocketProductionProgress = 0f;
        public bool isProducingRocket = false;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void StartRocketProduction()
        {
            if (!hasSpaceport || isProducingRocket) return;

            float cost = 5000f;
            if (Economy.EconomyManager.Instance != null && Economy.EconomyManager.Instance.treasuryBalance >= cost)
            {
                Economy.EconomyManager.Instance.AddFunds(-cost);
                isProducingRocket = true;
                rocketProductionProgress = 0f;
                Debug.Log("Rocket production started for the glory of El Presidente!");
            }
        }

        public void LaunchSatellite(SatelliteType type)
        {
            if (!isProducingRocket || rocketProductionProgress < 100f)
            {
                Debug.LogWarning("Rocket not ready for launch.");
                return;
            }

            launchedSatellites.Add(type);
            isProducingRocket = false;
            rocketProductionProgress = 0f;
            Debug.Log($"SUCCESS! {type} satellite is in orbit.");

            ApplySatelliteBonuses(type);
        }

        private void ApplySatelliteBonuses(SatelliteType type)
        {
            switch (type)
            {
                case SatelliteType.Communication:
                    if (Economy.LogisticsManager.Instance != null)
                        Debug.Log("Global communication bonus: Logistics efficiency +15%");
                    break;
                case SatelliteType.Science:
                    if (World.TechnologyManager.Instance != null)
                        Debug.Log("Orbital research bonus: Research speed +20%");
                    break;
                case SatelliteType.Reconnaissance:
                    if (Politics.GlobalMapManager.Instance != null)
                        Debug.Log("Spy satellite active: Enemy movements revealed.");
                    break;
            }
        }

        public void UpdateMonthly()
        {
            if (isProducingRocket)
            {
                rocketProductionProgress += 10f; // 10 months to build
            }
        }
    }
}
