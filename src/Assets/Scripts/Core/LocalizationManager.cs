using UnityEngine;
using System.Collections.Generic;
using System;

namespace CaudilloBay.Core
{
    public class LocalizationManager : MonoBehaviour
    {
        public static LocalizationManager Instance { get; private set; }

        private Dictionary<string, string> currentLocalization = new Dictionary<string, string>();
        public string currentLanguage = "en";

        public event Action OnLanguageChanged;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                LoadLanguage(currentLanguage);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void LoadLanguage(string lang)
        {
            currentLanguage = lang;
            TextAsset textAsset = Resources.Load<TextAsset>($"Localization/{lang}");
            if (textAsset != null)
            {
                // Simplified JSON parsing: in a real project use a proper library
                var data = JsonUtility.FromJson<LocalizationData>(textAsset.text);
                currentLocalization.Clear();
                foreach (var item in data.items)
                {
                    currentLocalization[item.key] = item.value;
                }
                OnLanguageChanged?.Invoke();
            }
        }

        public string GetText(string key)
        {
            return currentLocalization.ContainsKey(key) ? currentLocalization[key] : key;
        }

        [Serializable]
        private class LocalizationItem { public string key; public string value; }
        [Serializable]
        private class LocalizationData { public List<LocalizationItem> items; }
    }
}
