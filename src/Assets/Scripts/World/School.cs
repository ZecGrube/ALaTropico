using UnityEngine;
using CaudilloBay.Core;

namespace CaudilloBay.World
{
    public class School : Building
    {
        protected override void OnEnable()
        {
            base.OnEnable();
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
