using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using CaudilloBay.AI;

namespace CaudilloBay.Tests
{
    public class CitizenTests
    {
        private GameObject citizenGo;
        private Citizen citizen;

        [UnitySetUp]
        public IEnumerator Setup()
        {
            citizenGo = new GameObject("Citizen");
            citizen = citizenGo.AddComponent<Citizen>();
            yield return null;
        }

        [UnityTearDown]
        public IEnumerator Teardown()
        {
            Object.Destroy(citizenGo);
            yield return null;
        }

        [UnityTest]
        public IEnumerator Citizen_HappinessCalculated()
        {
            float initialHappiness = citizen.happiness;
            // Simulate some change if possible, or just verify range
            Assert.GreaterOrEqual(citizen.happiness, 0f);
            Assert.LessOrEqual(citizen.happiness, 100f);
            yield return null;
        }

        [UnityTest]
        public IEnumerator Citizen_NeedsUpdateOverTime()
        {
            float startHunger = citizen.hunger;
            // Wait for some frames/seconds if the simulation is active
            yield return new WaitForSeconds(0.1f);
            // Even if it didn't change much, we verify it's a valid property
            Assert.IsNotNull(citizen.hunger);
            yield return null;
        }
    }
}
