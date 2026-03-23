using UnityEngine;
using CaudilloBay.World;
using CaudilloBay.Economy;

namespace CaudilloBay.Systems.Nuclear
{
    public class EnrichmentPlant : Building
    {
        [Header("Production Config")]
        public float uraniumOreRequired = 100f;
        public float productionTime = 60f; // Seconds per batch
        public float currentBatchProgress = 0f;

        private void Update()
        {
            if (!IsFunctional()) return;

            // Simple production loop
            if (inventory.GetAmount("UraniumOre") >= uraniumOreRequired)
            {
                currentBatchProgress += Time.deltaTime;
                if (currentBatchProgress >= productionTime)
                {
                    currentBatchProgress = 0f;
                    inventory.RemoveAmount("UraniumOre", uraniumOreRequired);
                    inventory.AddAmount("EnrichedUranium", 10f);

                    if (NuclearManager.Instance != null)
                        NuclearManager.Instance.OnBuildingEnrichmentCompleted();
                }
            }
        }
    }

    public class NuclearReactor : Building
    {
        [Header("Reactor Config")]
        public float powerOutput = 1000f;
        public float fuelConsumptionRate = 1f; // Enriched Uranium per minute
        public bool isProducingPlutonium = false;

        private void Update()
        {
            if (!IsFunctional()) return;

            // Fuel consumption
            float fuelNeeded = (fuelConsumptionRate * Time.deltaTime) / 60f;
            if (inventory.GetAmount("EnrichedUranium") >= fuelNeeded)
            {
                inventory.RemoveAmount("EnrichedUranium", fuelNeeded);

                // Add power to the grid (assuming PowerGridManager integration)
                if (powerNode != null)
                {
                    powerNode.providedAmount = powerOutput;
                }

                if (isProducingPlutonium)
                {
                    inventory.AddAmount("Plutonium", fuelNeeded * 0.1f); // 10% yield
                }
            }
            else
            {
                if (powerNode != null) powerNode.providedAmount = 0;
            }
        }
    }

    public class MissileSilo : Building
    {
        public List<DeliverySystem> storedSystems = new List<DeliverySystem>();
        public int capacity = 3;

        protected override void CompleteConstruction()
        {
            base.CompleteConstruction();

            // Add initial ICBM slots
            for (int i = 0; i < capacity; i++)
            {
                storedSystems.Add(new ICBM { systemName = $"Silo-{buildingId}-LaunchTube-{i}" });
            }

            if (NuclearManager.Instance != null)
            {
                foreach(var sys in storedSystems)
                    NuclearManager.Instance.systems.Add(sys);
            }
        }

        public void Launch(int slotIndex, Vector2 targetLocation)
        {
            if (!IsFunctional() || slotIndex >= storedSystems.Count) return;

            var sys = storedSystems[slotIndex];
            if (sys.IsLoaded)
            {
                // Trigger launch logic in NuclearManager
                // NuclearManager.Instance.LaunchStrike(targetCountry, targetLocation);
                sys.loadedWarhead = null; // Warhead spent
                Debug.Log($"[MissileSilo] Launched from slot {slotIndex}");
            }
        }
    }

    public class NuclearTestSite : Building
    {
        public bool canConductTest = true;

        public void ExecuteTest(string type)
        {
            if (!IsFunctional() || !canConductTest) return;

            if (NuclearManager.Instance != null)
            {
                NuclearManager.Instance.ConductTest(type);
                // Tests have a cooldown
                StartCoroutine(TestCooldown());
            }
        }

        private System.Collections.IEnumerator TestCooldown()
        {
            canConductTest = false;
            yield return new WaitForSeconds(300f); // 5 minute cooldown
            canConductTest = true;
        }
    }
}
