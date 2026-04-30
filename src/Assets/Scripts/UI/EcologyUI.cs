using UnityEngine;
using UnityEngine.UI;
using CaudilloBay.Ecology;
using System.Collections.Generic;
using System.Linq;

namespace CaudilloBay.UI
{
    public class EcologyUI : MonoBehaviour
    {
        [Header("Overview")]
        public Text totalBiodiversityText;
        public Text ecoTourismBonusText;
        public Transform zoneListParent;
        public GameObject zoneEntryPrefab;

        [Header("Controls")]
        public Toggle huntingToggle;
        public Button plantTreesButton;

        private void OnEnable()
        {
            RefreshUI();
        }

        public void RefreshUI()
        {
            if (EcosystemManager.Instance == null) return;

            totalBiodiversityText.text = $"Total Biodiversity: {EcosystemManager.Instance.GetTotalBiodiversity():F1}%";
            ecoTourismBonusText.text = $"Eco-Tourism Bonus: +{EcosystemManager.Instance.GetEcoTourismBonus():F0}";

            huntingToggle.isOn = EcosystemManager.Instance.isHuntingLegal;

            // Clear old entries
            foreach (Transform child in zoneListParent) Destroy(child.gameObject);

            // Populate zones
            foreach (var zone in EcosystemManager.Instance.zones)
            {
                GameObject go = Instantiate(zoneEntryPrefab, zoneListParent);
                go.GetComponentInChildren<Text>().text = $"{zone.type} - Health: {zone.health:F0}% - Pop: {zone.animalPopulations.Values.Sum()}";
            }
        }

        public void ToggleHunting(bool value)
        {
            if (EcosystemManager.Instance != null)
                EcosystemManager.Instance.isHuntingLegal = value;
        }

        public void OnPlantTreesClicked()
        {
            // Implementation for selecting a zone and planting trees
            Debug.Log("Tree planting action triggered.");
        }
    }
}
