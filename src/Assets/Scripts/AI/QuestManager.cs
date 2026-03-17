using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.Data;

namespace CaudilloBay.AI
{
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager Instance { get; private set; }

        public List<QuestInstance> activeQuests = new List<QuestInstance>();
        public List<string> completedQuestIds = new List<string>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void AddQuest(Quest quest)
        {
            if (completedQuestIds.Contains(quest.questId)) return;
            if (activeQuests.Exists(q => q.template.questId == quest.questId)) return;

            activeQuests.Add(new QuestInstance(quest));
            Debug.Log($"Quest Started: {quest.questName}");
        }

        public void NotifyProgress(QuestType type, string targetId, int amount = 1)
        {
            for (int i = activeQuests.Count - 1; i >= 0; i--)
            {
                var q = activeQuests[i];
                if (q.template.type == type && q.template.targetId == targetId)
                {
                    q.AddProgress(amount);
                    if (q.isComplete)
                    {
                        CompleteQuest(q);
                    }
                }
            }
        }

        private void CompleteQuest(QuestInstance quest)
        {
            Debug.Log($"Quest Completed: {quest.template.questName}!");
            ApplyRewards(quest.template.rewards);

            completedQuestIds.Add(quest.template.questId);
            activeQuests.Remove(quest);

            if (quest.template.isMainQuest && Core.EraManager.Instance != null)
            {
                Core.EraManager.Instance.CheckEraTransition();
            }
        }

        private void ApplyRewards(List<QuestReward> rewards)
        {
            foreach (var r in rewards)
            {
                switch (r.type)
                {
                    case QuestReward.RewardType.Resources:
                        if (World.StatsManager.Instance != null)
                            World.StatsManager.Instance.AddResource(r.data, r.amount);
                        break;
                    case QuestReward.RewardType.Mandate:
                        if (Politics.FactionManager.Instance != null)
                            Politics.FactionManager.Instance.currentMandate += (int)r.amount;
                        break;
                    case QuestReward.RewardType.Legitimacy:
                        if (Politics.LegitimacySystem.Instance != null)
                            Politics.LegitimacySystem.Instance.currentLegitimacy += r.amount;
                        break;
                }
            }
        }
    }
}
