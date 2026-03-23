using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CaudilloBay.Politics;

namespace CaudilloBay.UI
{
    public class MilitaryUI : MonoBehaviour
    {
        public static MilitaryUI Instance { get; private set; }

        [Header("Global Stats")]
        public TextMeshProUGUI strengthText;
        public TextMeshProUGUI loyaltyText;

        [Header("Army Management")]
        public GameObject armyListContainer;
        public GameObject armyEntryPrefab;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Update()
        {
            RefreshStats();
        }

        private void RefreshStats()
        {
            if (MilitaryManager.Instance != null)
            {
                strengthText.text = $"Strength: {MilitaryManager.Instance.totalMilitaryStrength:F1}";
                loyaltyText.text = $"Loyalty: {MilitaryManager.Instance.armyLoyalty:F0}%";
            }
        }

        public void RebuildArmyList()
        {
            foreach (Transform child in armyListContainer.transform) Destroy(child.gameObject);

            if (MilitaryManager.Instance != null)
            {
                foreach (var army in MilitaryManager.Instance.activeArmies)
                {
                    GameObject go = Instantiate(armyEntryPrefab, armyListContainer.transform);
                    go.GetComponentInChildren<TextMeshProUGUI>().text = $"{army.armyName} (Power: {army.GetTotalPower():F1})";
                }
            }
        }
    }
}
