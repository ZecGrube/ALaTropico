using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Economy
{
    public class LogisticsHub : MonoBehaviour
    {
        public float hubRadius = 20f;
        public float efficiencyBonus = 0.2f;

        public List<World.Building> connectedBuildings = new List<World.Building>();

        private void Start()
        {
            UpdateHubConnections();
        }

        public void UpdateHubConnections()
        {
            connectedBuildings.Clear();
            if (World.StatsManager.Instance != null)
            {
                foreach (var b in World.StatsManager.Instance.GetTrackedBuildings())
                {
                    if (Vector3.Distance(transform.position, b.transform.position) <= hubRadius)
                    {
                        connectedBuildings.Add(b);
                        // Apply bonus to buildings in range
                        Debug.Log($"Hub {name} linked to {b.data.buildingName}");
                    }
                }
            }
        }
    }
}
