using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.Economy;

namespace CaudilloBay.Maritime
{
    public class MaritimeLogisticsManager : MonoBehaviour
    {
        public static MaritimeLogisticsManager Instance { get; private set; }

        public List<CargoShip> ships = new List<CargoShip>();
        public List<ShipRoute> activeRoutes = new List<ShipRoute>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void CreateRoute(string fromColonyId, string toColonyId, Data.ResourceType resource, float amount)
        {
            activeRoutes.Add(new ShipRoute {
                sourceId = fromColonyId,
                destId = toColonyId,
                resourceType = resource,
                periodicAmount = amount
            });
            Debug.Log($"Maritime route created from {fromColonyId} to {toColonyId}");
        }
    }

    [System.Serializable]
    public class ShipRoute
    {
        public string sourceId;
        public string destId;
        public Data.ResourceType resourceType;
        public float periodicAmount;
    }
}
