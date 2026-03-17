using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.World
{
    public class LandmarkManager : MonoBehaviour
    {
        public static LandmarkManager Instance { get; private set; }

        public List<Landmark> constructedLandmarks = new List<Landmark>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public bool CanConstructLandmark(string landmarkId)
        {
            foreach (var l in constructedLandmarks)
            {
                if (l.landmarkUniqueId == landmarkId) return false;
            }
            return true;
        }

        public void RegisterLandmark(Landmark landmark)
        {
            if (!constructedLandmarks.Contains(landmark))
            {
                constructedLandmarks.Add(landmark);
            }
        }

        public void ApplyGlobalEffects()
        {
            foreach (var landmark in constructedLandmarks)
            {
                if (landmark.IsFunctional())
                {
                    // Update global modifiers here if they're not purely event-based
                }
            }
        }
    }
}
