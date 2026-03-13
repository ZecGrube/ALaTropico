using UnityEngine;
using UnityEngine.UI;
using CaudilloBay.Core;

namespace CaudilloBay.UI
{
    public class LocalizedText : MonoBehaviour
    {
        public string key;
        private Text textComponent;

        private void Start()
        {
            textComponent = GetComponent<Text>();
            UpdateText();
            if (LocalizationManager.Instance != null)
                LocalizationManager.Instance.OnLanguageChanged += UpdateText;
        }

        private void OnDestroy()
        {
            if (LocalizationManager.Instance != null)
                LocalizationManager.Instance.OnLanguageChanged -= UpdateText;
        }

        public void UpdateText()
        {
            if (textComponent != null && LocalizationManager.Instance != null)
                textComponent.text = LocalizationManager.Instance.GetText(key);
        }
    }
}
