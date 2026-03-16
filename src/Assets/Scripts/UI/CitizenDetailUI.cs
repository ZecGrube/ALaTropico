using UnityEngine;
using UnityEngine.UI;
using CaudilloBay.AI;

namespace CaudilloBay.UI
{
    public class CitizenDetailUI : MonoBehaviour
    {
        public Text nameText;
        public Text ageText;
        public Text wealthText;
        public Text statusText;
        public Slider satisfactionSlider;

        public void DisplayCitizen(Citizen citizen)
        {
            if (citizen == null) return;

            nameText.text = citizen.firstName + " " + citizen.lastName;
            ageText.text = "Age: " + citizen.age;
            wealthText.text = "Wealth: $" + citizen.personalWealth.ToString("F0");
            statusText.text = "Status: " + citizen.currentState;
            satisfactionSlider.value = citizen.satisfaction / 100f;
        }
    }
}
