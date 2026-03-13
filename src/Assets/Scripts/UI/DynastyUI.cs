using UnityEngine;
using UnityEngine.UI;
using System.Text;
using CaudilloBay.Politics;

namespace CaudilloBay.UI
{
    public class DynastyUI : MonoBehaviour
    {
        public GameObject panel;
        public Button openButton;
        public Text rulerText;
        public Text heirsText;

        private void Start()
        {
            if (panel != null) panel.SetActive(false);
            if (openButton != null) openButton.onClick.AddListener(TogglePanel);
        }

        private void Update()
        {
            if (panel == null || !panel.activeSelf) return;
            Refresh();
        }

        public void TogglePanel()
        {
            if (panel == null) return;
            panel.SetActive(!panel.activeSelf);
            if (panel.activeSelf) Refresh();
        }

        private void Refresh()
        {
            if (DynastyManager.Instance == null) return;

            Heir ruler = DynastyManager.Instance.currentRuler;
            if (rulerText != null && ruler != null)
            {
                rulerText.text = $"Ruler: {ruler.heirName} | Age: {ruler.age} | Charisma: {ruler.charisma:F0} | Int: {ruler.intelligence:F0}";
            }

            if (heirsText != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Heirs:");
                foreach (var heir in DynastyManager.Instance.heirs)
                {
                    if (heir == null) continue;
                    sb.AppendLine($"- {heir.heirName} ({heir.age}y) Loy:{heir.loyaltyToRuler:F0} Sup:{heir.GetAverageSupport():F0} Alive:{heir.isAlive}");
                }
                heirsText.text = sb.ToString();
            }
        }
    }
}
