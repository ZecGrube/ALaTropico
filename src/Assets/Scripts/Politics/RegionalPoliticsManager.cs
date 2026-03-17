using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.Data;

namespace CaudilloBay.Politics
{
    public class RegionalPoliticsManager : MonoBehaviour
    {
        public static RegionalPoliticsManager Instance { get; private set; }

        public List<NeighborState> neighbors = new List<NeighborState>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void ProposeTradeAgreement(NeighborState neighbor)
        {
            if (neighbor.relations > 20)
            {
                Debug.Log($"Trade agreement established with {neighbor.stateName}");
                neighbor.relations += 10;
                // Add actual trade logic integration here later
            }
            else
            {
                Debug.Log($"{neighbor.stateName} rejected the trade agreement.");
                neighbor.relations -= 5;
            }
        }

        public void ProposeNonAggressionPact(NeighborState neighbor)
        {
            if (neighbor.relations > 40)
            {
                neighbor.hasNonAggressionPact = true;
                neighbor.relations += 15;
                Debug.Log($"Non-aggression pact signed with {neighbor.stateName}");
            }
            else
            {
                Debug.Log($"{neighbor.stateName} is not interested in a pact right now.");
            }
        }

        public void ProposeAlliance(NeighborState neighbor)
        {
            if (neighbor.relations > 70 && neighbor.hasNonAggressionPact)
            {
                neighbor.isAllied = true;
                neighbor.relations += 25;
                Debug.Log($"Alliance formed with {neighbor.stateName}!");
            }
            else
            {
                Debug.Log($"{neighbor.stateName} declines the alliance.");
            }
        }

        public void ProvokeNeighbor(NeighborState neighbor)
        {
            neighbor.relations -= 30;
            Debug.Log($"You provoked {neighbor.stateName}. Relations dropped significantly.");

            if (neighbor.relations < -50)
            {
                TriggerBorderConflict(neighbor);
            }
        }

        private void TriggerBorderConflict(NeighborState neighbor)
        {
            Debug.LogWarning($"Border conflict triggered with {neighbor.stateName}!");
            if (MilitaryManager.Instance != null)
            {
                // Logic for minor military skirmish
            }
        }

        public void UpdateMonthly()
        {
            foreach (var neighbor in neighbors)
            {
                // Passive relations drift towards 0
                neighbor.relations = Mathf.MoveTowards(neighbor.relations, 0, 0.5f);

                // Random events could be added here
            }
        }
    }
}
