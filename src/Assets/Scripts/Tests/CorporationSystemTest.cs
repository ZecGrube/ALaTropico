using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.Economy;
using CaudilloBay.Politics;
using CaudilloBay.World;
using CaudilloBay.Data;

namespace CaudilloBay.Tests
{
    public class CorporationSystemTest : MonoBehaviour
    {
        public void Start()
        {
            RunTests();
        }

        public void RunTests()
        {
            Debug.Log("--- Starting Corporation System Tests ---");

            TestCorporationCreation();
            TestProfitRedirection();
            TestLobbying();
            TestTaxation();
            TestSaveLoadConsistency();

            Debug.Log("--- Corporation System Tests Complete ---");
        }

        private void TestCorporationCreation()
        {
            GameObject managerGO = new GameObject("CorpManager");
            var manager = managerGO.AddComponent<CorporationManager>();

            var corp = manager.CreateCorporation("TestCorp", CorporationType.Private, IndustryType.Industry, new List<Building>());

            if (corp != null && manager.corporations.Contains(corp))
            {
                Debug.Log("[PASS] Corporation Creation");
            }
            else
            {
                Debug.LogError("[FAIL] Corporation Creation");
            }
        }

        private void TestProfitRedirection()
        {
            // Setup mock EconomyManager
            GameObject econGO = new GameObject("EconManager");
            var econ = econGO.AddComponent<EconomyManager>();

            GameObject managerGO = GameObject.Find("CorpManager");
            var manager = managerGO.GetComponent<CorporationManager>();

            var corp = manager.corporations[0];
            corp.treasury = 0;

            // Mock Building
            GameObject bGO = new GameObject("TestBuilding");
            var building = bGO.AddComponent<MockBuilding>();
            building.ownerCorporation = corp;
            corp.ownedBuildings.Add(building);

            econ.ProcessMonthlyEconomy(new List<Building> { building });

            if (corp.treasury > 0)
            {
                Debug.Log($"[PASS] Profit Redirection. Corp Treasury: {corp.treasury}");
            }
            else
            {
                Debug.LogError("[FAIL] Profit Redirection");
            }
        }

        private void TestLobbying()
        {
            GameObject managerGO = GameObject.Find("CorpManager");
            var manager = managerGO.GetComponent<CorporationManager>();
            var corp = manager.corporations[0];
            corp.treasury = 1000f;

            // Mock FactionManager
            GameObject factionGO = new GameObject("FactionManager");
            var factionMgr = factionGO.AddComponent<FactionManager>();
            factionMgr.factions.Add(new FactionData { type = FactionType.Capitalists, loyalty = 50f });

            corp.Lobby(FactionType.Capitalists, 500f);

            var capitalists = factionMgr.factions.Find(f => f.type == FactionType.Capitalists);
            if (capitalists.loyalty > 50f && corp.treasury == 500f)
            {
                Debug.Log($"[PASS] Lobbying. New loyalty: {capitalists.loyalty}");
            }
            else
            {
                Debug.LogError("[FAIL] Lobbying");
            }
        }

        private void TestTaxation()
        {
            GameObject managerGO = GameObject.Find("CorpManager");
            var manager = managerGO.GetComponent<CorporationManager>();
            var corp = manager.corporations[0];
            corp.treasury = 1000f;

            float initialTreasury = EconomyManager.Instance.treasuryBalance;
            manager.ProcessMonthlyFinances();

            if (corp.treasury < 1000f && EconomyManager.Instance.treasuryBalance > initialTreasury)
            {
                Debug.Log("[PASS] Taxation Logic");
            }
            else
            {
                Debug.LogError("[FAIL] Taxation Logic");
            }
        }

        private void TestSaveLoadConsistency()
        {
            GameObject managerGO = GameObject.Find("CorpManager");
            var manager = managerGO.GetComponent<CorporationManager>();
            var corp = manager.corporations[0];
            corp.treasury = 777f;

            // Trigger save
            GameObject saveGO = new GameObject("SaveSystem");
            var saveSys = saveGO.AddComponent<Core.SaveSystem>();
            saveSys.SaveGame("test_serialization.json");

            // Clear and load
            corp.treasury = 0f;
            saveSys.LoadGame("test_serialization.json");

            if (manager.corporations[0].treasury == 777f)
            {
                Debug.Log("[PASS] Save/Load Consistency");
            }
            else
            {
                Debug.LogError($"[FAIL] Save/Load Consistency. Treasury: {manager.corporations[0].treasury}");
            }
        }
    }

    public class MockBuilding : Building
    {
        private void Awake()
        {
            isConstructed = true;
            data = ScriptableObject.CreateInstance<BuildingData>();
            data.maintenanceCost = 10f;
        }
    }
}
