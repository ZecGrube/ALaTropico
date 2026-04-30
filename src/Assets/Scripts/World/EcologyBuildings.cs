using UnityEngine;
using CaudilloBay.Ecology;

namespace CaudilloBay.World
{
    public class ForestNursery : Building
    {
        public float reforestationPower = 0.5f;

        private void OnEnable()
        {
            if (EcosystemManager.Instance != null)
                EcosystemManager.Instance.globalReforestationRate += reforestationPower;
        }

        private void OnDisable()
        {
            if (EcosystemManager.Instance != null)
                EcosystemManager.Instance.globalReforestationRate -= reforestationPower;
        }
    }

    public class WildlifeReserve : Building
    {
        public float biodiversityBonus = 1.0f;

        public override bool IsFunctional()
        {
            return base.IsFunctional() && workersRequired > 0;
        }

        // Logic handled via EcosystemManager check or specific effect
    }

    public class RangerStation : Building
    {
        public float antiPoachingPower = 10f;

        private void OnEnable()
        {
            if (EcosystemManager.Instance != null)
                EcosystemManager.Instance.antiPoachingStrength += antiPoachingPower;
        }

        private void OnDisable()
        {
            if (EcosystemManager.Instance != null)
                EcosystemManager.Instance.antiPoachingStrength -= antiPoachingPower;
        }
    }

    public class EcoCertificationOffice : Building
    {
        public float exportBonus = 0.05f;
        // Increases international prestige or resource value
    }
}
