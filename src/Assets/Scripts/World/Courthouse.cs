using UnityEngine;
using CaudilloBay.Core;

namespace CaudilloBay.World
{
    public class Courthouse : Building
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            if (CrimeManager.Instance != null)
                CrimeManager.Instance.RegisterCourthouse();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (CrimeManager.Instance != null)
                CrimeManager.Instance.UnregisterCourthouse();
        }
    }
}
