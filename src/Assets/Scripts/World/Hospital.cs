using UnityEngine;
using CaudilloBay.Core;

namespace CaudilloBay.World
{
    public class Hospital : Building
    {
        public Data.ResourceType medicineResource;
        public float medicineConsumptionPerMonth = 5f;

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

        public bool HasMedicine()
        {
            return inventory.HasResource(medicineResource, 1.0f);
        }
    }
}
