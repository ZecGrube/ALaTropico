using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.Data;
using CaudilloBay.AI;

namespace CaudilloBay.World
{
    public class CommercialBuilding : Building
    {
        public List<ResourceType> goodsForSale = new List<ResourceType>();
        public float salesTax = 0.1f;

        public void SellToCitizen(Citizen citizen, ResourceType item, float amount)
        {
            if (inventory.HasResource(item, amount))
            {
                float cost = item.baseValue * amount;
                if (citizen.personalWealth >= cost)
                {
                    citizen.personalWealth -= cost;
                    inventory.RemoveResource(item, amount);

                    float tax = cost * salesTax;
                    Economy.EconomyManager.Instance.AddFunds(tax);

                    Debug.Log($"{citizen.firstName} bought {item.resourceName} for ${cost}");
                }
            }
        }
    }
}
