using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.World
{
    public class DistrictManager : MonoBehaviour
    {
        public static DistrictManager Instance { get; private set; }

        public List<District> activeDistricts = new List<District>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public District CreateDistrict(string name, Color color, RectInt area)
        {
            District newDistrict = new District(name, color, area);
            activeDistricts.Add(newDistrict);

            // Assign buildings in area to district
            UpdateDistrictBuildings(newDistrict);

            return newDistrict;
        }

        public void UpdateDistrictBuildings(District district)
        {
            district.buildings.Clear();
            if (StatsManager.Instance != null)
            {
                foreach (var b in StatsManager.Instance.GetTrackedBuildings())
                {
                    if (district.area.Contains(new Vector2Int(b.GridPosition.x, b.GridPosition.z)))
                    {
                        district.AddBuilding(b);
                        b.district = district;
                    }
                }
            }
        }

        public void ProcessMonthlyUpdate()
        {
            float totalPolicyCosts = 0f;
            foreach (var district in activeDistricts)
            {
                district.RecalculateStats();
                foreach (var policy in district.activePolicies)
                {
                    totalPolicyCosts += policy.maintenanceCost;
                    ApplyPolicyEffects(district, policy);
                }
            }

            if (Economy.EconomyManager.Instance != null)
            {
                Economy.EconomyManager.Instance.AddFunds(-totalPolicyCosts);
            }
        }

        private void ApplyPolicyEffects(District district, Data.PolicyData policy)
        {
            // Logic to apply modifiers to buildings in the district
            // This could interface with ModifierManager or directly affect building stats
        }
    }
}
