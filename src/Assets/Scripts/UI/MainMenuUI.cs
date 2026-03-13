using UnityEngine;
using UnityEngine.UI;
using CaudilloBay.Core;

namespace CaudilloBay.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [Header("Panels")]
        public GameObject mainMenuPanel;
        public GameObject optionsPanel;
        public GameObject loadPanel;
        public GameObject modeSelectionPanel;

        public void OnNewGameClicked()
        {
            ShowPanel(modeSelectionPanel);
        }

        public void OnLoadClicked()
        {
            ShowPanel(loadPanel);
        }

        public void OnOptionsClicked()
        {
            ShowPanel(optionsPanel);
        }

        public void OnExitClicked()
        {
            GameStateManager.Instance.QuitGame();
        }

        public void ShowMainMenu()
        {
            ShowPanel(mainMenuPanel);
        }

        private void ShowPanel(GameObject panelToShow)
        {
            mainMenuPanel.SetActive(false);
            optionsPanel.SetActive(false);
            loadPanel.SetActive(false);
            if (modeSelectionPanel != null) modeSelectionPanel.SetActive(false);

            panelToShow.SetActive(true);
        }
    }
}
