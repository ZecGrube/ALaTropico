using UnityEngine;

namespace CaudilloBay.Core
{
    public class ExplorationManager : MonoBehaviour
    {
        public static ExplorationManager Instance { get; private set; }

        public float explorationProgress = 0f;
        public bool isExploring = false;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void StartGeologicalSurvey(float cost)
        {
            if (isExploring) return;

            if (Economy.EconomyManager.Instance != null && Economy.EconomyManager.Instance.treasuryBalance >= cost)
            {
                Economy.EconomyManager.Instance.AddFunds(-cost);
                isExploring = true;
                explorationProgress = 0f;
                Debug.Log("Geological survey started.");
            }
        }

        public void UpdateMonthly()
        {
            if (isExploring)
            {
                explorationProgress += 20f; // 5 months to complete
                if (explorationProgress >= 100f)
                {
                    CompleteSurvey();
                }
            }
        }

        private void CompleteSurvey()
        {
            isExploring = false;
            Debug.Log("Geological survey complete! New resource deposits found.");

            // Randomly spawn a new ResourceNode on an empty tile
            if (World.TileManager.Instance != null)
            {
                // Logic to find a random grass tile and place a resource node (Gold, Oil, etc)
            }
        }
    }
}
