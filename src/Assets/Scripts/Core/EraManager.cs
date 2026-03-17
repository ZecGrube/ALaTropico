using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.Data;

namespace CaudilloBay.Core
{
    public class EraManager : MonoBehaviour
    {
        public static EraManager Instance { get; private set; }

        public Era currentEra;
        public List<Era> allEras = new List<Era>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            if (currentEra == null && allEras.Count > 0)
            {
                SetEra(allEras[0]);
            }
        }

        public void CheckEraTransition()
        {
            if (currentEra == null || currentEra.nextEra == null) return;

            // Check if all main quests for current era are in completedQuestIds
            if (AI.QuestManager.Instance == null) return;

            bool allMainComplete = true;
            foreach (var q in currentEra.mainQuests)
            {
                if (!AI.QuestManager.Instance.completedQuestIds.Contains(q.questId))
                {
                    allMainComplete = false;
                    break;
                }
            }

            if (allMainComplete)
            {
                TransitionToNextEra();
            }
        }

        public void TransitionToNextEra()
        {
            if (currentEra.nextEra == null) return;

            Era next = currentEra.nextEra;
            Debug.Log($"HISTORICAL MILESTONE: Island has entered the {next.eraName}!");

            SetEra(next);

            // Notification UI
            if (UI.EventNotificationUI.Instance != null)
            {
                // UI trigger for era change
            }
        }

        private void SetEra(Era era)
        {
            currentEra = era;

            // Load quests for the new era
            if (AI.QuestManager.Instance != null)
            {
                foreach (var q in era.mainQuests) AI.QuestManager.Instance.AddQuest(q);
                foreach (var q in era.sideQuests) AI.QuestManager.Instance.AddQuest(q);
            }
        }
    }
}
