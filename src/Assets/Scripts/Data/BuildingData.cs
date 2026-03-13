using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.Politics;

namespace CaudilloBay.Data
{
    [CreateAssetMenu(fileName = "New Building Data", menuName = "Caudillo Bay/Building Data")]
    public class BuildingData : ScriptableObject
    {
        [Header("Identity")]
        public string buildingId;
        public string buildingName;
        [TextArea]
        public string description;
        public BuildingCategory category;
        public Sprite icon;
        public GameObject prefab;

        [Header("Physical")]
        public Vector2Int footprint = new Vector2Int(1, 1);

        [Header("Construction")]
        public List<ResourceCost> buildCosts;
        public float buildTime;

        [Header("Economic")]
        public int workersRequired;
        public List<ResourceEffect> production;
        public List<ResourceEffect> consumption;
        public float maintenanceCost;
        public int storageCapacity;

        [Header("Political/Environmental")]
        public float pollutionOutput;
        public List<BuildingFactionEffect> loyaltyEffects;
        public int touristAttraction;

        [Header("Requirements")]
        public Technology requiredTech;

        [Header("Flags")]
        public bool isDecoration;
    }

    [System.Serializable]
    public struct ResourceEffect
    {
        public ResourceType resourceType;
        public float amount;
    }

    [System.Serializable]
    public struct BuildingFactionEffect
    {
        public FactionType faction;
        public float effect;
    }
}
