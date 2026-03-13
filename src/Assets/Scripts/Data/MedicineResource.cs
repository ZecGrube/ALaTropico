using UnityEngine;
using CaudilloBay.Data;

namespace CaudilloBay.Data
{
    public static class MedicineResource
    {
        public static ResourceType CreateMedicine(Sprite icon)
        {
            ResourceType medicine = ScriptableObject.CreateInstance<ResourceType>();
            medicine.resourceId = "medicine";
            medicine.resourceName = "Medicine";
            medicine.icon = icon;
            medicine.unitWeight = 0.1f;
            medicine.baseValue = 100f;
            return medicine;
        }
    }
}
