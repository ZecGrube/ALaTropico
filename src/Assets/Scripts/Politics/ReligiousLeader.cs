using UnityEngine;

namespace CaudilloBay.Politics
{
    [System.Serializable]
    public class ReligiousLeader : Agent
    {
        [Range(0, 100)] public float loyaltyToRegime = 50f;
        [Range(0, 100)] public float faith = 80f;

        public void GenerateRandomStats()
        {
            agentName = "Bishop " + (Random.Range(0, 2) == 0 ? "Rodriguez" : "Sanchez");
            charisma = Random.Range(40, 95);
            stealth = Random.Range(10, 50);
            combat = Random.Range(5, 30);
            tech = Random.Range(10, 40);
            faith = Random.Range(70, 100);
        }
    }
}
