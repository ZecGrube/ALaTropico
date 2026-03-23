using UnityEngine;

namespace CaudilloBay.World
{
    public class CounterIntelligenceHQ : Building
    {
        public float defenseBonus = 25f;

        protected override void Start()
        {
            base.Start();
            if (Politics.SpyNetworkManager.Instance != null)
            {
                Politics.SpyNetworkManager.Instance.counterIntelligenceBonus += defenseBonus;
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (Politics.SpyNetworkManager.Instance != null)
            {
                Politics.SpyNetworkManager.Instance.counterIntelligenceBonus -= defenseBonus;
            }
        }
    }
}
