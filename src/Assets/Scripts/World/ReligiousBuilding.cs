using UnityEngine;
using CaudilloBay.Core;

namespace CaudilloBay.World
{
    public abstract class ReligiousBuilding : Building
    {
        public float influencePower = 5f;

        protected override void OnEnable()
        {
            base.OnEnable();
            if (ReligionManager.Instance != null)
                ReligionManager.Instance.RegisterTemple(this);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (ReligionManager.Instance != null)
                ReligionManager.Instance.UnregisterTemple(this);
        }
    }
}
