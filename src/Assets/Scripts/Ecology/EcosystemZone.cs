using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Ecology
{
    public enum EcosystemType { Forest, Wetland, Grassland, Mountain, Coastal }

    [System.Serializable]
    public class EcosystemZone
    {
        public string zoneId;
        public EcosystemType type;
        public Vector2 center;
        public float radius;

        [Range(0, 100)] public float biodiversity = 80f;
        [Range(0, 100)] public float health = 100f;
        [Range(0, 100)] public float treeCover = 0f;

        public Dictionary<string, int> animalPopulations = new Dictionary<string, int>();

        public void ProcessMonthlyTick(float pollutionPenalty, float reforestationRate, float antiPoachingBonus)
        {
            // Health decay from pollution
            health = Mathf.Clamp(health - (pollutionPenalty * 0.1f) + reforestationRate, 0, 100);

            // Biodiversity depends on health
            biodiversity = Mathf.Lerp(biodiversity, health, 0.05f);

            // Tree cover growth
            if (type == EcosystemType.Forest)
            {
                treeCover = Mathf.Clamp(treeCover + (health * 0.01f * reforestationRate), 0, 100);
            }
        }
    }
}
