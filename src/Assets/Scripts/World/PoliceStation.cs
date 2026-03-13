using UnityEngine;
using CaudilloBay.Core;

namespace CaudilloBay.World
{
    public class PoliceStation : Building
    {
        [Header("Police Security")]
        public float coverageRadius = 15f;
        public float effectiveness = 15f;

        protected override void OnEnable()
        {
            base.OnEnable();
            if (CrimeManager.Instance != null)
                CrimeManager.Instance.RegisterPoliceStation(this);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (CrimeManager.Instance != null)
                CrimeManager.Instance.UnregisterPoliceStation(this);
        }
    }
}
