using UnityEngine;
using UnityEngine.UI;
using CaudilloBay.Data;
using CaudilloBay.Core;

namespace CaudilloBay.UI
{
    public class EventNotificationUI : MonoBehaviour
    {
        public static EventNotificationUI Instance { get; private set; }

        public GameObject eventPanel;
        public Text titleText;
        public Text descriptionText;
        public Image eventImage;
        public Transform choiceContainer;
        public GameObject choiceButtonPrefab;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            if (eventPanel != null) eventPanel.SetActive(false);
        }

        public void ShowEvent(GameEvent ev)
        {
            if (eventPanel == null) return;

            eventPanel.SetActive(true);
            titleText.text = ev.title;
            descriptionText.text = ev.description;
            if (ev.eventImage != null) eventImage.sprite = ev.eventImage;

            // Clear old choices
            foreach (Transform child in choiceContainer) Destroy(child.gameObject);

            // Add new choices
            foreach (var choice in ev.choices)
            {
                GameObject btnObj = Instantiate(choiceButtonPrefab, choiceContainer);
                btnObj.GetComponentInChildren<Text>().text = choice.choiceText;
                btnObj.GetComponent<Button>().onClick.AddListener(() => SelectChoice(ev, choice));
            }
        }

        private void SelectChoice(GameEvent ev, EventChoice choice)
        {
            EventManager.Instance.ApplyChoiceEffects(choice);
            eventPanel.SetActive(false);
            Debug.Log($"Player chose: {choice.choiceText} for event {ev.title}");
        }
    }
}
