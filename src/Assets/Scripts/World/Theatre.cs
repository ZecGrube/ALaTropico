using UnityEngine;
using CaudilloBay.Core;

namespace CaudilloBay.World
{
    public class Theatre : Building
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            if (CultureManager.Instance != null)
                CultureManager.Instance.RegisterBuilding(this);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (CultureManager.Instance != null)
                CultureManager.Instance.UnregisterBuilding(this);
        }
    }
}
