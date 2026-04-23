using NUnit.Framework;
using CaudilloBay.Politics;
using CaudilloBay.Data;
using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Tests
{
    public class PoliticsTests
    {
        private FactionManager factionManager;
        private GameObject go;

        [SetUp]
        public void Setup()
        {
            go = new GameObject("FactionManager");
            factionManager = go.AddComponent<FactionManager>();
            factionManager.factions = new List<FactionData>();

            var faction = ScriptableObject.CreateInstance<FactionData>();
            faction.displayName = "Militarists";
            faction.loyalty = 50f;
            factionManager.factions.Add(faction);
        }

        [TearDown]
        public void Teardown()
        {
            Object.DestroyImmediate(go);
        }

        [Test]
        public void FactionLoyalty_ChangesCorrectly()
        {
            var faction = factionManager.factions[0];
            faction.loyalty += 10;
            Assert.AreEqual(60f, faction.loyalty);
        }

        [Test]
        public void FactionManager_FindsFactionByName()
        {
            var found = factionManager.factions.Find(f => f.displayName == "Militarists");
            Assert.IsNotNull(found);
        }
    }
}
