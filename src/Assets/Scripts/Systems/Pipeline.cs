using UnityEngine;

namespace CaudilloBay.Systems
{
    public class Pipeline : MonoBehaviour
    {
        public World.Building startBuilding;
        public World.Building endBuilding;

        public float flowCapacity = 100f;
        public Data.ResourceType resourceType;

        public void TransferResource()
        {
            if (startBuilding == null || endBuilding == null) return;

            float amount = startBuilding.inventory.GetAmount(resourceType);
            float transfer = Mathf.Min(amount, flowCapacity);

            if (transfer > 0)
            {
                startBuilding.inventory.TransferTo(endBuilding.inventory, resourceType, transfer);
                Debug.Log($"Pipeline transferred {transfer} units of {resourceType.resourceId}");
            }
        }
    }
}
