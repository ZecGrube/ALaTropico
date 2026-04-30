using UnityEngine;
using CaudilloBay.Data;
using CaudilloBay.Economy;
using CaudilloBay.World;

namespace CaudilloBay.Tests
{
    public static class TestHelper
    {
        public static ResourceType CreateMockResource(string id, string name, float weight = 1f)
        {
            var resource = ScriptableObject.CreateInstance<ResourceType>();
            resource.resourceId = id;
            resource.resourceName = name;
            resource.unitWeight = weight;
            return resource;
        }

        public static BuildingData CreateMockBuildingData(string id, string name)
        {
            var data = ScriptableObject.CreateInstance<BuildingData>();
            data.buildingId = id;
            data.displayName = name;
            return data;
        }

        public static GameObject CreateMockBuilding(string id, string name)
        {
            var go = new GameObject(name);
            var b = go.AddComponent<Building>();
            b.data = CreateMockBuildingData(id, name);
            return go;
        }
    }
}
