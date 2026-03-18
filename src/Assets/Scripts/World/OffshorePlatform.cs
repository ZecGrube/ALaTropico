using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.World
{
    public abstract class OffshorePlatform : Building
    {
        public OffshoreDeposit targetDeposit;
        public float extractionRate = 50f;
        public float riskFactor = 0.01f; // 1% chance per month

        public void PerformExtraction()
        {
            if (targetDeposit != null && targetDeposit.quantity > 0)
            {
                float amount = targetDeposit.Extract(extractionRate);
                // Resources stored in platform inventory, requires logistics to shore
                // inventory.AddResource(targetDeposit.resourceId, amount);
                Debug.Log($"{displayName} extracted {amount} units.");
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            if (OffshoreManager.Instance != null)
                OffshoreManager.Instance.activePlatforms.Add(this);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (OffshoreManager.Instance != null)
                OffshoreManager.Instance.activePlatforms.Remove(this);
        }
    }

    public class DrillingPlatform : OffshorePlatform { }
    public class OffshoreMine : OffshorePlatform { }
}
