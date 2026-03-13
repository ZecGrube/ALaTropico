using UnityEngine;

namespace CaudilloBay.World
{
    public abstract class ShadowEconomyBuilding : Building
    {
        [Header("Shadow Economy")]
        public float corruptionImpact = 5f;
        [Range(0f, 1f)] public float detectionRisk = 0.15f;
        public float monthlyIllegalIncome = 120f;

        protected override void CompleteConstruction()
        {
            base.CompleteConstruction();
            if (CaudilloBay.Politics.CorruptionManager.Instance != null)
            {
                CaudilloBay.Politics.CorruptionManager.Instance.RegisterCorruptionSource(GetCorruptionKey(), corruptionImpact);
            }
        }

        protected virtual void OnDestroy()
        {
            if (CaudilloBay.Politics.CorruptionManager.Instance != null)
            {
                CaudilloBay.Politics.CorruptionManager.Instance.UnregisterCorruptionSource(GetCorruptionKey());
            }
        }

        protected string GetCorruptionKey()
        {
            return $"{GetType().Name}_{GetInstanceID()}";
        }

        public virtual void ProcessMonthlyShadowIncome()
        {
            if (!IsFunctional()) return;

            if (CaudilloBay.Politics.CorruptionManager.Instance != null)
            {
                CaudilloBay.Politics.CorruptionManager.Instance.AddBlackMarketMoney(monthlyIllegalIncome);
            }
        }
    }
}
