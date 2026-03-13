using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.AI;

namespace CaudilloBay.Core
{
    public class EducationManager : MonoBehaviour
    {
        public static EducationManager Instance { get; private set; }

        [Header("Global Education State")]
        public float globalEducationLevel = 0f;
        public float monthlyEducationGrowth = 0f;

        private List<World.Building> educationalBuildings = new List<World.Building>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void ProcessMonthlyEducation()
        {
            CalculateEducationGrowth();
            UpdateCitizenEducation();
            CalculateAverageEducation();
        }

        private void CalculateEducationGrowth()
        {
            float totalBonus = 0f;
            foreach (var b in educationalBuildings)
            {
                if (b.IsFunctional())
                {
                    if (b is World.School) totalBonus += 5f;
                    else if (b is World.University) totalBonus += 10f;
                    // Vocational schools affect productivity directly, but can also add to global education
                }
            }

            int popSize = PopulationManager.Instance != null ? PopulationManager.Instance.allCitizens.Count : 1;
            monthlyEducationGrowth = totalBonus / Mathf.Max(popSize, 1);
        }

        private void UpdateCitizenEducation()
        {
            if (PopulationManager.Instance == null) return;

            foreach (var citizen in PopulationManager.Instance.allCitizens)
            {
                citizen.educationLevel = Mathf.Clamp(citizen.educationLevel + monthlyEducationGrowth, 0, 100);
            }
        }

        private void CalculateAverageEducation()
        {
            if (PopulationManager.Instance == null || PopulationManager.Instance.allCitizens.Count == 0) return;

            float sum = 0f;
            foreach (var citizen in PopulationManager.Instance.allCitizens)
            {
                sum += citizen.educationLevel;
            }
            globalEducationLevel = sum / PopulationManager.Instance.allCitizens.Count;
        }

        public void RegisterBuilding(World.Building building)
        {
            if (!educationalBuildings.Contains(building)) educationalBuildings.Add(building);
        }

        public void UnregisterBuilding(World.Building building)
        {
            educationalBuildings.Remove(building);
        }
    }
}
