using UnityEngine;
using UnityEngine.UI;
using CaudilloBay.Colonization;
using System.Collections.Generic;

namespace CaudilloBay.UI
{
    public class ColonyUI : MonoBehaviour
    {
        public Text colonyNameText;
        public Text populationText;
        public Text loyaltyText;
        public Dropdown policyDropdown;

        private Colony currentColony;

        public void OpenColonyInfo(Colony colony)
        {
            currentColony = colony;
            RefreshUI();
            gameObject.SetActive(true);
        }

        private void RefreshUI()
        {
            if (currentColony == null) return;
            colonyNameText.text = currentColony.name;
            populationText.text = $"Population: {currentColony.population}";
            loyaltyText.text = $"Loyalty: {currentColony.loyalty:F0}%";
            policyDropdown.value = (int)currentColony.autoPolicy;
        }

        public void OnPolicyChanged(int index)
        {
            if (currentColony == null) return;
            currentColony.autoPolicy = (ColonyPolicy)index;
            Debug.Log($"Colony {currentColony.name} policy changed to {currentColony.autoPolicy}");
        }
    }
}
