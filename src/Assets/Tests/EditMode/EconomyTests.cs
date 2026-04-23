using NUnit.Framework;
using CaudilloBay.Economy;
using CaudilloBay.Data;
using UnityEngine;

namespace CaudilloBay.Tests
{
    public class EconomyTests
    {
        private Inventory inventory;
        private ResourceType wood;

        [SetUp]
        public void Setup()
        {
            inventory = new Inventory();
            wood = TestHelper.CreateMockResource("wood", "Wood", 2f);
        }

        [Test]
        public void AddResource_IncreasesAmount()
        {
            inventory.AddResource(wood, 10);
            Assert.AreEqual(10, inventory.GetAmount(wood));
        }

        [Test]
        public void RemoveResource_DecreasesAmount()
        {
            inventory.AddResource(wood, 10);
            bool success = inventory.RemoveResource(wood, 4);
            Assert.IsTrue(success);
            Assert.AreEqual(6, inventory.GetAmount(wood));
        }

        [Test]
        public void GetTotalWeight_CalculatesCorrectly()
        {
            inventory.AddResource(wood, 5); // 5 * 2 = 10
            Assert.AreEqual(10f, inventory.GetTotalWeight());
        }
    }
}
