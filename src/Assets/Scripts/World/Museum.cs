using UnityEngine;

namespace CaudilloBay.World
{
    public class Museum : CulturalBuilding
    {
        public float monthlyArtifactsProduction = 2f;
        public const string ArtifactsResourceId = "artifacts";

        public void ProcessMonthlyArtifacts()
        {
            if (!IsFunctional()) return;

            if (StatsManager.Instance != null)
            {
                StatsManager.Instance.AddResource(ArtifactsResourceId, monthlyArtifactsProduction);
            }
        }
    }
}
