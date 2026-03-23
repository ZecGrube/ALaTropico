using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Systems.Space
{
    public class SpaceRaceManager : MonoBehaviour
    {
        public static SpaceRaceManager Instance { get; private set; }

        [Header("Global Competitors")]
        public float usaProgress = 0f;
        public float ussrProgress = 0f;

        [Header("Historical Milestones")]
        public bool satellite_USSR = false;
        public bool human_USSR = false;
        public bool moon_USA = false;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void ProcessMonthlyUpdate()
        {
            // Simple competition logic
            usaProgress += Random.Range(0.5f, 2.0f);
            ussrProgress += Random.Range(0.5f, 2.0f);

            CheckGlobalMilestones();
        }

        private void CheckGlobalMilestones()
        {
            if (ussrProgress >= 100f && !satellite_USSR)
            {
                satellite_USSR = true;
                if (SpaceMissionManager.Instance != null && !SpaceMissionManager.Instance.firstSatellite)
                {
                    Debug.Log("[SpaceRaceManager] USSR launches Sputnik 1! First satellite in orbit.");
                }
            }

            if (ussrProgress >= 200f && !human_USSR)
            {
                human_USSR = true;
                if (SpaceMissionManager.Instance != null && !SpaceMissionManager.Instance.firstHumanInOrbit)
                {
                    Debug.Log("[SpaceRaceManager] Yuri Gagarin (USSR) is the first human in space!");
                }
            }

            if (usaProgress >= 500f && !moon_USA)
            {
                moon_USA = true;
                if (SpaceMissionManager.Instance != null && !SpaceMissionManager.Instance.firstMoonLanding)
                {
                    Debug.Log("[SpaceRaceManager] Neil Armstrong (USA) is the first human on the Moon!");
                }
            }
        }
    }
}
