using UnityEngine;
using UnityEngine.UI;
using CaudilloBay.Politics;

namespace CaudilloBay.UI
{
    public class PoliticsUI : MonoBehaviour
    {
        public Text loyaltyText;
        public Text mandateText;
        public GameObject decreePanel;
        public Button openPoliticsButton;

        private void Start()
        {
            decreePanel.SetActive(false);
            openPoliticsButton.onClick.AddListener(TogglePanel);
        }

        private void Update()
        {
            if (FactionManager.Instance != null)
            {
                loyaltyText.text = $"Peasants Loyalty: {FactionManager.Instance.peasantFaction.loyalty:F0}%";
                mandateText.text = $"Mandate: {FactionManager.Instance.currentMandate}";
            }
        }

        public void TogglePanel()
        {
            decreePanel.SetActive(!decreePanel.activeSelf);
        }

        public void OnIssueDecree(Decree decree)
        {
            if (FactionManager.Instance != null)
            {
                FactionManager.Instance.ApplyDecree(decree);
            }
        }
    }
}
