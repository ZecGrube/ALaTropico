using UnityEngine;

namespace CaudilloBay.Data
{
    [System.Serializable]
    public class SandboxSettings
    {
        public enum IslandSize { Small, Medium, Large }
        public enum ResourceRichness { Sparse, Normal, Abundant }

        public IslandSize size = IslandSize.Medium;
        public ResourceRichness richness = ResourceRichness.Normal;
        public float startingMoney = 5000f;
        public int difficulty = 1; // 1: Easy, 2: Normal, 3: Hard

        public int GetWidth()
        {
            switch (size) {
                case IslandSize.Small: return 30;
                case IslandSize.Large: return 80;
                default: return 50;
            }
        }

        public int GetHeight()
        {
            return GetWidth();
        }
    }
}
