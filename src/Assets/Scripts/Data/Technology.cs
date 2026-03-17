using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.Politics;

namespace CaudilloBay.Data
{
    [CreateAssetMenu(fileName = "New Technology", menuName = "Caudillo Bay/Technology")]
    public class Technology : ScriptableObject
    {
        public string techId;
        public string techName;
        public int requiredEraIndex = 0;
        [TextArea]
        public string description;

        public int researchCost;
        public float researchTime; // e.g., in months

        public List<Technology> prerequisites = new List<Technology>();
        public List<GameObject> unlockedBuildings = new List<GameObject>();
        public List<ResourceType> unlockedResources = new List<ResourceType>();

        public List<FactionEffect> loyaltyEffects = new List<FactionEffect>();
        public Sprite icon;
    }
}
