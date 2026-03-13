using UnityEngine;

namespace CaudilloBay.World
{
    public class University : Building
    {
        [Header("Research Generation")]
        public float researchPointsPerMonth = 10f;

        protected override void OnEnable()
        {
            base.OnEnable();
            if (Core.EducationManager.Instance != null)
                Core.EducationManager.Instance.RegisterBuilding(this);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (Core.EducationManager.Instance != null)
                Core.EducationManager.Instance.UnregisterBuilding(this);
        }

        private void Update()
        {
            if (IsConstructed)
            {
                // In a real tick system, this would be monthly
                // For now, distribute per second
                float pointsToAdd = (researchPointsPerMonth / 60f) * Time.deltaTime;
                TechnologyManager.Instance.AddResearchPoints(pointsToAdd);
            }
        }
    }
}
