using UnityEngine;
using UnityEngine.UI;
using CaudilloBay.AI;

namespace CaudilloBay.UI
{
    public class DemographicsUI : MonoBehaviour
    {
        public Text populationCountText;
        public Text unemploymentText;
        public Text avgSatisfactionText;

        public void Update()
        {
            if (PopulationManager.Instance != null)
            {
                populationCountText.text = "Population: " + PopulationManager.Instance.allCitizens.Count;
                unemploymentText.text = "Unemployment: " + (PopulationManager.Instance.unemploymentRate * 100f).ToString("F1") + "%";
                avgSatisfactionText.text = "Avg Satisfaction: " + PopulationManager.Instance.averageSatisfaction.ToString("F1") + "%";
            }
        }
    }
}
