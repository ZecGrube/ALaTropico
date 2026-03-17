using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.AI
{
    public class MigrationManager : MonoBehaviour
    {
        public static MigrationManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void TriggerRefugeeCrisis(int count)
        {
            Debug.LogWarning($"REFUGEE CRISIS: {count} refugees have arrived at the border.");
            // UI prompt to Accept or Reject
        }

        public void AcceptRefugees(int count)
        {
            for (int i = 0; i < count; i++)
            {
                PopulationManager.Instance.CreateCitizen("Refugee", Random.Range(18, 50));
            }

            // Political fallout
            if (Politics.FactionManager.Instance != null)
            {
                ModifyFactionLoyalty(Politics.FactionType.Communists, 10f); // Compassion
                ModifyFactionLoyalty(Politics.FactionType.Nationalists, -20f); // Xenophobia
            }
        }

        private void ModifyFactionLoyalty(Politics.FactionType type, float delta)
        {
            var faction = Politics.FactionManager.Instance.factions.Find(f => f.type == type);
            if (faction != null) faction.loyalty = Mathf.Clamp(faction.loyalty + delta, 0, 100);
        }
    }
}
