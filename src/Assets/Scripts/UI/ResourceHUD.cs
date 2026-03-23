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
                militaryInfo = $" | MIL: {CaudilloBay.Politics.MilitaryManager.Instance.totalMilitaryStrength:F0}";
            }

            string cultureInfo = "";
            if (CaudilloBay.Core.CultureManager.Instance != null)
            {
                cultureInfo = $" | CULT: {CaudilloBay.Core.CultureManager.Instance.globalCultureLevel:F1}%";
            }

            string corruptionInfo = "";
            if (CaudilloBay.Core.CorruptionManager.Instance != null)
            {
                corruptionInfo = $" | CORR: {CaudilloBay.Core.CorruptionManager.Instance.globalCorruptionRate:F1}% | BLACK: ${CaudilloBay.Core.CorruptionManager.Instance.blackMarketMoney:F0}";
            }

            string nuclearInfo = "";
            if (CaudilloBay.Systems.Nuclear.NuclearManager.Instance != null)
            {
                nuclearInfo = $" | TENSION: {CaudilloBay.Systems.Nuclear.NuclearManager.Instance.nuclearTension:F1} | DET: {CaudilloBay.Systems.Nuclear.NuclearManager.Instance.currentDeterrence:F0}";
            }

            resourceText.text = $"Storage: {mainStorage.inventory.GetTotalWeight()} / {mainStorage.storageCapacity}{crimeInfo}{educationInfo}{healthInfo}{militaryInfo}{cultureInfo}{corruptionInfo}{nuclearInfo}";
        }
    }
}
