using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.AI;
using CaudilloBay.World;

namespace CaudilloBay.Core
{
    public class CultureManager : MonoBehaviour
    {
        public static CultureManager Instance { get; private set; }

        public float globalCultureLevel = 0f;
        private List<Building> culturalBuildings = new List<Building>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void ProcessMonthlyCulture()
        {
            float totalCulture = 0f;
            foreach (var b in culturalBuildings)
            {
                if (b.IsFunctional())
                {
                    totalCulture += b.data.touristAttraction; // Use attraction as a proxy for culture output
                }
            }

            int popSize = PopulationManager.Instance != null ? PopulationManager.Instance.allCitizens.Count : 1;
            globalCultureLevel = Mathf.Clamp(totalCulture / Mathf.Max(popSize / 10f, 1), 0, 100);
        }

        public void RegisterBuilding(Building b) { if (!culturalBuildings.Contains(b)) culturalBuildings.Add(b); }
        public void UnregisterBuilding(Building b) { culturalBuildings.Remove(b); }
    }
}
