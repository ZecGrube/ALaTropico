using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.Politics;

namespace CaudilloBay.Colonization
{
    public class ColonyManager : MonoBehaviour
    {
        public static ColonyManager Instance { get; private set; }

        public List<Colony> colonies = new List<Colony>();
        public GameObject colonyHubPrefab;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public Colony CreateColony(string name, string islandId)
        {
            Colony newColony = new Colony {
                islandId = islandId,
                name = name,
                isClaimed = true,
                ownerName = "Player"
            };
            colonies.Add(newColony);
            return newColony;
        }

        public void ProcessMonthlyTick()
        {
            foreach (var colony in colonies)
            {
                UpdateColony(colony);
            }
        }

        private void UpdateColony(Colony colony)
        {
            // Autonomous growth based on policy
            switch (colony.autoPolicy)
            {
                case ColonyPolicy.RapidGrowth:
                    colony.population += Mathf.RoundToInt(colony.population * 0.05f) + 1;
                    colony.loyalty -= 1f;
                    break;
                case ColonyPolicy.ResourceExtraction:
                    colony.loyalty -= 2f;
                    break;
                default:
                    colony.population += Mathf.RoundToInt(colony.population * 0.01f);
                    break;
            }

            // Check for independence movements
            if (colony.loyalty < 30f)
            {
                TriggerUnrest(colony);
            }
        }

        private void TriggerUnrest(Colony colony)
        {
            Debug.LogWarning($"Colony {colony.name} is in unrest!");
            // Integration with EventManager would go here
        }
    }
}
