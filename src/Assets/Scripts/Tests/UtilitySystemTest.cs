using UnityEngine;
using CaudilloBay.Systems;
using CaudilloBay.World;

namespace CaudilloBay.Tests
{
    public class UtilitySystemTest : MonoBehaviour
    {
        public void Start()
        {
            RunTests();
        }

        public void RunTests()
        {
            Debug.Log("--- Starting Utility System Tests ---");
            TestPowerGridFlow();
            TestBuildingUtilityRequirement();
            TestWaterStorageUsage();
            Debug.Log("--- Utility System Tests Complete ---");
        }

        private void TestPowerGridFlow()
        {
            GameObject mgrGO = new GameObject("PowerGridManager");
            var mgr = mgrGO.AddComponent<PowerGridManager>();

            GameObject genGO = new GameObject("Generator");
            var generator = genGO.AddComponent<MockUtilityNode>();
            generator.nodeType = UtilityNodeType.Producer;
            generator.utilityType = UtilityType.Power;
            generator.capacity = 100f;
            generator.Register();

            GameObject conGO = new GameObject("Consumer");
            var consumer = conGO.AddComponent<MockUtilityNode>();
            consumer.nodeType = UtilityNodeType.Consumer;
            consumer.utilityType = UtilityType.Power;
            consumer.demand = 50f;
            consumer.Register();

            // Connect them
            mgr.CreateConnection(generator, consumer);
            mgr.RecalculateNetworks();

            if (consumer.isSatisfied)
                Debug.Log("[PASS] Power Grid Flow: Consumer satisfied.");
            else
                Debug.LogError("[FAIL] Power Grid Flow: Consumer not satisfied.");
        }

        private void TestBuildingUtilityRequirement()
        {
            GameObject bGO = new GameObject("ProducerBuilding");
            var pb = bGO.AddComponent<ProducerBuilding>();
            pb.requiresPower = true;

            GameObject nodeGO = new GameObject("Node");
            var node = nodeGO.AddComponent<MockUtilityNode>();
            node.isSatisfied = false;
            pb.powerNode = node;

            if (!pb.IsFunctional())
                Debug.Log("[PASS] Building Utility Requirement: Building non-functional without power.");
            else
                Debug.LogError("[FAIL] Building Utility Requirement: Building functional without power.");

            node.isSatisfied = true;
            if (pb.IsFunctional())
                Debug.Log("[PASS] Building Utility Requirement: Building functional with power.");
            else
                Debug.LogError("[FAIL] Building Utility Requirement: Building non-functional despite power.");
        }

        private void TestWaterStorageUsage()
        {
            GameObject mgrGO = new GameObject("WaterNetworkManager");
            var mgr = mgrGO.AddComponent<WaterNetworkManager>();

            GameObject storeGO = new GameObject("WaterTower");
            var tower = storeGO.AddComponent<MockUtilityNode>();
            tower.nodeType = UtilityNodeType.Storage;
            tower.utilityType = UtilityType.Water;
            tower.currentStorage = 100f;
            tower.Register();

            GameObject conGO = new GameObject("Consumer");
            var consumer = conGO.AddComponent<MockUtilityNode>();
            consumer.nodeType = UtilityNodeType.Consumer;
            consumer.utilityType = UtilityType.Water;
            consumer.demand = 10f;
            consumer.Register();

            mgr.CreateConnection(tower, consumer);
            mgr.RecalculateNetworks();

            if (consumer.isSatisfied)
                Debug.Log("[PASS] Water Storage Usage: Consumer satisfied by storage.");
            else
                Debug.LogError("[FAIL] Water Storage Usage: Consumer not satisfied by storage.");
        }
    }

    public class MockUtilityNode : UtilityNode { }
}
