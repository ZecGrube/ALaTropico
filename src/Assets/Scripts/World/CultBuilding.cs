using UnityEngine;
using CaudilloBay.Core;

namespace CaudilloBay.World
{
    public abstract class CultBuilding : Building
    {
        public float cultBonus = 2f;

        protected override void OnEnable()
        {
            base.OnEnable();
            if (PersonalityCultManager.Instance != null)
                PersonalityCultManager.Instance.RegisterCultAsset(this);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (PersonalityCultManager.Instance != null)
                PersonalityCultManager.Instance.UnregisterCultAsset(this);
        }
    }
}
