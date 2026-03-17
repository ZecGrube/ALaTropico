using UnityEngine;

namespace CaudilloBay.World
{
    public class SuperpowerMilitaryBase : Building
    {
        public Politics.SuperpowerType hostSuperpower;
        public float monthlyRent = 2000f;
        public float protectionLevel = 50f;

        public void CollectRent()
        {
            if (Economy.EconomyManager.Instance != null)
            {
                Economy.EconomyManager.Instance.AddFunds(monthlyRent);
            }
        }

        public void EvictBase()
        {
            Debug.LogWarning($"El Presidente is evicting the {hostSuperpower} base! Prepare for diplomatic fallout.");
            // Drastic reduction in relations
            Destroy(gameObject);
        }
    }
}
