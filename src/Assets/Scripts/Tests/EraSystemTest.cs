using UnityEngine;
using CaudilloBay.Core;
using CaudilloBay.AI;
using CaudilloBay.Data;
using System.Collections.Generic;

namespace CaudilloBay.Tests
{
    public class EraSystemTest : MonoBehaviour
    {
        public void Start()
        {
            RunTests();
        }

        public void RunTests()
        {
            Debug.Log("--- Starting Era & Quest System Tests ---");
            TestQuestProgress();
            TestEraTransition();
            TestEraLockedContent();
            Debug.Log("--- Era & Quest System Tests Complete ---");
        }

        private void TestQuestProgress()
        {
            GameObject mgrGO = new GameObject("QuestManager");
            var mgr = mgrGO.AddComponent<QuestManager>();

            Quest q = ScriptableObject.CreateInstance<Quest>();
            q.questId = "test_pop";
            q.type = QuestType.ReachPopulation;
            q.requiredAmount = 100;

            mgr.AddQuest(q);
            mgr.NotifyProgress(QuestType.ReachPopulation, null, 50);

            if (mgr.activeQuests[0].currentProgress == 50)
                Debug.Log("[PASS] Quest Progress: Values updated correctly.");
            else
                Debug.LogError("[FAIL] Quest Progress: Values mismatch.");
        }

        private void TestEraTransition()
        {
            GameObject eraGO = new GameObject("EraManager");
            var eraMgr = eraGO.AddComponent<EraManager>();

            GameObject questGO = new GameObject("QuestManager");
            var questMgr = questGO.AddComponent<QuestManager>();

            Era colonial = ScriptableObject.CreateInstance<Era>();
            colonial.eraName = "Colonial";
            colonial.eraIndex = 0;

            Era industrial = ScriptableObject.CreateInstance<Era>();
            industrial.eraName = "Industrial";
            industrial.eraIndex = 1;

            colonial.nextEra = industrial;

            Quest mainQ = ScriptableObject.CreateInstance<Quest>();
            mainQ.questId = "main_1";
            mainQ.isMainQuest = true;
            mainQ.requiredAmount = 1;

            colonial.mainQuests = new List<Quest> { mainQ };
            eraMgr.allEras = new List<Era> { colonial, industrial };
            eraMgr.currentEra = colonial;

            questMgr.AddQuest(mainQ);
            questMgr.NotifyProgress(mainQ.type, mainQ.targetId, 1);

            if (eraMgr.currentEra.eraName == "Industrial")
                Debug.Log("[PASS] Era Transition: Automatically advanced after main quest.");
            else
                Debug.LogError("[FAIL] Era Transition: Stayed in old era.");
        }

        private void TestEraLockedContent()
        {
            GameObject eraGO = new GameObject("EraManager");
            var eraMgr = eraGO.AddComponent<EraManager>();
            Era colonial = ScriptableObject.CreateInstance<Era>();
            colonial.eraIndex = 0;
            eraMgr.currentEra = colonial;

            GameObject techGO = new GameObject("TechnologyManager");
            var techMgr = techGO.AddComponent<CaudilloBay.World.TechnologyManager>();

            Technology advancedTech = ScriptableObject.CreateInstance<Technology>();
            advancedTech.requiredEraIndex = 1;

            if (!techMgr.CanResearch(advancedTech))
                Debug.Log("[PASS] Era Locked Content: Advanced tech blocked in early era.");
            else
                Debug.LogError("[FAIL] Era Locked Content: Research allowed for future era.");
        }
    }
}
