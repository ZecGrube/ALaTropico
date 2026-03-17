using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Systems
{
    public class PowerGridManager : MonoBehaviour
    {
        public static PowerGridManager Instance { get; private set; }

        private List<UtilityNode> nodes = new List<UtilityNode>();
        private List<UtilityNetwork> activeNetworks = new List<UtilityNetwork>();

        private bool needsRecalculation = false;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void RegisterNode(UtilityNode node)
        {
            if (!nodes.Contains(node))
            {
                nodes.Add(node);
                needsRecalculation = true;
            }
        }

        public void UnregisterNode(UtilityNode node)
        {
            if (nodes.Contains(node))
            {
                nodes.Remove(node);
                needsRecalculation = true;
            }
        }

        public void CreateConnection(UtilityNode a, UtilityNode b)
        {
            if (a.utilityType != UtilityType.Power || b.utilityType != UtilityType.Power) return;

            UtilityConnection conn = new UtilityConnection(a, b, ConnectionType.PowerLine);
            a.connections.Add(conn);
            b.connections.Add(conn);
            needsRecalculation = true;
        }

        private void Update()
        {
            if (needsRecalculation)
            {
                RecalculateNetworks();
                needsRecalculation = false;
            }
        }

        public void RecalculateNetworks()
        {
            activeNetworks.Clear();
            HashSet<UtilityNode> visited = new HashSet<UtilityNode>();

            foreach (var node in nodes)
            {
                if (!visited.Contains(node))
                {
                    UtilityNetwork network = new UtilityNetwork(UtilityType.Power);
                    BFS_Explore(node, visited, network);
                    activeNetworks.Add(network);
                }
            }

            foreach (var network in activeNetworks)
            {
                network.BalanceLoad();
            }
        }

        private void BFS_Explore(UtilityNode start, HashSet<UtilityNode> visited, UtilityNetwork network)
        {
            Queue<UtilityNode> queue = new Queue<UtilityNode>();
            queue.Enqueue(start);
            visited.Add(start);

            while (queue.Count > 0)
            {
                UtilityNode current = queue.Dequeue();
                network.nodes.Add(current);
                current.networkId = activeNetworks.Count;

                foreach (var conn in current.connections)
                {
                    if (conn.isBroken) continue;
                    UtilityNode neighbor = conn.GetOtherNode(current);
                    if (neighbor != null && !visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        queue.Enqueue(neighbor);
                    }
                }
            }
        }
    }

    public class UtilityNetwork
    {
        public UtilityType type;
        public List<UtilityNode> nodes = new List<UtilityNode>();

        public float totalSupply = 0f;
        public float totalDemand = 0f;

        public UtilityNetwork(UtilityType t) { type = t; }

        public void BalanceLoad()
        {
            totalSupply = 0f;
            totalDemand = 0f;

            foreach (var node in nodes)
            {
                if (node.nodeType == UtilityNodeType.Producer) totalSupply += node.capacity;
                else if (node.nodeType == UtilityNodeType.Consumer) totalDemand += node.demand;
                else if (node.nodeType == UtilityNodeType.Storage) totalSupply += node.currentStorage;
            }

            bool satisfied = totalSupply >= totalDemand && totalSupply > 0;

            foreach (var node in nodes)
            {
                if (node.nodeType == UtilityNodeType.Consumer)
                {
                    node.isSatisfied = satisfied;
                }
            }
        }
    }
}
