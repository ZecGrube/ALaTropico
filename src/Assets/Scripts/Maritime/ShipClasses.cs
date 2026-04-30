using UnityEngine;
using CaudilloBay.Economy;

namespace CaudilloBay.Maritime
{
    public class ExplorationVessel : MonoBehaviour
    {
        public float explorationRadius = 50f;
        public float speed = 15f;

        public void ExploreNearby()
        {
            Debug.Log("Vessel exploring nearby waters...");
            // Discover new islands on the global map
            // This would call GlobalMapManager.DiscoverIslandInRange(transform.position, explorationRadius)
        }
    }

    public class CargoShip : Vehicle
    {
        public float seaSpeed = 8f;

        private void Awake()
        {
            speed = seaSpeed;
            cargo.maxWeight = 500f; // Much larger than land vehicles
        }
    }
}
