using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using CaudilloBay.World;
using CaudilloBay.Data;

namespace CaudilloBay.Tests
{
    public class ProductionTests
    {
        [UnityTest]
        public IEnumerator ProducerBuilding_Initializes()
        {
            GameObject go = new GameObject("Producer");
            var producer = go.AddComponent<ProducerBuilding>();
            Assert.IsNotNull(producer);
            Object.Destroy(go);
            yield return null;
        }

        [UnityTest]
        public IEnumerator ProducerBuilding_EfficiencyRange()
        {
            GameObject go = new GameObject("Producer");
            var producer = go.AddComponent<ProducerBuilding>();
            producer.baseEfficiency = 0.5f;
            Assert.AreEqual(0.5f, producer.baseEfficiency);
            Object.Destroy(go);
            yield return null;
        }
    }
}
