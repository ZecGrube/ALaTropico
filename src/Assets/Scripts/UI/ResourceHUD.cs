using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using CaudilloBay.Data;
using CaudilloBay.World;

namespace CaudilloBay.UI
{
    public class ResourceHUD : MonoBehaviour
    {
        public Text resourceText;
        public StorageBuilding mainStorage;

        private void Update()
        {
            if (mainStorage == null)
            {
                mainStorage = UnityEngine.Object.FindAnyObjectByType<StorageBuilding>();
                return;
            }

            // Display a few key resources for demonstration
            // In a real game, this would loop through all tracked resources
            string crimeInfo = "";
            if (CaudilloBay.Core.CrimeManager.Instance != null)
            {
                crimeInfo = $" | Crime: {CaudilloBay.Core.CrimeManager.Instance.globalCrimeRate:F1}%";
            }

            resourceText.text = $"Storage: {mainStorage.inventory.GetTotalWeight()} / {mainStorage.storageCapacity}{crimeInfo}";
        }
    }
}
