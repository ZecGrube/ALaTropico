using UnityEngine;
using CaudilloBay.Core;

namespace CaudilloBay.World
{
    public class SmugglingDen : Building
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            if (CorruptionManager.Instance != null)
                CorruptionManager.Instance.RegisterShadowBuilding(this);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (CorruptionManager.Instance != null)
                CorruptionManager.Instance.UnregisterShadowBuilding(this);
        }
    }
}
