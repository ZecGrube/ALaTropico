using UnityEngine;
using CaudilloBay.Systems;

namespace CaudilloBay.World
{
    public class WaterPump : Building
    {
        public UtilityNode utilityNode;

        protected override void Start()
        {
            base.Start();
            if (utilityNode != null)
            {
                utilityNode.nodeType = UtilityNodeType.Producer;
                utilityNode.utilityType = UtilityType.Water;
                utilityNode.capacity = 200f;
                utilityNode.Register();
            }
        }
    }

    public class WaterTower : Building
    {
        public UtilityNode utilityNode;

        protected override void Start()
        {
            base.Start();
            if (utilityNode != null)
            {
                utilityNode.nodeType = UtilityNodeType.Storage;
                utilityNode.utilityType = UtilityType.Water;
                utilityNode.capacity = 1000f;
                utilityNode.currentStorage = 500f; // Start partially full
                utilityNode.Register();
            }
        }
    }
}
