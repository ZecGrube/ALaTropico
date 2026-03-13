using UnityEngine;
using CaudilloBay.Core;

namespace CaudilloBay.World
{
    public class Prison : Building
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            if (CrimeManager.Instance != null)
                CrimeManager.Instance.RegisterPrison(this);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (CrimeManager.Instance != null)
                CrimeManager.Instance.UnregisterPrison(this);
        }
    }
}
