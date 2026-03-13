using UnityEngine;
using CaudilloBay.Core;

namespace CaudilloBay.World
{
    public class VocationalSchool : Building
    {
        [Header("Industrial Bonus")]
        public float efficiencyBonus = 0.15f;

        protected override void OnEnable()
        {
            base.OnEnable();
            // Vocational schools primarily affect local productivity, but can contribute to global stats too
            if (EducationManager.Instance != null)
                EducationManager.Instance.RegisterBuilding(this);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (EducationManager.Instance != null)
                EducationManager.Instance.UnregisterBuilding(this);
        }
    }
}
