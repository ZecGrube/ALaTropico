using UnityEngine;

namespace CaudilloBay.Systems
{
    public enum ConnectionType { PowerLine, WaterPipe }

    [System.Serializable]
    public class UtilityConnection
    {
        public UtilityNode nodeA;
        public UtilityNode nodeB;
        public ConnectionType type;

        public float maxThroughput = 100f;
        public float length = 0f;
        public float efficiency = 0.95f; // Loss per unit length multiplier

        public bool isBroken = false;

        public UtilityConnection(UtilityNode a, UtilityNode b, ConnectionType t)
        {
            nodeA = a;
            nodeB = b;
            type = t;
            length = Vector3.Distance(a.transform.position, b.transform.position);
        }

        public UtilityNode GetOtherNode(UtilityNode node)
        {
            if (node == nodeA) return nodeB;
            if (node == nodeB) return nodeA;
            return null;
        }
    }
}
