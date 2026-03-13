using UnityEngine;
using CaudilloBay.Core;
using CaudilloBay.World;

namespace CaudilloBay.Tests
{
    public class FunctionalCheckTest : MonoBehaviour
    {
        void Start()
        {
            RunTests();
        }

        public void RunTests()
        {
            Debug.Log("Starting FunctionalCheck Tests...");

            GameObject go = new GameObject("TestProducer");
            ProducerBuilding producer = go.AddComponent<ProducerBuilding>();

            // Test 1: Destroyed Building should not produce
            producer.currentHealth = 0f;
            if (!producer.CanProduce) Debug.Log("Test 1 Passed: Destroyed building cannot produce.");
            else Debug.LogError("Test 1 Failed: Destroyed building attempted production!");

            Debug.Log("FunctionalCheck Tests Complete.");
            Destroy(go);
        }
    }
}
