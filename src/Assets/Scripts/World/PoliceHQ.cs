using UnityEngine;
using CaudilloBay.Core;

namespace CaudilloBay.World
{
    public class PoliceHQ : Building
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            if (CorruptionManager.Instance != null)
                CorruptionManager.Instance.RegisterPoliceHQ();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (CorruptionManager.Instance != null)
                CorruptionManager.Instance.UnregisterPoliceHQ();
        }
    }
}
