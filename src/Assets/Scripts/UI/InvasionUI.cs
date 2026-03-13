using UnityEngine;
using UnityEngine.UI;
using CaudilloBay.Politics;
using CaudilloBay.Core;

namespace CaudilloBay.UI
{
    public class InvasionUI : MonoBehaviour
    {
        public static InvasionUI Instance { get; private set; }

        public GameObject panel;
        public Text titleText;
        public Text descriptionText;
        public Button resistButton;
        public Button negotiateButton;
        public Button aidButton;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            if (panel != null) panel.SetActive(false);
        }

        public void ShowInvasion(Invasion invasion)
        {
            if (panel == null || invasion == null) return;

            panel.SetActive(true);
            if (titleText != null) titleText.text = "Invasion Incoming";
            if (descriptionText != null)
                descriptionText.text = $"{invasion.invader} attacks ({invasion.invasionStrength:F0}) targeting {invasion.target}.";

            if (resistButton != null)
                resistButton.onClick.AddListener(() => ResolveAndClose(InvasionAction.Resist));
            if (negotiateButton != null)
                negotiateButton.onClick.AddListener(() => ResolveAndClose(InvasionAction.Negotiate));
            if (aidButton != null)
                aidButton.onClick.AddListener(() => ResolveAndClose(InvasionAction.RequestAid));
        }

        private void ResolveAndClose(InvasionAction action)
        {
            if (EventManager.Instance != null)
            {
                EventManager.Instance.ResolveCurrentInvasion(action);
            }

            if (resistButton != null) resistButton.onClick.RemoveAllListeners();
            if (negotiateButton != null) negotiateButton.onClick.RemoveAllListeners();
            if (aidButton != null) aidButton.onClick.RemoveAllListeners();

            if (panel != null) panel.SetActive(false);
        }
    }
}
