using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.Politics;

namespace CaudilloBay.Core
{
    public class SportsManager : MonoBehaviour
    {
        public static SportsManager Instance { get; private set; }

        public float nationalPride = 50f;
        public int sportsFacilitiesCount = 0;

        [Header("Teams")]
        public float footballTeamLevel = 10f;
        public float baseballTeamLevel = 10f;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void InvestInTeam(string sport, float amount)
        {
            if (Economy.EconomyManager.Instance != null && Economy.EconomyManager.Instance.treasuryBalance >= amount)
            {
                Economy.EconomyManager.Instance.AddFunds(-amount);
                if (sport == "Football") footballTeamLevel += amount * 0.01f;
                else if (sport == "Baseball") baseballTeamLevel += amount * 0.01f;
                Debug.Log($"Invested ${amount} in {sport} team.");
            }
        }

        public void RunRegionalChampionship()
        {
            Debug.Log("Running Regional Sports Championship...");

            // Random result based on team levels
            bool win = Random.Range(0, 100) < (footballTeamLevel + baseballTeamLevel) / 2f;

            if (win)
            {
                nationalPride = Mathf.Clamp(nationalPride + 20, 0, 100);
                Debug.Log("We won! National pride increased!");

                // Loyalty bonus
                if (FactionManager.Instance != null)
                {
                    foreach(var f in FactionManager.Instance.factions)
                    {
                        if (f.type == FactionType.Nationalists) f.loyalty += 10;
                    }
                }
            }
            else
            {
                nationalPride = Mathf.Clamp(nationalPride - 5, 0, 100);
                Debug.Log("We lost. The fans are disappointed.");
            }
        }

        public void UpdateMonthly()
        {
            // Maintenance and slight pride decay
            nationalPride = Mathf.MoveTowards(nationalPride, 50, 0.1f);
        }
    }
}
