using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.World
{
    public class StreetSweeperStation : Building
    {
        public float cleaningRadius = 30f;
        public float cleanRate = 0.5f;

        private void Update()
        {
            if (IsFunctional())
            {
                CleanNearbyBuildings();
            }
        }

        private void CleanNearbyBuildings()
        {
            foreach (var b in StatsManager.Instance.GetTrackedBuildings())
            {
                if (b != null && Vector3.Distance(transform.position, b.transform.position) < cleaningRadius)
                {
                    b.garbageAccumulated = Mathf.Max(0, b.garbageAccumulated - cleanRate * Time.deltaTime);
                }
            }
        }
    }
}
