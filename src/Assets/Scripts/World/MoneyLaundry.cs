using UnityEngine;
using CaudilloBay.Core;

namespace CaudilloBay.World
{
    public class MoneyLaundry : Building
    {
        public float monthlyLaunderAmount = 500f;

        private void Update()
        {
            if (IsFunctional() && CorruptionManager.Instance != null)
            {
                // Launder over time
                float amountThisFrame = (monthlyLaunderAmount / 60f) * Time.deltaTime;
                CorruptionManager.Instance.LaunderMoney(amountThisFrame);
            }
        }
    }
}
