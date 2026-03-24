using UnityEngine;
using CaudilloBay.AI;

namespace CaudilloBay.Systems.Automation
{
    public class SyntheticCitizen : Citizen
    {
        [Header("Synthetic Specs")]
        public float integrity = 100f;
        public float powerLevel = 100f;

        public void Start()
        {
            // Synthetic citizens have fixed traits
            firstName = "Unit";
            lastName = Random.Range(1000, 9999).ToString();

            traits = new PersonalityTraits {
                aggressiveness = 0,
                ambition = 0,
                charisma = 0,
                loyalty = 100,
                greed = 0,
                faith = 0,
                curiosity = 50
            };

            // They don't have human needs
            hungerRate = 0;
            health = 100;
        }

        public void Maintenance(float amount)
        {
            integrity = Mathf.Clamp(integrity + amount, 0, 100);
        }

        public override void CalculateHappiness()
        {
            // Synthetics are always perfectly "satisfied" as long as powered
            satisfaction = powerLevel;
        }
    }
}
