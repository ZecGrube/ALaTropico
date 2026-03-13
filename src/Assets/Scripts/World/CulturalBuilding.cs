using UnityEngine;

namespace CaudilloBay.World
{
    public class CulturalBuilding : Building
    {
        public float cultureContribution = 1f;
        public float touristAttractiveness = 5f;

        protected override void CompleteConstruction()
        {
            base.CompleteConstruction();
            if (CaudilloBay.Politics.CultureManager.Instance != null)
            {
                CaudilloBay.Politics.CultureManager.Instance.RegisterCultureBuilding(GetInstanceID().ToString(), cultureContribution);
            }
        }

        private void OnDestroy()
        {
            if (CaudilloBay.Politics.CultureManager.Instance != null)
            {
                CaudilloBay.Politics.CultureManager.Instance.UnregisterCultureBuilding(GetInstanceID().ToString());
            }
        }
    }
}
