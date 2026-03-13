using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using CaudilloBay.Core;

namespace CaudilloBay.UI
{
    public class LoadPanel : MonoBehaviour
    {
        [Header("UI References")]
        public GameObject saveEntryPrefab;
        public Transform listContainer;
        public SaveSystem saveSystem;

        private void OnEnable()
        {
            RefreshList();
        }

        public void RefreshList()
        {
            // Clear existing
            foreach (Transform child in listContainer)
            {
                Destroy(child.gameObject);
            }

            // Get all saves
            var saves = saveSystem.GetAllSaveFiles();

            foreach (var save in saves)
            {
                GameObject entry = Instantiate(saveEntryPrefab, listContainer);
                // Setup entry UI (Name, Date, LoadButton)
                // entry.GetComponent<SaveEntry>().Setup(save, this);
            }
        }

        public void LoadSave(string fileName)
        {
            GameStateManager.Instance.LoadExistingGame(fileName);
        }

        public void DeleteSave(string fileName)
        {
            saveSystem.DeleteSave(fileName);
            RefreshList();
        }
    }
}
