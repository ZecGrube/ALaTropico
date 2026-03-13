using UnityEngine;

namespace CaudilloBay.World
{
    public class Barracks : Building
    {
        public float strengthContribution = 50f;

        protected override void CompleteConstruction()
        {
            base.CompleteConstruction();
            if (CaudilloBay.Politics.MilitaryManager.Instance != null)
            {
                CaudilloBay.Politics.MilitaryManager.Instance.AddBarracksStrength(strengthContribution);
            }
        }

        private void OnDestroy()
        {
            if (CaudilloBay.Politics.MilitaryManager.Instance != null)
            {
                CaudilloBay.Politics.MilitaryManager.Instance.RemoveBarracksStrength(strengthContribution);
            }
        }
    }
}
