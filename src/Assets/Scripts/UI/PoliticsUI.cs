using UnityEngine;
using UnityEngine.UI;
using CaudilloBay.Politics;

namespace CaudilloBay.UI
{
    public class PoliticsUI : MonoBehaviour
    {
        public Text loyaltyText;
        public Text mandateText;
        public Text cultureText;
        public GameObject decreePanel;
        public GameObject cultureTab;
        public Button openPoliticsButton;

        private void Start()
        {
            decreePanel.SetActive(false);
            if (cultureTab != null) cultureTab.SetActive(false);
            openPoliticsButton.onClick.AddListener(TogglePanel);
        }

        private void Update()
        {
            if (Core.InputManager.Instance != null && Core.InputManager.Instance.GetActionDown(Core.GameAction.OpenPolitics))
            {
                TogglePanel();
            }

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

            if (cultureText != null && CultureManager.Instance != null)
            {
                cultureText.text = $"Culture: {CultureManager.Instance.cultureLevel:F1}";
            }
        }

        public void TogglePanel()
        {
            decreePanel.SetActive(!decreePanel.activeSelf);
        }

        public void ToggleCultureTab()
        {
            if (cultureTab != null) cultureTab.SetActive(!cultureTab.activeSelf);
        }

        public void OnIssueDecree(Decree decree)
        {
            if (FactionManager.Instance != null)
            {
                FactionManager.Instance.ApplyDecree(decree);
            }
        }

        public void OnHoldFestival()
        {
            if (CultureManager.Instance != null && Economy.EconomyManager.Instance != null && Economy.EconomyManager.Instance.treasuryBalance >= 250f)
            {
                Economy.EconomyManager.Instance.treasuryBalance -= 250f;
                CultureManager.Instance.ApplyFestivalDecree();
            }
        }

        public void OnSubsidizeArts()
        {
            if (CultureManager.Instance != null)
            {
                CultureManager.Instance.ApplyArtsSubsidy();
            }
        }
        public void OnAntiCorruptionCampaign()
        {
            if (Economy.EconomyManager.Instance != null && CorruptionManager.Instance != null && Economy.EconomyManager.Instance.treasuryBalance >= 300f)
            {
                Economy.EconomyManager.Instance.treasuryBalance -= 300f;
                CorruptionManager.Instance.corruptionLevel = Mathf.Clamp(CorruptionManager.Instance.corruptionLevel - 8f, 0f, 100f);
            }
        }

        public void OnAmnestyForShadowEntrepreneurs()
        {
            if (CorruptionManager.Instance != null)
            {
                CorruptionManager.Instance.corruptionLevel = Mathf.Clamp(CorruptionManager.Instance.corruptionLevel + 5f, 0f, 100f);
                CorruptionManager.Instance.blackMarketMoney += 150f;
            }
        }

        public void OnBribeFaction(FactionType factionType)
        {
            if (FactionManager.Instance != null)
            {
                FactionManager.Instance.BribeFaction(factionType, 100f, 8f);
            }
        }
        public void OnAppointSuccessor()
        {
            if (DynastyManager.Instance == null || DynastyManager.Instance.heirs.Count == 0) return;
            DynastyManager.Instance.AppointSuccessor(DynastyManager.Instance.heirs[0]);
        }

        public void OnAdoptHeir()
        {
            if (DynastyManager.Instance == null) return;
            DynastyManager.Instance.AdoptHeir($"Adopted {Random.Range(100, 999)}");
        }

        public void OnSendHeirToMilitaryAcademy()
        {
            if (DynastyManager.Instance == null || DynastyManager.Instance.heirs.Count == 0) return;
            DynastyManager.Instance.SendHeirToMilitaryAcademy(DynastyManager.Instance.heirs[0]);
        }

    }
}
