using UnityEngine;
using UnityEngine.UI;
using CaudilloBay.Economy;

namespace CaudilloBay.UI
{
    public class CityServicesUI : MonoBehaviour
    {
        public Text totalGarbageText;
        public Text fleetStatusText;

        public void Update()
        {
            if (WasteManager.Instance != null)
            {
                totalGarbageText.text = "Island Garbage: " + WasteManager.Instance.totalGarbageOnIsland.ToString("F1") + " tons";
            }
        }
    }
}
