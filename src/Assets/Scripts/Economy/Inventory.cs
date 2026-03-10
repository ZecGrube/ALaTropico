using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.Data;

namespace CaudilloBay.Economy
{
    [System.Serializable]
    public class Inventory
    {
        public float maxWeight = 100f;
        private Dictionary<ResourceType, float> resources = new Dictionary<ResourceType, float>();

        public void AddResource(ResourceType type, float amount)
        {
            if (resources.ContainsKey(type))
            {
                resources[type] += amount;
            }
            else
            {
                resources.Add(type, amount);
            }
        }

        public bool RemoveResource(ResourceType type, float amount)
        {
            if (HasResource(type, amount))
            {
                resources[type] -= amount;
                return true;
            }
            return false;
        }

        public bool HasResource(ResourceType type, float amount)
        {
            return resources.ContainsKey(type) && resources[type] >= amount;
        }

        public float GetAmount(ResourceType type)
        {
            return resources.ContainsKey(type) ? resources[type] : 0;
        }

        public float GetTotalWeight()
        {
            float total = 0;
            foreach (var kvp in resources)
            {
                total += kvp.Key.unitWeight * kvp.Value;
            }
            return total;
        }

        public void TransferTo(Inventory other, ResourceType type, float amount)
        {
            float toTransfer = Mathf.Min(amount, GetAmount(type));
            if (toTransfer > 0)
            {
                RemoveResource(type, toTransfer);
                other.AddResource(type, toTransfer);
            }
        }
    }
}
