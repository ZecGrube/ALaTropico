using UnityEngine;

namespace CaudilloBay.World
{
    public class MoneyLaundry : ShadowEconomyBuilding
    {
        [Range(0f, 0.95f)] public float launderingFeeRate = 0.25f;
        public float monthlyLaunderLimit = 250f;

        public override void ProcessMonthlyShadowIncome()
        {
            base.ProcessMonthlyShadowIncome();

            var corruption = CaudilloBay.Politics.CorruptionManager.Instance;
            var economy = CaudilloBay.Economy.EconomyManager.Instance;
            if (corruption == null || economy == null) return;

            float legalMoney = corruption.LaunderMoney(monthlyLaunderLimit, launderingFeeRate);
            economy.treasuryBalance += legalMoney;
        }
    }
}
