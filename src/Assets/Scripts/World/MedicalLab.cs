using UnityEngine;
using CaudilloBay.Core;

namespace CaudilloBay.World
{
    public class MedicalLab : Building
    {
        [Header("Epidemic Bonus")]
        public float researchBonus = 0.2f;

        protected override void OnEnable()
        {
            base.OnEnable();
            if (HealthManager.Instance != null)
                HealthManager.Instance.RegisterBuilding(this);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (HealthManager.Instance != null)
                HealthManager.Instance.UnregisterBuilding(this);
        }
    }
}
