using UnityEngine;
using UnityEngine.UI;
using CaudilloBay.Core;

namespace CaudilloBay.UI
{
    public class ModeSelectionPanel : MonoBehaviour
    {
        [Header("Panels")]
        public GameObject campaignSelectionPanel;
        public GameObject sandboxSetupPanel;

        public void OnCampaignClicked()
        {
            gameObject.SetActive(false);
            campaignSelectionPanel.SetActive(true);
        }

        public void OnSandboxClicked()
        {
            gameObject.SetActive(false);
            sandboxSetupPanel.SetActive(true);
        }

        public void OnOnlineClicked()
        {
            Debug.Log("Online Mode coming soon!");
        }

        public void OnBackClicked()
        {
            gameObject.SetActive(false);
            // MainMenuUI handles showing its own panel usually
        }
    }
}
