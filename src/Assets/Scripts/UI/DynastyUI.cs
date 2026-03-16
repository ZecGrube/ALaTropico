using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using CaudilloBay.Politics;

namespace CaudilloBay.UI
{
    public class DynastyUI : MonoBehaviour
    {
        public static DynastyUI Instance { get; private set; }

        public GameObject dynastyPanel;
        public Text rulerInfoText;
        public Transform heirContainer;
        public GameObject heirEntryPrefab;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            if (dynastyPanel != null) dynastyPanel.SetActive(false);
        }

        public void TogglePanel()
        {
            dynastyPanel.SetActive(!dynastyPanel.activeSelf);
            if (dynastyPanel.activeSelf) RefreshUI();
        }

        public void RefreshUI()
        {
            if (DynastyManager.Instance == null) return;

            var ruler = DynastyManager.Instance.currentRuler;
            rulerInfoText.text = ruler != null ? $"Current Ruler: {ruler.name} (Age: {ruler.age})\nCHR: {ruler.charisma} INT: {ruler.intelligence}" : "No active ruler.";

            foreach (Transform child in heirContainer) Destroy(child.gameObject);

            foreach (var heir in DynastyManager.Instance.activeHeirs)
            {
                GameObject entry = Instantiate(heirEntryPrefab, heirContainer);
                entry.GetComponentInChildren<Text>().text = $"{heir.name} (Age: {heir.age}) - Skills: {heir.charisma}/{heir.intelligence}/{heir.military}";
            }
        }
    }
}
