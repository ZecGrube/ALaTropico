using UnityEngine;
using CaudilloBay.Systems;

namespace CaudilloBay.World
{
    public class CoalPowerPlant : ProducerBuilding
    {
        public UtilityNode utilityNode;

        protected override void Start()
        {
            base.Start();
            if (utilityNode != null)
            {
                utilityNode.nodeType = UtilityNodeType.Producer;
                utilityNode.utilityType = UtilityType.Power;
                utilityNode.capacity = 500f; // High capacity
                utilityNode.Register();
            }
        }

        public override void ProduceCycle()
        {
            // Requires Coal as input to produce power capacity
            if (inventory.HasResource(Data.ResourceType.Coal, 10f))
            {
                inventory.RemoveResource(Data.ResourceType.Coal, 10f);
                utilityNode.capacity = 500f;
            }
            else
            {
                utilityNode.capacity = 0f;
            }
            base.ProduceCycle();
        }
    }

    public class SolarPlant : Building
    {
        public UtilityNode utilityNode;

        protected override void Start()
        {
            base.Start();
            if (utilityNode != null)
            {
                utilityNode.nodeType = UtilityNodeType.Producer;
                utilityNode.utilityType = UtilityType.Power;
                utilityNode.capacity = 100f; // Clean but lower capacity
                utilityNode.Register();
            }
        }
    }

    public class PowerPole : MonoBehaviour
    {
        public UtilityNode utilityNode;

        private void Start()
        {
            if (utilityNode != null)
            {
                utilityNode.nodeType = UtilityNodeType.Junction;
                utilityNode.utilityType = UtilityType.Power;
                utilityNode.Register();
            }
        }
    }
}
