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
                string status = "Factions Loyalty:\n";
                foreach (var f in FactionManager.Instance.factions)
                {
                    status += $"{f.displayName}: {f.loyalty:F0}%\n";
                }
                loyaltyText.text = status;
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
