using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.World;

namespace CaudilloBay.Systems.Nuclear
{
    public class RadiationZone : MonoBehaviour
    {
        public Vector2 center;
        public float radius;
        public float intensity; // 0.0 to 1.0 (deadly)
        public float decayRate = 0.01f; // Per month

        private float timer = 0f;

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= 10f) // Check effects every 10 seconds
            {
                ApplyEffects();
                timer = 0f;
            }

            // Slowly decay radiation
            intensity -= (decayRate * Time.deltaTime) / 60f;
            if (intensity <= 0)
            {
                Destroy(gameObject);
            }
        }

        private void ApplyEffects()
        {
            // Find all buildings in range
            var buildings = StatsManager.Instance.GetBuildingsInRange(center, radius);
            foreach (var b in buildings)
            {
                // Radiation damages buildings slowly
                b.TakeDamage(intensity * 5f);

                // Affect citizens in the building (logic placeholder)
                // if (b is ResidentialBuilding res) res.KillCitizens(intensity);
            }

            // Soil quality degradation (if integrated with agriculture)
        }
    }
}
