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

            string educationInfo = "";
            if (CaudilloBay.Core.EducationManager.Instance != null)
            {
                educationInfo = $" | Edu: {CaudilloBay.Core.EducationManager.Instance.globalEducationLevel:F1}%";
            }

            string healthInfo = "";
            if (CaudilloBay.Core.HealthManager.Instance != null)
            {
                healthInfo = $" | HP: {CaudilloBay.Core.HealthManager.Instance.globalHealthLevel:F1}%";
            }

            string militaryInfo = "";
            if (CaudilloBay.Politics.MilitaryManager.Instance != null)
            {
                militaryInfo = $" | Army: {CaudilloBay.Politics.MilitaryManager.Instance.CalculateDefensePower():F0}";
            }

            string cultureInfo = "";
            if (CaudilloBay.Politics.CultureManager.Instance != null)
            {
                cultureInfo = $" | Culture: {CaudilloBay.Politics.CultureManager.Instance.cultureLevel:F0}";
            }

            string corruptionInfo = "";
            if (CaudilloBay.Politics.CorruptionManager.Instance != null)
            {
                corruptionInfo = $" | Corruption: {CaudilloBay.Politics.CorruptionManager.Instance.corruptionLevel:F0}% | Black$: {CaudilloBay.Politics.CorruptionManager.Instance.blackMarketMoney:F0}";
            }

            string dynastyInfo = "";
            if (CaudilloBay.Politics.DynastyManager.Instance != null && CaudilloBay.Politics.DynastyManager.Instance.currentRuler != null)
            {
                dynastyInfo = $" | Ruler: {CaudilloBay.Politics.DynastyManager.Instance.currentRuler.heirName}";
            }

            resourceText.text = $"Storage: {mainStorage.inventory.GetTotalWeight()} / {mainStorage.storageCapacity}{crimeInfo}{educationInfo}{healthInfo}{militaryInfo}{cultureInfo}{corruptionInfo}{dynastyInfo}";
        }
    }
}
