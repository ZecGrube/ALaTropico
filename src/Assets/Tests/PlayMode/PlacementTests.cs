using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using CaudilloBay.Construction;
using CaudilloBay.Data;
using CaudilloBay.World;

namespace CaudilloBay.Tests
{
    public class PlacementTests
    {
        private GameObject placementGo;
        private PlacementSystem placementSystem;
        private BuildingData testBuilding;

        [UnitySetUp]
        public IEnumerator Setup()
        {
            placementGo = new GameObject("PlacementSystem");
            placementSystem = placementGo.AddComponent<PlacementSystem>();

            testBuilding = ScriptableObject.CreateInstance<BuildingData>();
            testBuilding.buildingId = "test_house";
            testBuilding.footprint = new Vector2Int(1, 1);

            // Mock main camera for PlacementSystem
            GameObject camGo = new GameObject("MainCamera");
            camGo.tag = "MainCamera";
            var cam = camGo.AddComponent<Camera>();
            placementSystem.mainCamera = cam;

            yield return null;
        }

        [UnityTearDown]
        public IEnumerator Teardown()
        {
            Object.Destroy(placementGo);
            GameObject camGo = GameObject.Find("MainCamera");
            if (camGo != null) Object.Destroy(camGo);
            yield return null;
        }

        [UnityTest]
        public IEnumerator PlacementSystem_InitializesCorrectly()
        {
            Assert.IsNotNull(placementSystem.mainCamera);
            Assert.AreEqual(placementSystem.mainCamera.tag, "MainCamera");
            yield return null;
        }

        [UnityTest]
        public IEnumerator PlacementSystem_SelectionWorks()
        {
            placementSystem.selectedBuildingData = testBuilding;
            Assert.AreEqual(testBuilding, placementSystem.selectedBuildingData);
            yield return null;
        }
    }
}
