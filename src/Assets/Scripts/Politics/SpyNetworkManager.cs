using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Politics
{
    public enum SpyMissionType { Sabotage, CyberAttack, Recruitment, Assassination, Disinformation }

    [System.Serializable]
    public class SpyNetwork
    {
        public string countryName;
        public float networkStrength = 0f; // Increases over time
        public List<Agent> activeAgents = new List<Agent>();

        public void UpdateMonthly()
        {
            if (activeAgents.Count > 0)
            {
                networkStrength = Mathf.Min(networkStrength + activeAgents.Count * 2f, 100f);
            }
        }
    }

    public class SpyNetworkManager : MonoBehaviour
    {
        public static SpyNetworkManager Instance { get; private set; }

        public List<SpyNetwork> networks = new List<SpyNetwork>();
        public float counterIntelligenceBonus = 0f;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void StartMission(SpyNetwork network, SpyMissionType type)
        {
            Debug.Log($"Starting {type} mission in {network.countryName}...");

            float successChance = network.networkStrength * 0.5f + 20f;
            if (Random.Range(0, 100) < successChance)
            {
                ResolveMissionSuccess(network, type);
            }
            else
            {
                Debug.LogWarning($"Mission {type} FAILED in {network.countryName}!");
            }
        }

        private void ResolveMissionSuccess(SpyNetwork network, SpyMissionType type)
        {
            Debug.Log($"Mission {type} SUCCESSFUL in {network.countryName}!");
            // Apply effects based on type (Sabotage production, etc)
        }

        public void UpdateMonthly()
        {
            foreach (var network in networks) network.UpdateMonthly();
        }
    }
}
