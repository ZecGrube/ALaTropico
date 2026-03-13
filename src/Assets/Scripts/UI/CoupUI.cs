using UnityEngine;
using UnityEngine.UI;
using CaudilloBay.Politics;

namespace CaudilloBay.UI
{
    public class CoupUI : MonoBehaviour
    {
        public GameObject coupPanel;
        public Text infoText;
        public Button suppressButton;
        public Button negotiateButton;

        private void Start()
        {
            coupPanel.SetActive(false);
            suppressButton.onClick.AddListener(() => Resolve(true));
            negotiateButton.onClick.AddListener(() => Resolve(false));
        }

        private void Update()
        {
            if (CoupManager.Instance != null && CoupManager.Instance.isCoupActive)
            {
                if (!coupPanel.activeSelf)
                {
                    coupPanel.SetActive(true);
                    infoText.text = $"COUP DETECTED!\nForce: {CoupManager.Instance.coupPower:F0}";
                }
            }
        }

        private void Resolve(bool suppress)
        {
            CoupManager.Instance.ResolveCoup(suppress);
            coupPanel.SetActive(false);
        }
    }
}
