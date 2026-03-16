using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.Politics;
using CaudilloBay.World;

namespace CaudilloBay.Core
{
    public class ReligionManager : MonoBehaviour
    {
        public static ReligionManager Instance { get; private set; }

        [Header("State")]
        public float religiousInfluence = 0f;
        public ReligiousLeader currentLeader;

        private List<ReligiousBuilding> activeTemples = new List<ReligiousBuilding>();

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

        public void RegisterTemple(ReligiousBuilding temple)
        {
            if (!activeTemples.Contains(temple))
            {
                activeTemples.Add(temple);
                UpdateInfluence();
            }
        }

        public void UnregisterTemple(ReligiousBuilding temple)
        {
            if (activeTemples.Contains(temple))
            {
                activeTemples.Remove(temple);
                UpdateInfluence();
            }
        }

        public void UpdateInfluence()
        {
            float total = 0f;
            foreach (var temple in activeTemples)
            {
                if (temple.IsFunctional())
                {
                    total += temple.influencePower;
                }
            }
            religiousInfluence = total;

            if (religiousInfluence > 50f && currentLeader == null)
            {
                SpawnLeader();
            }
        }

        public void ProcessMonthlyUpdate()
        {
            UpdateInfluence();

            if (FactionManager.Instance != null)
            {
                var religiousFaction = FactionManager.Instance.factions.Find(f => f.type == FactionType.Religious);
                if (religiousFaction != null)
                {
                    // Loyalty depends on influence and leader's loyalty
                    float loyaltyBonus = (religiousInfluence / 10f);
                    if (currentLeader != null)
                    {
                        loyaltyBonus += (currentLeader.loyaltyToRegime - 50f) / 5f;
                    }

                    religiousFaction.loyalty = Mathf.Clamp(religiousFaction.loyalty + loyaltyBonus, 0, 100);
                }
            }

            // High religion reduces crime
            if (CrimeManager.Instance != null)
            {
                CrimeManager.Instance.globalCrimeRate = Mathf.Max(0, CrimeManager.Instance.globalCrimeRate - (religiousInfluence * 0.1f));
            }
        }

        private void SpawnLeader()
        {
            currentLeader = new ReligiousLeader { agentName = "Bishop Rodriguez", isUnique = true };
            currentLeader.GenerateRandomStats();
            currentLeader.loyaltyToRegime = 50f;
            Debug.Log("Religious Leader Spawned: " + currentLeader.agentName);
        }
    }
}
