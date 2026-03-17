using UnityEngine;
using UnityEngine.UI;
using CaudilloBay.Politics;
using CaudilloBay.Data;

namespace CaudilloBay.UI
{
    public class GlobalEventUI : MonoBehaviour
    {
        public static GlobalEventUI Instance { get; private set; }

        [Header("UI References")]
        public Text titleText;
        public Text descriptionText;
        public GameObject optionsContainer;
        public GameObject optionButtonPrefab;
        public GameObject panel;

        private GlobalEventInstance currentEvent;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            if (panel != null) panel.SetActive(false);
        }

        public void ShowEvent(GlobalEventInstance ev)
        {
            currentEvent = ev;
            titleText.text = ev.resolvedTitle;
            descriptionText.text = ev.resolvedDescription;

            // Clear old buttons
            foreach (Transform child in optionsContainer.transform) Destroy(child.gameObject);

            // Create new buttons
            foreach (var option in ev.template.options)
            {
                GameObject btnGO = Instantiate(optionButtonPrefab, optionsContainer.transform);
                btnGO.GetComponentInChildren<Text>().text = option.optionText;
                btnGO.GetComponent<Button>().onClick.AddListener(() => OnOptionSelected(option));
            }

            panel.SetActive(true);
        }

        private void OnOptionSelected(GlobalEventOption option)
        {
            GlobalEventGenerator.Instance.ResolveEvent(currentEvent, option);
            panel.SetActive(false);
            Debug.Log($"Resolved global event {currentEvent.resolvedTitle} with choice: {option.optionText}");
        }
    }
}
