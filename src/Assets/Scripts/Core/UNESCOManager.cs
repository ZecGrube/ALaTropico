using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Core
{
    public class UNESCOManager : MonoBehaviour
    {
        public static UNESCOManager Instance { get; private set; }

        public float internationalPrestige = 50f;
        public List<World.Building> heritageSites = new List<World.Building>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void ApplyForHeritageStatus(World.Building building)
        {
            if (heritageSites.Contains(building)) return;

            // Chance based on building age or type
            bool approved = Random.Range(0, 100) < 30;

            if (approved)
            {
                heritageSites.Add(building);
                internationalPrestige += 10f;
                Debug.Log($"UNESCO SUCCESS: {building.data.buildingName} is now a World Heritage Site!");

                // Boost tourism nearby
                if (Economy.TouristManager.Instance != null)
                {
                    Economy.TouristManager.Instance.activeEventBonus += 20f;
                }
            }
            else
            {
                Debug.Log("UNESCO denied the application. Try again in a few years.");
            }
        }

        public void UpdateMonthly()
        {
            // Maintenance of sites costs money
            float cost = heritageSites.Count * 100f;
            if (Economy.EconomyManager.Instance != null)
            {
                Economy.EconomyManager.Instance.AddFunds(-cost);
            }
        }
    }
}
