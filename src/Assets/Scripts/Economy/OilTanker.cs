using UnityEngine;

namespace CaudilloBay.Economy
{
    public class OilTanker : Vehicle
    {
        public override void ProcessOrder(TransportOrder order)
        {
            base.ProcessOrder(order);
            Debug.Log($"Oil Tanker {name} is transporting {order.amount} of {order.resourceId} to port.");
        }
    }
}
