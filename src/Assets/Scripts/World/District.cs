using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.World
{
    [System.Serializable]
    public class District
    {
        public string districtName;
        public Color districtColor;
        public List<Building> buildings = new List<Building>();
        public List<Data.PolicyData> activePolicies = new List<Data.PolicyData>();

        public RectInt area;

        // Aggregated Stats
        public float avgSatisfaction = 50f;
        public float totalPollution = 0f;
        public float crimeRate = 0f;

        public District(string name, Color color, RectInt area)
        {
            this.districtName = name;
            this.districtColor = color;
            this.area = area;
        }

        public void AddBuilding(Building b)
        {
            if (!buildings.Contains(b))
            {
                buildings.Add(b);
                // b.district = this; // Update Building link
            }
        }

        public void RecalculateStats()
        {
            if (buildings.Count == 0) return;

            float satisfactionSum = 0f;
            totalPollution = 0f;
            foreach (var b in buildings)
            {
                totalPollution += b.pollutionOutput;
                if (b is ResidentialBuilding rb) satisfactionSum += rb.currentHappiness;
            }
            avgSatisfaction = satisfactionSum / (buildings.Count > 0 ? buildings.Count : 1);
        }
    }
}
