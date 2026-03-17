using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CaudilloBay.Core;
using CaudilloBay.AI;

namespace CaudilloBay.UI
{
    public class EraQuestUI : MonoBehaviour
    {
        public static EraQuestUI Instance { get; private set; }

        [Header("Era Display")]
        public TextMeshProUGUI eraNameText;
        public Image eraIcon;
        public TextMeshProUGUI eraDescriptionText;

        [Header("Quest List")]
        public GameObject questContainer;
        public GameObject questPrefab;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Update()
        {
            RefreshUI();
        }

        public void RefreshUI()
        {
            if (EraManager.Instance != null && EraManager.Instance.currentEra != null)
            {
                eraNameText.text = EraManager.Instance.currentEra.eraName;
                eraDescriptionText.text = EraManager.Instance.currentEra.description;
                if (eraIcon != null) eraIcon.sprite = EraManager.Instance.currentEra.icon;
            }

            if (QuestManager.Instance != null)
            {
                // Clear and rebuild quest list (in a real project use pooling)
                foreach (Transform child in questContainer.transform) Destroy(child.gameObject);

                foreach (var q in QuestManager.Instance.activeQuests)
                {
                    GameObject go = Instantiate(questPrefab, questContainer.transform);
                    var txt = go.GetComponentInChildren<TextMeshProUGUI>();
                    if (txt != null) txt.text = $"{q.template.questName} ({q.currentProgress}/{q.template.requiredAmount})";
                }
            }
        }
    }
}
