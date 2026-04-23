using NUnit.Framework;
using CaudilloBay.Economy;
using CaudilloBay.Data;
using CaudilloBay.World;
using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Tests
{
    public class LogisticsTests
    {
        private LogisticsManager logistics;
        private GameObject go;

        [SetUp]
        public void Setup()
        {
            go = new GameObject("LogisticsManager");
            logistics = go.AddComponent<LogisticsManager>();
            logistics.pendingOrders = new List<TransportOrder>();
        }

        [TearDown]
        public void Teardown()
        {
            Object.DestroyImmediate(go);
        }

        [Test]
        public void LogisticsManager_CanCreateOrder()
        {
            GameObject b1 = new GameObject("B1");
            GameObject b2 = new GameObject("B2");
            var res = TestHelper.CreateMockResource("wood", "Wood");

            logistics.CreateOrder(b1.AddComponent<Building>(), b2.AddComponent<Building>(), res, 5f);

            Assert.AreEqual(1, logistics.pendingOrders.Count);
            Assert.AreEqual(5f, logistics.pendingOrders[0].amount);

            Object.DestroyImmediate(b1);
            Object.DestroyImmediate(b2);
        }

        [Test]
        public void LogisticsManager_OrderQueueing()
        {
            logistics.CreateOrder(null, null, null, 10f);
            logistics.CreateOrder(null, null, null, 20f);

            Assert.AreEqual(2, logistics.pendingOrders.Count);
            Assert.AreEqual(10f, logistics.pendingOrders[0].amount);
        }
    }
}
