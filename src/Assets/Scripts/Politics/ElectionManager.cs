using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Politics
{
    public class ElectionManager : MonoBehaviour
    {
        public static ElectionManager Instance { get; private set; }

        public float yearsBetweenElections = 4f;
        public float nextElectionTime;
        public List<Decree> currentPromises = new List<Decree>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            ScheduleNextElection();
        }

        public void ScheduleNextElection()
        {
            // Assuming 1 minute per month, 12 minutes per year
            nextElectionTime = Time.time + (yearsBetweenElections * 12 * 60f);
        }

        public void HoldElection(bool useFraud = false)
        {
            Debug.Log("Holding Election...");

            float winChance = CalculateWinChance();
            if (useFraud) winChance += 30f;

            bool won = UnityEngine.Random.Range(0, 100) < winChance;

            if (won)
            {
                Debug.Log("El Presidente won the election!");
                LegitimacySystem.Instance.ModifyLegitimacy(useFraud ? -20f : 10f);
            }
            else
            {
                Debug.Log("El Presidente lost the election. GAME OVER.");
                // Trigger GameOver
            }

            ScheduleNextElection();
        }

        private float CalculateWinChance()
        {
            // Average loyalty of all factions weighted by support base
            float totalLoyalty = 0;
            foreach (var f in FactionManager.Instance.factions)
            {
                totalLoyalty += f.loyalty;
            }
            return FactionManager.Instance.factions.Count > 0 ? totalLoyalty / FactionManager.Instance.factions.Count : 50f;
        }
    }
}
