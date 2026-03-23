using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.Core;
using CaudilloBay.Economy;

namespace CaudilloBay.Systems.Nuclear
{
    public class NuclearManager : MonoBehaviour
    {
        public static NuclearManager Instance { get; private set; }

        [Header("Nuclear Program State")]
        public bool hasNPT = true; // Non-Proliferation Treaty status
        public bool hasSTART = false; // Strategic Arms Reduction Treaty
        public bool hasCTBT = false; // Comprehensive Test Ban Treaty

        public List<NuclearWarhead> arsenal = new List<NuclearWarhead>();
        public List<DeliverySystem> systems = new List<DeliverySystem>();

        [Header("Global Nuclear Tension (0-100)")]
        [Range(0f, 100f)]
        public float nuclearTension = 10f;
        public float tensionDecayRate = 0.5f; // Per month decay

        [Header("Nuclear Deterrence")]
        public float currentDeterrence = 0f;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            // Update nuclear tension decay
            if (nuclearTension > 0)
            {
                // decay logic here - assume monthly for now
                nuclearTension -= (tensionDecayRate * Time.deltaTime) / 30f;
            }
        }

        public void CalculateDeterrence()
        {
            float totalPower = 0f;
            foreach (var system in systems)
            {
                if (system.IsLoaded)
                {
                    // Basic formula: deterrence = warhead yield * reliability * (1 - interception chance)
                    totalPower += system.loadedWarhead.yieldMT * system.loadedWarhead.reliability * (1f - system.interceptionChance);
                }
            }

            currentDeterrence = Mathf.Sqrt(totalPower) * 10f; // Soft cap with square root
            Debug.Log($"[NuclearManager] Current Deterrence: {currentDeterrence}");
        }

        public void ConductTest(string testType)
        {
            // Underground, Atmospheric, Underwater
            float tensionIncrease = 0f;
            float reliabilityGain = 0f;

            switch (testType.ToLower())
            {
                case "underground":
                    tensionIncrease = 5f;
                    reliabilityGain = 0.05f;
                    break;
                case "atmospheric":
                    tensionIncrease = 20f;
                    reliabilityGain = 0.15f;
                    break;
                case "underwater":
                    tensionIncrease = 12f;
                    reliabilityGain = 0.10f;
                    break;
            }

            if (hasCTBT)
            {
                tensionIncrease *= 3f; // Severe penalty for breaking treaty
                // Handle diplomatic fallout here via GlobalMapManager
            }

            nuclearTension += tensionIncrease;

            // Improve all existing warheads reliability
            foreach (var warhead in arsenal)
            {
                warhead.reliability = Mathf.Clamp01(warhead.reliability + reliabilityGain);
            }

            Debug.Log($"[NuclearManager] Conducted {testType} test. Tension increased to {nuclearTension}");
        }

        public void IncreaseTension(float amount)
        {
            nuclearTension = Mathf.Clamp(nuclearTension + amount, 0f, 100f);
        }

        public void ProposeTreaty(string treatyId)
        {
            // Integrate with GlobalMapManager
            Debug.Log($"[NuclearManager] Proposing treaty: {treatyId}");
        }

        public void OnBuildingEnrichmentCompleted()
        {
            // Triggered when uranium enrichment plant finishes a batch
            var newWarhead = new NuclearWarhead
            {
                warheadId = System.Guid.NewGuid().ToString(),
                warheadName = "Model Alpha",
                yieldMT = 1.0f,
                reliability = 0.8f
            };
            arsenal.Add(newWarhead);
            Debug.Log("[NuclearManager] New nuclear warhead produced.");
        }

        public void LaunchStrike(string targetName, Vector2 targetLocation, DeliverySystem system)
        {
            if (system == null || !system.IsLoaded) return;

            Debug.Log($"[NuclearManager] Launching nuclear strike on {targetName} at {targetLocation}");

            // Random chance for interception
            if (Random.value < system.interceptionChance)
            {
                Debug.Log($"[NuclearManager] Strike intercepted by {targetName} defense systems!");
                system.loadedWarhead = null;
                return;
            }

            // Reliability check
            if (Random.value > system.loadedWarhead.reliability)
            {
                Debug.Log($"[NuclearManager] Warhead malfunctioned and failed to detonate.");
                system.loadedWarhead = null;
                return;
            }

            // Detonation logic
            Detonate(targetLocation, system.loadedWarhead);
            system.loadedWarhead = null;

            // Global Fallout
            IncreaseTension(25f);
        }

        private void Detonate(Vector2 location, NuclearWarhead warhead)
        {
            float blastRadius = Mathf.Sqrt(warhead.yieldMT) * 100f; // Scale by yield
            Debug.Log($"[NuclearManager] DETONATION! Yield: {warhead.yieldMT}MT. Blast Radius: {blastRadius}");

            // Destroy buildings in blast radius
            var buildings = StatsManager.Instance.GetBuildingsInRange(location, blastRadius);
            foreach (var b in buildings)
            {
                b.TakeDamage(1000000f); // Guaranteed destruction
            }

            // Create Radiation Zone
            GameObject radObj = new GameObject("RadiationZone");
            radObj.transform.position = new Vector3(location.x, 0, location.y);
            var zone = radObj.AddComponent<RadiationZone>();
            zone.center = location;
            zone.radius = blastRadius * 1.5f;
            zone.intensity = 1.0f;
        }
    }
}
