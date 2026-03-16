using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.Core;
using CaudilloBay.Politics;
using CaudilloBay.World;
using CaudilloBay.Economy;

namespace CaudilloBay.Tests
{
    public class ReligionCultSystemTest : MonoBehaviour
    {
        public void Start()
        {
            RunTests();
        }

        public void RunTests()
        {
            Debug.Log("--- Starting Religion and Cult System Tests ---");

            TestReligiosityGrowth();
            TestLeaderSpawning();
            TestCultBonuses();
            TestTourismImpact();
            TestPersistence();

            Debug.Log("--- Religion and Cult System Tests Complete ---");
        }

        private void TestReligiosityGrowth()
        {
            GameObject managerGO = new GameObject("ReligionManager");
            var manager = managerGO.AddComponent<ReligionManager>();

            GameObject churchGO = new GameObject("Church");
            var church = churchGO.AddComponent<Church>();
            church.influencePower = 10f;

            manager.RegisterTemple(church);

            if (manager.religiousInfluence == 10f)
            {
                Debug.Log("[PASS] Religiosity Growth");
            }
            else
            {
                Debug.LogError("[FAIL] Religiosity Growth. Influence: " + manager.religiousInfluence);
            }
        }

        private void TestLeaderSpawning()
        {
            var manager = ReligionManager.Instance;
            manager.UpdateInfluence(); // Should be 10 from previous test

            // Add another church to cross 50 threshold
            GameObject cathedralGO = new GameObject("Cathedral");
            var cathedral = cathedralGO.AddComponent<Cathedral>();
            cathedral.influencePower = 50f;
            manager.RegisterTemple(cathedral);

            if (manager.currentLeader != null)
            {
                Debug.Log("[PASS] Leader Spawning: " + manager.currentLeader.agentName);
            }
            else
            {
                Debug.LogError("[FAIL] Leader Spawning");
            }
        }

        private void TestCultBonuses()
        {
            GameObject cultGO = new GameObject("PersonalityCultManager");
            var manager = cultGO.AddComponent<PersonalityCultManager>();

            GameObject statueGO = new GameObject("Statue");
            var statue = statueGO.AddComponent<StatueOfPresident>();
            statue.cultBonus = 5f;
            manager.RegisterCultAsset(statue);

            if (manager.cultLevel == 5f)
            {
                Debug.Log("[PASS] Cult Level Growth");
            }
            else
            {
                Debug.LogError("[FAIL] Cult Level Growth. Level: " + manager.cultLevel);
            }
        }

        private void TestTourismImpact()
        {
            GameObject touristGO = new GameObject("TouristManager");
            var manager = touristGO.AddComponent<TouristManager>();

            float initialAttractiveness = 0f;
            manager.UpdateTourism(initialAttractiveness, 1.0f);
            int touristsWithoutReligion = manager.currentTourists;

            // Attractiveness should increase due to ReligionManager and PersonalityCultManager
            manager.UpdateTourism(initialAttractiveness, 1.0f);

            if (manager.currentTourists > touristsWithoutReligion)
            {
                Debug.Log("[PASS] Tourism Impact. Tourists with religion: " + manager.currentTourists);
            }
            else
            {
                Debug.LogError("[FAIL] Tourism Impact. Initial: " + touristsWithoutReligion + " Current: " + manager.currentTourists);
            }
        }

        private void TestPersistence()
        {
            GameObject saveGO = new GameObject("SaveSystem");
            var saveSys = saveGO.AddComponent<SaveSystem>();

            ReligionManager.Instance.religiousInfluence = 88f;
            PersonalityCultManager.Instance.cultLevel = 44f;

            saveSys.SaveGame("test_religion_cult.json");

            ReligionManager.Instance.religiousInfluence = 0f;
            PersonalityCultManager.Instance.cultLevel = 0f;

            saveSys.LoadGame("test_religion_cult.json");

            if (ReligionManager.Instance.religiousInfluence == 88f && PersonalityCultManager.Instance.cultLevel == 44f)
            {
                Debug.Log("[PASS] Religion/Cult Persistence");
            }
            else
            {
                Debug.LogError("[FAIL] Religion/Cult Persistence");
            }
        }
    }
}
